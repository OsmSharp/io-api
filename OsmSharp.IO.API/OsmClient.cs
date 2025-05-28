using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OsmSharp.API;
using OsmSharp.Streams;
using OsmSharp.Streams.Complete;
using OsmSharp.Complete;
using System.Xml.Serialization;
using OsmSharp.Changesets;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web;
using Microsoft.Extensions.Logging;
using System.Globalization;
using OsmSharp.IO.Xml;
using OsmSharp.Tags;

namespace OsmSharp.IO.API
{
    public class OsmClient : IAuthClient
    {
        /// <summary>
        /// The OSM base address
        /// </summary>
        /// <example>
        /// "https://master.apis.dev.openstreetmap.org/api/"
        /// "https://www.openstreetmap.org/api/"
        /// </example>
        private readonly string _baseAddress;

        /// <summary>
        /// The OSM OAuth2 bearer token used for authentication
        /// </summary>
        private readonly string _token;
        
        // Prevent scientific notation in a url.
        private const string OsmMaxPrecision = "0.########";

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates an instance of a NonAuthClient which can make
        /// unauthenticated (generally read-only) calls to the OSM API.
        /// </summary>
        /// <param name="baseAddress">The base address for the OSM API (for example: 'https://www.openstreetmap.org/api/0.6/')</param>
        /// <param name="httpClient">An HttpClient</param>
        /// <param name="token">OAuth2 bearer token</param>
        /// <param name="logger">For logging out details of requests. Optional.</param>
        public OsmClient(string baseAddress,
            HttpClient httpClient,
            string token = null,
            ILogger logger = null)
        {
            _baseAddress = baseAddress;
            _httpClient = httpClient;
            _logger = logger;
            _token = token;
        }

        #region Miscellaneous
        /// <inheritdoc />
        public async Task<double?> GetVersions()
        {
            var osm = await Get<Osm>(_baseAddress + "versions");
            return osm.Api.Version.Maximum;
        }

        /// <inheritdoc />
        public async Task<Osm> GetCapabilities()
        {
            return await Get<Osm>(_baseAddress + "0.6/capabilities");
        }

        /// <inheritdoc />
        public async Task<Osm> GetMap(Bounds bounds)
        {
            Validate.BoundLimits(bounds);
            var address = _baseAddress + $"0.6/map?bbox={ToString(bounds)}";

            return await Get<Osm>(address);
        }
        
        #endregion

        #region Elements
        /// <inheritdoc />
        public Task<CompleteWay> GetCompleteWay(long id)
        {
            return GetCompleteElement<CompleteWay>(id);
        }

        /// <inheritdoc />
        public Task<CompleteRelation> GetCompleteRelation(long id)
        {
            return GetCompleteElement<CompleteRelation>(id);
        }

        /// <summary>
        /// Element Full
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Full:_GET_.2Fapi.2F0.6.2F.5Bway.7Crelation.5D.2F.23id.2Ffull">
        /// GET /api/0.6/[way|relation]/#id/full</see>.
        /// </summary>
        private async Task<TCompleteOsmGeo> GetCompleteElement<TCompleteOsmGeo>(long id) where TCompleteOsmGeo : ICompleteOsmGeo, new()
        {
            var type = new TCompleteOsmGeo().Type.ToString().ToLower();
            var address = _baseAddress + $"0.6/{type}/{id}/full";
            var content = await Get(address);
            var stream = await content.ReadAsStreamAsync();
            var streamSource = new XmlOsmStreamSource(stream);
            var completeSource = new OsmSimpleCompleteStreamSource(streamSource);
            var element = completeSource.OfType<TCompleteOsmGeo>().FirstOrDefault();
            return element;
        }

        /// <inheritdoc />
        public async Task<Node> GetNode(long id)
        {
            return await GetElement<Node>(id);
        }

        /// <inheritdoc />
        public async Task<Way> GetWay(long id)
        {
            return await GetElement<Way>(id);
        }

