using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmSharp.Service.API
{
    /// <summary>
    /// Implements a copy of the OSM-API v0.6 according to the documentation found here:
    /// 
    /// http://wiki.openstreetmap.org/wiki/API_v0.6
    /// </summary>
    public class ApiModule : NancyModule
    {
        /// <summary>
        /// Creates a new api module.
        /// </summary>
        public ApiModule()
        {
            Get["{instance}/api/capabilities"] = _ => { return this.GetCapabilities(_); };
            Get["{instance}/api/0.6/capabilities"] = _ => { return this.GetCapabilities(_); };
            Get["{instance}/api/0.6/map"] = _ => { return this.GetMap(_); };
            Get["{instance}/api/0.6/permissions"] = _ => { return this.GetPermissions(_); };
            Put["{instance}/api/0.6/changeset/create"] = _ => { return this.PutChangesetCreate(_); };
            Get["{instance}/api/0.6/changeset/{changesetid}"] = _ => { return this.GetChangeset(_); };
            Put["{instance}/api/0.6/changeset/{changesetid}"] = _ => { return this.PutChangesetUpdate(_); };
            Get["{instance}/api/0.6/changeset/{changesetid}/download"] = _ => { return this.GetChangesetDownload(_); };
            Post["{instance}/api/0.6/changeset/{changesetid}/expand_bbox"] = _ => { return this.PostChangesetExpandBB(_); };
            Get["{instance}/api/0.6/changesets"] = _ => { return this.GetChangesetQuery(_); };
            Post["{instance}/api/0.6/changeset/{changesetid}/upload"] = _ => { return this.PostChangesetUpload(_); };
            Put["{instance}/api/0.6/[node|way|relation]/create"] = _ => { return this.PutElementCreate(_); };
            Get["{instance}/api/0.6/[node|way|relation]/{elementid}"] = _ => { return this.GetElement(_); };
            Put["{instance}/api/0.6/[node|way|relation]/{elementid}"] = _ => { return this.PutElementUpdate(_); };
            Delete["{instance}/api/0.6/[node|way|relation]/{elementid}"] = _ => { return this.DeleteElement(_); };
            Get["{instance}/api/0.6/[node|way|relation]/{elementid}/history"] = _ => { return this.GetElementHistory(_); };
            Get["{instance}/api/0.6/[node|way|relation]/{elementid}/{version}"] = _ => { return this.GetElementVersion(_); };
            Get["{instance}/api/0.6/[node|way|relation]"] = _ => { return this.GetElementMultiple(_); };
            Get["{instance}/api/0.6/[node|way|relation]/{elementid}/relations"] = _ => { return this.GetElementFull(_); };
            Get["{instance}/api/0.6/node/{id}/ways"] = _ => { return this.GetWaysForNode(_); };
            Get["{instance}/api/0.6/map"] = _ => { return this.GetMap(_); };
            Get["{instance}/api/0.6/map"] = _ => { return this.GetMap(_); };
        }

        /// <summary>
        /// This API call is meant to provide information about the capabilities and limitations of the current API. 
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetCapabilities(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieving map data by bounding box: GET /api/0.6/map
        /// </summary>
        /// <param name="_"></param>
        /// <returns>
        /// - All nodes that are inside a given bounding box and any relations that reference them.
        /// - All ways that reference at least one node that is inside a given bounding box, any relations that reference them [the ways], and any nodes outside the bounding box that the ways may reference.
        /// - All relations that reference one of the nodes, ways or relations included due to the above rules. (Does not apply recursively, see explanation below.)
        /// </returns>
        private dynamic GetMap(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieving permissions: GET /api/0.6/permissions
        /// </summary>
        /// <param name="_"></param>
        /// <returns> 
        /// - If the API client is not authorized, an empty list of permissions will be returned.
        /// - If the API client uses Basic Auth, the list of permissions will contain all permissions.
        /// - If the API client uses OAuth, the list will contain the permissions actually granted by the user.
        /// </returns>
        private dynamic GetPermissions(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create: PUT /api/0.6/changeset/create
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PutChangesetCreate(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read: GET /api/0.6/changeset/#id?include_discussion=true
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Returns the changeset with the given id in OSM-XML format. </returns>
        private dynamic GetChangeset(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update: PUT /api/0.6/changeset/#id
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PutChangesetUpdate(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Close: PUT /api/0.6/changeset/#id/close
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PutChangesetClose(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Download: GET /api/0.6/changeset/#id/download
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetChangesetDownload(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expand Bounding Box: POST /api/0.6/changeset/#id/expand_bbox
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PostChangesetExpandBB(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query: GET /api/0.6/changesets
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetChangesetQuery(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Diff upload: POST /api/0.6/changeset/#id/upload
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PostChangesetUpload(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create: PUT /api/0.6/[node|way|relation]/create
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PutElementCreate(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read: GET /api/0.6/[node|way|relation]/#id
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetElement(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update: PUT /api/0.6/[node|way|relation]/#id
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic PutElementUpdate(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete: DELETE /api/0.6/[node|way|relation]/#id
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic DeleteElement(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// History: GET /api/0.6/[node|way|relation]/#id/history
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetElementHistory(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Version: GET /api/0.6/[node|way|relation]/#id/#version
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetElementVersion(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multi fetch: GET /api/0.6/[nodes|ways|relations]?#parameters
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetElementMultiple(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Relations for element: GET /api/0.6/[node|way|relation]/#id/relations
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetElementRelations(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ways for node: GET /api/0.6/node/#id/ways
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetWaysForNode(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Full: GET /api/0.6/[way|relation]/#id/full
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private  dynamic GetElementFull(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GET /api/0.6/user/#id
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetUserDetails(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GET /api/0.6/user/details
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetCurrentUserDetails(dynamic _)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  GET /api/0.6/user/preferences
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        private dynamic GetCurrentUserPreferences(dynamic _)
        {
            throw new NotImplementedException();
        }
    }
}