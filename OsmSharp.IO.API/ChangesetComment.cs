using System;
using System.Xml.Serialization;

namespace OsmSharp.IO.API
{
    /// <summary>
    /// A changeset comment as returned by
    /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#Search_changeset_comments:_GET_/api/0.6/changeset_comments">
    /// GET /api/0.6/changeset_comments</see>.
    /// </summary>
    [XmlType("comment")]
    public class ChangesetComment
    {
        /// <summary>
        /// The comment id, this is different from the changeset id.
        /// </summary>
        [XmlAttribute("id")]
        public long Id { get; set; }

        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlAttribute("visible")]
        public bool Visible { get; set; }

        [XmlAttribute("uid")]
        public long UserId { get; set; }

        [XmlAttribute("user")]
        public string UserName { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }
    }
}