        /// <inheritdoc />
        public async Task<Relation> GetRelation(long id)
        {
            return await GetElement<Relation>(id);
        }

        /// <summary>
        /// Element Read
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id">
        /// GET /api/0.6/[node|way|relation]/#id</see>.
        /// </summary>
        private async Task<TOsmGeo> GetElement<TOsmGeo>(long id) where TOsmGeo : OsmGeo, new()
        {
            var type = new TOsmGeo().Type.ToString().ToLower();
            var address = _baseAddress + $"0.6/{type}/{id}";
            var elements = await GetOfType<TOsmGeo>(address);
            return elements.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<Node[]> GetNodeHistory(long id)
        {
            return await GetElementHistory<Node>(id);
        }

        /// <inheritdoc />
        public async Task<Way[]> GetWayHistory(long id)
        {
            return await GetElementHistory<Way>(id);
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetRelationHistory(long id)
        {
            return await GetElementHistory<Relation>(id);
        }

        /// <summary>
        /// Element History
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#History:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Fhistory">
        /// GET /api/0.6/[node|way|relation]/#id/history</see>.
        /// </summary>
        private async Task<TOsmGeo[]> GetElementHistory<TOsmGeo>(long id) where TOsmGeo : OsmGeo, new()
        {
            var type = new TOsmGeo().Type.ToString().ToLower();
            var address = _baseAddress + $"0.6/{type}/{id}/history";
            var elements = await GetOfType<TOsmGeo>(address);
            return elements.ToArray();
        }

        /// <inheritdoc />
        public async Task<Node> GetNodeVersion(long id, long version)
        {
            return await GetElementVersion<Node>(id, version);
        }

        /// <inheritdoc />
        public async Task<Way> GetWayVersion(long id, long version)
        {
            return await GetElementVersion<Way>(id, version);
        }

        /// <inheritdoc />
        public async Task<Relation> GetRelationVersion(long id, long version)
        {
            return await GetElementVersion<Relation>(id, version);
        }

        /// <summary>
        /// Element Version
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Version:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2F.23version">
        /// GET /api/0.6/[node|way|relation]/#id/#version</see>.
        /// </summary>
        private async Task<TOsmGeo> GetElementVersion<TOsmGeo>(long id, long version) where TOsmGeo : OsmGeo, new()
        {
            var type = new TOsmGeo().Type.ToString().ToLower();
            var address = _baseAddress + $"0.6/{type}/{id}/{version}";
            var elements = await GetOfType<TOsmGeo>(address);
            return elements.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<Node[]> GetNodes(params long[] ids)
        {
            return await GetElements<Node>(ids);
        }

        /// <inheritdoc />
        public async Task<Way[]> GetWays(params long[] ids)
        {
            return await GetElements<Way>(ids);
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetRelations(params long[] ids)
        {
            return await GetElements<Relation>(ids);
        }

        private async Task<TOsmGeo[]> GetElements<TOsmGeo>(params long[] ids) where TOsmGeo : OsmGeo, new()
        {
            var idVersions = ids.Select(id => new KeyValuePair<long, long?>(id, null));
            return await GetElements<TOsmGeo>(idVersions);
        }

        /// <inheritdoc />
        public async Task<Node[]> GetNodes(IEnumerable<KeyValuePair<long, long?>> idVersions)
        {
            return await GetElements<Node>(idVersions);
        }

        /// <inheritdoc />
        public async Task<Way[]> GetWays(IEnumerable<KeyValuePair<long, long?>> idVersions)
        {
            return await GetElements<Way>(idVersions);
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetRelations(IEnumerable<KeyValuePair<long, long?>> idVersions)
        {
            return await GetElements<Relation>(idVersions);
        }

        public async Task<OsmGeo[]> GetElements(params OsmGeoKey[] elementKeys)
        {
            return await GetElements(elementKeys.ToDictionary(ek => ek, ek => (long?)null));
        }
        
        /// <inheritdoc />
        public async Task<OsmGeo[]> GetElements(Dictionary<OsmGeoKey, long?> elementKeyVersions)
        {
            var elements = new List<OsmGeo>();

            foreach (var typeGroup in elementKeyVersions.GroupBy(kvp => kvp.Key.Type))
            {
                var chunkAsDictionary = typeGroup.ToDictionary(kvp => kvp.Key.Id, kvp => kvp.Value);
                if(typeGroup.Key == OsmGeoType.Node)
                    elements.AddRange(await GetElements<Node>(chunkAsDictionary));
                else if (typeGroup.Key == OsmGeoType.Way)
                    elements.AddRange(await GetElements<Way>(chunkAsDictionary));
                else if (typeGroup.Key == OsmGeoType.Relation)
                    elements.AddRange(await GetElements<Relation>(chunkAsDictionary));
            }

            return elements.ToArray();
        }

        
        private async Task<TOsmGeo[]> GetElements<TOsmGeo>(IEnumerable<KeyValuePair<long, long?>> idVersions) where TOsmGeo : OsmGeo, new()
        {
            var tasks = new List<Task<IEnumerable<TOsmGeo>>>();

            foreach (var chunk in Chunks(idVersions, 400)) // to avoid http error code 414, UIR too long.
            {
                var type = new TOsmGeo().Type.ToString().ToLower();
                // For exmple: "12,13,14v1,15v1"
                var parameters = string.Join(",", chunk.Select(e => e.Value.HasValue ? $"{e.Key}v{e.Value}" : e.Key.ToString()));
                var address = _baseAddress + $"0.6/{type}s?{type}s={parameters}";
                tasks.Add(GetOfType<TOsmGeo>(address));
            }

            await Task.WhenAll(tasks);

            return tasks.SelectMany(t => t.Result).ToArray();
        }

        private IEnumerable<T[]> Chunks<T>(IEnumerable<T> elements, int chunkSize)
        {
            return elements.Select((e, i) => new { e, i })
                .GroupBy(ei => ei.i / chunkSize) // Intentional integer division
                .Select(g => g.Select(ei => ei.e).ToArray());
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetNodeRelations(long id)
        {
            return await GetElementRelations<Node>(id);
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetWayRelations(long id)
        {
            return await GetElementRelations<Way>(id);
        }

        /// <inheritdoc />
        public async Task<Relation[]> GetRelationRelations(long id)
        {
            return await GetElementRelations<Relation>(id);
        }

        /// <summary>
        /// Element Relations
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Relations_for_element:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Frelations">
        /// GET /api/0.6/[node|way|relation]/#id/relations</see>.
        /// </summary>
        private async Task<Relation[]> GetElementRelations<TOsmGeo>(long id) where TOsmGeo : OsmGeo, new()
        {
            var type = new TOsmGeo().Type.ToString().ToLower();
            var address = _baseAddress + $"0.6/{type}/{id}/relations";
            var elements = await GetOfType<Relation>(address);
            return elements.ToArray();
        }

        /// <inheritdoc />
        public async Task<Way[]> GetNodeWays(long id)
        {
            var address = _baseAddress + $"0.6/node/{id}/ways";
            var elements = await GetOfType<Way>(address);
            return elements.ToArray();
        }
        #endregion

        #region Changesets
        /// <inheritdoc />
        public async Task<Changeset> GetChangeset(long changesetId, bool includeDiscussion = false)
        {
            var address = _baseAddress + $"0.6/changeset/{changesetId}";
            if (includeDiscussion)
            {
                address += "?include_discussion=true";
            }
            var osm = await Get<Osm>(address);
            return osm.Changesets[0];
        }

        /// <inheritdoc />
        public async Task<Changeset[]> QueryChangesets(Bounds bounds, long? userId, string userName,
            DateTime? minClosedDate, DateTime? maxOpenedDate, bool openOnly, bool closedOnly,
            long[] ids)
        {
            if (userId.HasValue && userName != null)
                throw new ArgumentException("Query can only specify userID OR userName, not both.");
            if (openOnly && closedOnly)
                throw new ArgumentException("Query can only specify openOnly OR closedOnly, not both.");
            if (!minClosedDate.HasValue && maxOpenedDate.HasValue)
                throw new ArgumentException("Query must specify minClosedDate if maxOpenedDate is specified.");

            var query = HttpUtility.ParseQueryString(string.Empty);
            if (bounds != null) query["bbox"] = ToString(bounds);
            if (userId.HasValue) query["user"] = userId.ToString();
            if (userName != null) query["display_name"] = userName;
            if (minClosedDate.HasValue) query["time"] = FormatNoteDate(minClosedDate.Value);
            if (maxOpenedDate.HasValue) query["time"] += "," + FormatNoteDate(maxOpenedDate.Value);
            if (openOnly) query["open"] = "true";
            if (closedOnly) query["closed"] = "true";
            if (ids != null) query["changesets"] = string.Join(",", ids);

            var address = _baseAddress + "0.6/changesets?" + query.ToString();
            var osm = await Get<Osm>(address);
            return osm.Changesets;
        }

        /// <inheritdoc />
        public async Task<OsmChange> GetChangesetDownload(long changesetId)
        {
            return await Get<OsmChange>(_baseAddress + $"0.6/changeset/{changesetId}/download");
        }
        #endregion

        #region Changesets and Element Changes
        /// <inheritdoc />
        public async Task<long> CreateChangeset(TagsCollectionBase tags)
        {
            Validate.ContainsTags(tags, "comment", "created_by");
            var address = _baseAddress + "0.6/changeset/create";
            var changeSet = new Osm { Changesets = new[] { new Changeset { Tags = tags } } };
            var content = new StringContent(changeSet.SerializeToXml());
            var resultContent = await SendAuthRequest(HttpMethod.Put, address, content);
            var id = await resultContent.ReadAsStringAsync();
            return long.Parse(id);
        }

        /// <inheritdoc />
        public async Task<Changeset> UpdateChangeset(long changesetId, TagsCollectionBase tags)
        {
            Validate.ContainsTags(tags, "comment", "created_by");
            // TODO: Validate change meets OsmSharp.API.Capabilities?
            var address = _baseAddress + $"0.6/changeset/{changesetId}";
            var changeSet = new Osm { Changesets = new[] { new Changeset { Tags = tags } } };
            var content = new StringContent(changeSet.SerializeToXml());
            var osm = await Put<Osm>(address, content);
            return osm.Changesets[0];
        }

        /// <inheritdoc />
        public async Task<DiffResult> UploadChangeset(long changesetId, OsmChange osmChange)
        {
            var elements = new OsmGeo[][] { osmChange.Create, osmChange.Modify, osmChange.Delete }
                .Where(c => c != null).SelectMany(c => c);

            foreach (var osmGeo in elements)
            {
                osmGeo.ChangeSetId = changesetId;
            }

            var address = _baseAddress + $"0.6/changeset/{changesetId}/upload";
            var request = new StringContent(osmChange.SerializeToXml());

            return await Post<DiffResult>(address, request);
        }

        /// <inheritdoc />
        public async Task<long> CreateElement(long changesetId, OsmGeo osmGeo)
        {
            var address = _baseAddress + $"0.6/{osmGeo.Type.ToString().ToLower()}/create";
            var osmRequest = GetOsmRequest(changesetId, osmGeo);
            var content = new StringContent(osmRequest.SerializeToXml());
            var response = await SendAuthRequest(HttpMethod.Put, address, content);
            var id = await response.ReadAsStringAsync();
            return long.Parse(id);
        }

        /// <inheritdoc />
        public async Task<long> UpdateElement(long changesetId, ICompleteOsmGeo osmGeo)
        {
            switch (osmGeo.Type)
            {
                case OsmGeoType.Node:
                    return await UpdateElement(changesetId, osmGeo as OsmGeo);
                case OsmGeoType.Way:
                    return await UpdateElement(changesetId, ((CompleteWay)osmGeo).ToSimple());
                case OsmGeoType.Relation:
                    return await UpdateElement(changesetId, ((CompleteRelation)osmGeo).ToSimple());
                default:
                    throw new Exception($"Invalid OSM geometry type: {osmGeo.Type}");
            }
        }

        /// <inheritdoc />
        public async Task<long> UpdateElement(long changesetId, OsmGeo osmGeo)
        {
            Validate.ElementHasAVersion(osmGeo);
            var address = _baseAddress + $"0.6/{osmGeo.Type.ToString().ToLower()}/{osmGeo.Id}";
            var osmRequest = GetOsmRequest(changesetId, osmGeo);
            var content = new StringContent(osmRequest.SerializeToXml());
            var responseContent = await SendAuthRequest(HttpMethod.Put, address, content);
            var newVersionNumber = await responseContent.ReadAsStringAsync();
            return long.Parse(newVersionNumber);
        }

        /// <inheritdoc />
        public async Task<long> DeleteElement(long changesetId, OsmGeo osmGeo)
        {
            Validate.ElementHasAVersion(osmGeo);
            var address = _baseAddress + $"0.6/{osmGeo.Type.ToString().ToLower()}/{osmGeo.Id}";
            var osmRequest = GetOsmRequest(changesetId, osmGeo);
            var content = new StringContent(osmRequest.SerializeToXml());
            var responseContent = await SendAuthRequest(HttpMethod.Delete, address, content);
            var newVersionNumber = await responseContent.ReadAsStringAsync();
            return long.Parse(newVersionNumber);
        }

        /// <inheritdoc />
        public async Task CloseChangeset(long changesetId)
        {
            var address = _baseAddress + $"0.6/changeset/{changesetId}/close";
            await SendAuthRequest(HttpMethod.Put, address, new StringContent(""));
        }

        /// <inheritdoc />
        public async Task<Changeset> AddChangesetComment(long changesetId, string text)
        {
            var address = _baseAddress + $"0.6/changeset/{changesetId}/comment";
            var content = new MultipartFormDataContent() { { new StringContent(text), "text" } };
            var osm = await Post<Osm>(address, content);
            return osm.Changesets[0];
        }

        /// <inheritdoc />
        public async Task ChangesetSubscribe(long changesetId)
        {
            var address = _baseAddress + $"0.6/changeset/{changesetId}/subscribe";
            await SendAuthRequest(HttpMethod.Post, address, new StringContent(""));
        }

        /// <inheritdoc />
        public async Task ChangesetUnsubscribe(long changesetId)
        {
            var address = _baseAddress + $"0.6/changeset/{changesetId}/unsubscribe";
            await SendAuthRequest(HttpMethod.Post, address, new StringContent(""));
        }
        #endregion
        
        #region Traces
        /// <inheritdoc />
        public async Task<Stream> GetTrackPoints(Bounds bounds, int pageNumber = 0)
        {
            var address = _baseAddress + $"0.6/trackpoints?bbox={ToString(bounds)}&page={pageNumber}";
            var content = await Get(address);
            var stream = await content.ReadAsStreamAsync();
            return stream;
        }

        /// <inheritdoc />
        public async Task<GpxFile> GetTraceDetails(long id)
        {
            var address = _baseAddress + $"0.6/gpx/{id}/details";
            var osm = await Get<Osm>(address, c => AddAuthentication(c, address));
            return osm.GpxFiles[0];
        }

        /// <inheritdoc />
        public async Task<TypedStream> GetTraceData(long id)
        {
            var address = _baseAddress + $"0.6/gpx/{id}/data";
            var content = await Get(address, c => AddAuthentication(c, address));
            return await TypedStream.Create(content);
        }
        
        /// <inheritdoc />
        public async Task<GpxFile[]> GetTraces()
        {
            var address = _baseAddress + "0.6/user/gpx_files";
            var osm = await Get<Osm>(address, c => AddAuthentication(c, address));
            return osm.GpxFiles ?? Array.Empty<GpxFile>();
        }

        /// <inheritdoc />
        public async Task<long> CreateTrace(GpxFile gpx, Stream fileStream)
        {
            var address = _baseAddress + "0.6/gpx/create";
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(gpx.Description), "\"description\"");
            form.Add(new StringContent(gpx.Visibility.ToString().ToLower()), "\"visibility\"");
            var tags = string.Join(",", gpx.Tags ?? Array.Empty<string>());
            form.Add(new StringContent(tags), "\"tags\"");
            var stream = new StreamContent(fileStream);
            var cleanName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(gpx.Name));
            form.Add(stream, "file", cleanName);
            var content = await SendAuthRequest(HttpMethod.Post, address, form);
            var id = await content.ReadAsStringAsync();
            return long.Parse(id);
        }

        /// <inheritdoc />
        public async Task UpdateTrace(GpxFile trace)
        {
            var address = _baseAddress + $"0.6/gpx/{trace.Id}";
            var osm = new Osm { GpxFiles = new[] { trace } };
            var content = new StringContent(osm.SerializeToXml());
            await SendAuthRequest(HttpMethod.Put, address, content);
        }

        /// <inheritdoc />
        public async Task DeleteTrace(long traceId)
        {
            var address = _baseAddress + $"0.6/gpx/{traceId}";
            await SendAuthRequest(HttpMethod.Delete, address, null);
        }
        #endregion

        #region Notes
        /// <inheritdoc />
        public async Task<Note> GetNote(long id)
        {
            var address = _baseAddress + $"0.6/notes/{id}";
            var osm = await Get<Osm>(address);
            return osm.Notes?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<Note[]> GetNotes(Bounds bounds, int limit = 100, int maxClosedDays = 7)
        {
            string format = ".xml";
            var address = _baseAddress + $"0.6/notes{format}?bbox={ToString(bounds)}&limit={limit}&closed={maxClosedDays}";
            var osm = await Get<Osm>(address);
            return osm.Notes;
        }

        /// <inheritdoc />
        public async Task<Stream> GetNotesRssFeed(Bounds bounds)
        {
            var address = _baseAddress + $"0.6/notes/feed?bbox={ToString(bounds)}";
            var content = await Get(address);
            var stream = await content.ReadAsStreamAsync();
            return stream;
        }

        /// <inheritdoc />
        public async Task<Note[]> QueryNotes(string searchText, long? userId, string userName,
            int? limit, int? maxClosedDays, DateTime? fromDate, DateTime? toDate)
        {
            if (userId.HasValue && userName != null)
                throw new ArgumentException("Query can only specify userID OR userName, not both.");
            if (fromDate > toDate)
                throw new ArgumentException("Query [fromDate] must be before [toDate] if both are provided.");
            if (searchText == null)
                throw new ArgumentException("Query searchText is required.");

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["q"] = searchText;
            if (limit != null) query["limit"] = limit.ToString();
            if (maxClosedDays != null) query["closed"] = maxClosedDays.ToString();
            if (userName != null) query["display_name"] = userName;
            if (userId != null) query["user"] = userId.ToString();
            if (fromDate != null) query["from"] = FormatNoteDate(fromDate.Value);
            if (toDate != null) query["to"] = FormatNoteDate(toDate.Value);

            string format = ".xml";
            var address = _baseAddress + $"0.6/notes/search{format}?{query}";
            var osm = await Get<Osm>(address);
            return osm.Notes;
        }

        private static string FormatNoteDate(DateTime date)
        {
            // DateTimes in notes are 'different'.
            return date.ToString("yyyy-MM-dd HH:mm:ss") + " UTC";
        }

        /// <inheritdoc />
        public async Task<Note> CreateNote(double latitude, double longitude, string text)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["text"] = text;
            query["lat"] = ToString(latitude);
            query["lon"] = ToString(longitude);

            var address = _baseAddress + $"0.6/notes?{query}";
            // Can be with Auth or without.
            var osm = await Post<Osm>(address);
            return osm.Notes[0];
        }
        
        /// <inheritdoc />
        public async Task<Note> CommentNote(long noteId, string text)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["text"] = text;
            var address = _baseAddress + $"0.6/notes/{noteId}/comment?{query}";
            // Can be with Auth or without.
            var osm = await Post<Osm>(address);
            return osm.Notes[0];
        }

        /// <inheritdoc />
        public async Task<Note> CloseNote(long noteId, string text)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["text"] = text;
            var address = _baseAddress + $"0.6/notes/{noteId}/close?{query}";
            var osm = await Post<Osm>(address);
            return osm.Notes[0];
        }

        /// <inheritdoc />
        public async Task<Note> ReOpenNote(long noteId, string text)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["text"] = text;
            var address = _baseAddress + $"0.6/notes/{noteId}/reopen?{query}";
            var osm = await Post<Osm>(address);
            return osm.Notes[0];
        }
        #endregion

        #region Http
        private static readonly Func<string, string> Encode = HttpUtility.UrlEncode;
        
        private void AddAuthentication(HttpRequestMessage message, string url, string method = "GET")
        {
            if (!string.IsNullOrEmpty(_token))
            {
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);    
            }
        }
        
        private async Task<T> Get<T>(string address, Action<HttpRequestMessage> auth = null) where T : class
        {
            var content = await Get(address, auth);
            var stream = await content.ReadAsStreamAsync();
            var serializer = new XmlSerializer(typeof(T));
            var element = serializer.Deserialize(stream) as T;
            return element;
        }

        private async Task<HttpContent> Get(string address, Action<HttpRequestMessage> auth = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, address))
            {
                auth?.Invoke(request);
                var response = await _httpClient.SendAsync(request);
                await VerifyAndLogResponse(response, $"{GetType().Name} GET: {address}");
                return response.Content;
            }
        }

