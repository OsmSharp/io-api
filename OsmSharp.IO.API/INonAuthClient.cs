using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OsmSharp.API;
using OsmSharp.Changesets;
using OsmSharp.Complete;

namespace OsmSharp.IO.API
{
    public interface INonAuthClient
    {
        /// <summary>
        /// Available API versions
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Available_API_versions:_GET_.2Fapi.2Fversions">
        /// GET /api/versions</see>.
        /// </summary>
        Task<double?> GetVersions();
        /// <summary>
        /// API Capabilities
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Capabilities:_GET_.2Fapi.2Fcapabilities">
        /// GET /api/capabilities</see>.
        /// </summary>
        Task<Osm> GetCapabilities();
        /// <summary>
        /// Retrieving map data by bounding box
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Retrieving_map_data_by_bounding_box:_GET_.2Fapi.2F0.6.2Fmap">
        /// GET /api/0.6/map</see>.
        /// </summary>
        Task<Osm> GetMap(Bounds bounds);
        Task<User> GetUser(long id);
        Task<User[]> GetUsers(params long[] ids);
        /// <summary>
        /// Gets a Way, including the details of each Node in it
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Full:_GET_.2Fapi.2F0.6.2F.5Bway.7Crelation.5D.2F.23id.2Ffull">
        /// GET /api/0.6/way/#id/full</see>.
        /// </summary>
        Task<CompleteWay> GetCompleteWay(long id);
        /// <summary>
        /// Gats a Relation, including the details of each Element in it
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Full:_GET_.2Fapi.2F0.6.2F.5Bway.7Crelation.5D.2F.23id.2Ffull">
        /// GET /api/0.6/relation/#id/full</see>.
        /// </summary>
        Task<CompleteRelation> GetCompleteRelation(long id);
        /// <summary>
        /// Gets a Node and its details
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id">
        /// GET /api/0.6/node/#id</see>.
        /// </summary>
        Task<Node> GetNode(long id);
        /// <summary>
        /// Gets a Way and its details (but not the details of its Nodes)
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id">
        /// GET /api/0.6/way/#id</see>.
        /// </summary>
        Task<Way> GetWay(long id);
        /// <summary>
        /// Gets a Relation and its details (but not the details of its elements)
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id">
        /// GET /api/0.6/relation/#id</see>.
        /// </summary>
        Task<Relation> GetRelation(long id);
        /// <summary>
        /// Gets a Node's history
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#History:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Fhistory">
        /// GET /api/0.6/node/#id/history</see>.
        /// </summary>
        Task<Node[]> GetNodeHistory(long id);
        /// <summary>
        /// Gets a Way's history
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#History:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Fhistory">
        /// GET /api/0.6/way/#id/history</see>.
        /// </summary>
        Task<Way[]> GetWayHistory(long id);
        /// <summary>
        /// Gets a Relation's history
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#History:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Fhistory">
        /// GET /api/0.6/relation/#id/history</see>.
        /// </summary>
        Task<Relation[]> GetRelationHistory(long id);
        /// <summary>
        /// Gets a Node's version
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Version:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2F.23version">
        /// GET /api/0.6/node/#id/#version</see>.
        /// </summary>
        Task<Node> GetNodeVersion(long id, long version);
        /// <summary>
        /// Gets a Way's version
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Version:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2F.23version">
        /// GET /api/0.6/way/#id/#version</see>.
        /// </summary>
        Task<Way> GetWayVersion(long id, long version);
        /// <summary>
        /// Gets a Relation's version
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Version:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2F.23version">
        /// GET /api/0.6/relation/#id/#version</see>.
        /// </summary>
        Task<Relation> GetRelationVersion(long id, long version);
        /// <summary>
        /// Gets many Nodes
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/nodes?#parameters</see>.
        /// </summary>
        Task<Node[]> GetNodes(params long[] ids);
        /// <summary>
        /// Gets many Ways
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/ways?#parameters</see>.
        /// </summary>
        Task<Way[]> GetWays(params long[] ids);
        /// <summary>
        /// Gets many Relations
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/relations?#parameters</see>.
        /// </summary>
        Task<Relation[]> GetRelations(params long[] ids);
        Task<OsmGeo[]> GetElements(params OsmGeoKey[] elementKeys);
        /// <summary>
        /// Elements Multifetch
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/[nodes|ways|relations]?#parameters</see>.
        /// </summary>
        Task<OsmGeo[]> GetElements(Dictionary<OsmGeoKey, long?> elementKeyVersions);
        /// <summary>
        /// Gets many Nodes at specific versions
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/nodes?#parameters</see>.
        /// </summary>
        Task<Node[]> GetNodes(IEnumerable<KeyValuePair<long, long?>> idVersions);
        /// <summary>
        /// Gets many Ways at specific versions
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/ways?#parameters</see>.
        /// </summary>
        Task<Way[]> GetWays(IEnumerable<KeyValuePair<long, long?>> idVersions);
        /// <summary>
        /// Gets many Relations at specific versions
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Multi_fetch:_GET_.2Fapi.2F0.6.2F.5Bnodes.7Cways.7Crelations.5D.3F.23parameters">
        /// GET /api/0.6/relations?#parameters</see>.
        /// </summary>
        Task<Relation[]> GetRelations(IEnumerable<KeyValuePair<long, long?>> idVersions);
        /// <summary>
        /// Gets the Relations containing a specific Node
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Relations_for_element:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Frelations">
        /// GET /api/0.6/node/#id/relations</see>.
        /// </summary>
        Task<Relation[]> GetNodeRelations(long id);
        /// <summary>
        /// Gets the Relations containing a specific Way
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Relations_for_element:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Frelations">
        /// GET /api/0.6/way/#id/relations</see>.
        /// </summary>
        Task<Relation[]> GetWayRelations(long id);
        /// <summary>
        /// Gets the Relations containing a specific Relation
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Relations_for_element:_GET_.2Fapi.2F0.6.2F.5Bnode.7Cway.7Crelation.5D.2F.23id.2Frelations">
        /// GET /api/0.6/relation/#id/relations</see>.
        /// </summary>
        Task<Relation[]> GetRelationRelations(long id);
        /// <summary>
        /// Node Ways
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Ways_for_node:_GET_.2Fapi.2F0.6.2Fnode.2F.23id.2Fways">
        /// GET /api/0.6/node/#id/ways</see>.
        /// </summary>
        Task<Way[]> GetNodeWays(long id);
        /// <summary>
        /// Changeset Read
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2Fchangeset.2F.23id.3Finclude_discussion.3Dtrue">
        /// GET /api/0.6/changeset/#id?include_discussion=true</see>.
        /// </summary>
        Task<Changeset> GetChangeset(long changesetId, bool includeDiscussion = false);
        /// <summary>
        /// Changeset Query
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Query:_GET_.2Fapi.2F0.6.2Fchangesets">
        /// GET /api/0.6/changesets</see>
        /// </summary>
        Task<Changeset[]> QueryChangesets(Bounds bounds, long? userId, string userName, DateTime? minClosedDate, DateTime? maxOpenedDate, bool openOnly, bool closedOnly, long[] ids);
        /// <summary>
        /// Changeset Download
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2Fchangeset.2F.23id.3Finclude_discussion.3Dtrue">
        /// GET /api/0.6/changeset/#id/download</see>
        /// </summary>
        Task<OsmChange> GetChangesetDownload(long changesetId);
        /// <summary>
        /// Get GPS Points
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Get_GPS_Points:_Get_.2Fapi.2F0.6.2Ftrackpoints.3Fbbox.3Dleft.2Cbottom.2Cright.2Ctop.26page.3DpageNumber">
        /// Get /api/0.6/trackpoints?bbox=left,bottom,right,top&page=pageNumber</see>.
        /// Retrieve the GPS track points that are inside a given bounding box (formatted in a GPX format).
        /// Warning: GPX version 1.0 is not the current version. Your tools might not support it.
        /// </summary>
        /// <returns>A stream of a GPX (version 1.0) file.</returns>
        Task<Stream> GetTrackPoints(Bounds bounds, int pageNumber = 0);
        /// <summary>
        /// Download Metadata
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Download_Metadata:_GET_.2Fapi.2F0.6.2Fgpx.2F.23id.2Fdetails">
        /// GET /api/0.6/gpx/#id/details</see>.
        /// </summary>
        Task<GpxFile> GetTraceDetails(long id);
        /// <summary>
        /// Download Data
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Download_Data:_GET_.2Fapi.2F0.6.2Fgpx.2F.23id.2Fdata">
        /// GET /api/0.6/gpx/#id/data</see>.
        /// This will return exactly what was uploaded, which might not be a gpx file (it could be a zip etc.)
        /// </summary>
        /// <returns>A stream of a GPX (version 1.0) file.</returns>
        Task<TypedStream> GetTraceData(long id);
        /// <summary>
        /// Gets a Note
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Read:_GET_.2Fapi.2F0.6.2Fnotes.2F.23id">
        /// GET /api/0.6/notes/#id</see>.
        /// </summary>
        Task<Note> GetNote(long id);
        /// <summary>
        /// Gets many a Notes in a box and with the spcified time since closed
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Retrieving_notes_data_by_bounding_box:_GET_.2Fapi.2F0.6.2Fnotes">
        /// GET /api/0.6/notes?[parameters]</see>.
        /// </summary>
        /// <param name="limit">Must be between 1 and 10,000.</param>
        /// <param name="maxClosedDays">0 means only open notes. -1 mean all (open and closed) notes.</param>
        Task<Note[]> GetNotes(Bounds bounds, int limit = 100, int maxClosedDays = 7);
        /// <summary>
        /// Gets an RSS feed of Notes in an area
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#RSS_Feed:_GET_.2Fapi.2F0.6.2Fnotes.2Ffeed">
        /// GET /api/0.6/notes/feed</see>.
        /// </summary>
        Task<Stream> GetNotesRssFeed(Bounds bounds);
        /// <summary>
        /// Search for Notes
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Search_for_notes:_GET_.2Fapi.2F0.6.2Fnotes.2Fsearch">
        /// GET /api/0.6/notes/search</see>.
        /// </summary>
        /// <param name="searchText">Specifies the search query. This is the only required field.</param>
        /// <param name="userId">Specifies the creator of the returned notes by the id of the user. Does not work together with the display_name parameter</param>
        /// <param name="userName">Specifies the creator of the returned notes by the display name. Does not work together with the user parameter</param>
        /// <param name="limit">Must be between 1 and 10,000. 100 is default if null.</param>
        /// <param name="maxClosedDays">0 means only open notes. -1 mean all (open and closed) notes. 7 is default if null.</param>
        /// <param name="fromDate">Specifies the beginning of a date range to search in for a note</param>
        /// <param name="toDate">Specifies the end of a date range to search in for a note</param>
        Task<Note[]> QueryNotes(string searchText, long? userId, string userName, int? limit, int? maxClosedDays, DateTime? fromDate, DateTime? toDate);
        /// <summary>
        /// Creates a new Note
        /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Create_a_new_note:_Create:_POST_.2Fapi.2F0.6.2Fnotes">
        /// POST /api/0.6/notes</see>.
        /// </summary>
        Task<Note> CreateNote(double latitude, double longitude, string text);
    }
}