        private async Task<T> Post<T>(string address, HttpContent requestContent = null) where T : class
        {
            var responseContent = await SendAuthRequest(HttpMethod.Post, address, requestContent);
            var stream = await responseContent.ReadAsStreamAsync();
            var serializer = new XmlSerializer(typeof(T));
            var content = serializer.Deserialize(stream) as T;
            return content;
        }

        private async Task<T> Put<T>(string address, HttpContent requestContent = null) where T : class
        {
            var content = await SendAuthRequest(HttpMethod.Put, address, requestContent);
            var stream = await content.ReadAsStreamAsync();
            var serializer = new XmlSerializer(typeof(T));
            var element = serializer.Deserialize(stream) as T;
            return element;
        }

        private async Task<HttpContent> SendAuthRequest(HttpMethod method, string address, HttpContent requestContent)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(method, address))
            {
                AddAuthentication(request, address, method.ToString());
                request.Content = requestContent;
                var response = await _httpClient.SendAsync(request);
                await VerifyAndLogResponse(response, $"{GetType().Name} {method}: {address}");
                return response.Content;
            }
        }

        private async Task VerifyAndLogResponse(HttpResponseMessage response, string logMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger?.LogError($"{logMessage}: failed: {response.StatusCode}-{response.ReasonPhrase} {message}");
                throw new OsmApiException(response.RequestMessage?.RequestUri, message, response.StatusCode);
            }
            else
            {
                _logger?.LogInformation($"{logMessage}: succeeded");
            }
        }
        #endregion
        
        #region Users
        /// <summary>
        /// Details of a User
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Details_of_a_user">
        /// GET /api/0.6/user/#id</see>.
        /// </summary>
        public async Task<User> GetUser(long id)
        {
            var address = _baseAddress + $"0.6/user/{id}";
            var osm = await Get<Osm>(address);
            return osm.User;
        }

        /// <summary>
        /// Details of multiple Users
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Details_of_multiple_users">
        /// GET /api/0.6/users?users=#id1,#id2,...,#idn</see>.
        /// </summary>
        public async Task<User[]> GetUsers(params long[] ids)
        {
            var address = _baseAddress + $"0.6/users?users={string.Join(",", ids)}";
            var osm = await Get<Osm>(address);
            return osm.Users;
        }
        
        /// <inheritdoc />
        public async Task<Permissions> GetPermissions()
        {
            var address = _baseAddress + "0.6/permissions";
            var osm = await Get<Osm>(address, c => AddAuthentication(c, address));
            return osm.Permissions;
        }

        /// <inheritdoc />
        public async Task<User> GetUserDetails()
        {
            var address = _baseAddress + "0.6/user/details";
            var osm = await Get<Osm>(address, c => AddAuthentication(c, address));
            return osm.User;
        }

        /// <inheritdoc />
        public async Task<Preference[]> GetUserPreferences()
        {
            var address = _baseAddress + "0.6/user/preferences";
            var osm = await Get<Osm>(address, c => AddAuthentication(c, address));
            return osm.Preferences.UserPreferences;
        }

        /// <inheritdoc />
        public async Task SetUserPreferences(Preferences preferences)
        {
            var address = _baseAddress + "0.6/user/preferences";
            var osm = new Osm() { Preferences = preferences };
            var content = new StringContent(osm.SerializeToXml());
            await SendAuthRequest(HttpMethod.Put, address, content);
        }

        /// <inheritdoc />
        public async Task<string> GetUserPreference(string key)
        {
            var address = _baseAddress + $"0.6/user/preferences/{Encode(key)}";
            var content = await Get(address, c => AddAuthentication(c, address));
            var value = await content.ReadAsStringAsync();
            return value;
        }

        /// <inheritdoc />
        public async Task SetUserPreference(string key, string value)
        {
            var address = _baseAddress + $"0.6/user/preferences/{Encode(key)}";
            var content = new StringContent(value);
            await SendAuthRequest(HttpMethod.Put, address, content);
        }

        /// <inheritdoc />
        public async Task DeleteUserPreference(string key)
        {
            var address = _baseAddress + $"0.6/user/preferences/{Encode(key)}";
            await SendAuthRequest(HttpMethod.Delete, address, null);
        }
        #endregion
        
        private Osm GetOsmRequest(long changesetId, OsmGeo osmGeo)
        {
            osmGeo.ChangeSetId = changesetId;
            var osm = new Osm();
            switch (osmGeo.Type)
            {
                case OsmGeoType.Node:
                    osm.Nodes = new[] { osmGeo as Node };
                    break;
                case OsmGeoType.Way:
                    osm.Ways = new[] { osmGeo as Way };
                    break;
                case OsmGeoType.Relation:
                    osm.Relations = new[] { osmGeo as Relation };
                    break;
            }
            return osm;
        }
        
        private async Task<IEnumerable<T>> GetOfType<T>(string address, Action<HttpRequestMessage> auth = null) where T : class
        {
            var content = await Get(address, auth);
            var streamSource = new XmlOsmStreamSource(await content.ReadAsStreamAsync());
            var elements = streamSource.OfType<T>();
            return elements;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ToString(Bounds bounds)
        {
            StringBuilder x = new StringBuilder();
            x.Append(bounds.MinLongitude.Value.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture));
            x.Append(',');
            x.Append(bounds.MinLatitude.Value.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture));
            x.Append(',');
            x.Append(bounds.MaxLongitude.Value.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture));
            x.Append(',');
            x.Append(bounds.MaxLatitude.Value.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture));

            return x.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ToString(float number)
        {
            return number.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ToString(double number)
        {
            return number.ToString(OsmMaxPrecision, CultureInfo.InvariantCulture);
        }
    }
}

