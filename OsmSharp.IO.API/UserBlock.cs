using System;
using System.Xml.Serialization;

namespace OsmSharp.IO.API
{
    /// <summary>
    /// A user block as returned by
    /// <see href="https://wiki.openstreetmap.org/wiki/API_v0.6#User_Blocks">
    /// /api/0.6/user_blocks</see>.
    /// </summary>
    [XmlType("user_block")]
    public class UserBlock
    {
        [XmlAttribute("id")]
        public long Id { get; set; }

        [XmlAttribute("created_at")]
        public DateTime CreatedAt { get; set; }

        [XmlAttribute("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [XmlAttribute("ends_at")]
        public DateTime EndsAt { get; set; }

        /// <summary>
        /// Whether the blocked user is required to view the block page for the block to be lifted.
        /// </summary>
        [XmlAttribute("needs_view")]
        public bool NeedsView { get; set; }

        /// <summary>
        /// The blocked user.
        /// </summary>
        [XmlElement("user")]
        public UserReference User { get; set; }

        /// <summary>
        /// The moderator who created the block.
        /// </summary>
        [XmlElement("creator")]
        public UserReference Creator { get; set; }

        /// <summary>
        /// The moderator who revoked the block, null if it wasn't revoked.
        /// </summary>
        [XmlElement("revoker")]
        public UserReference Revoker { get; set; }

        /// <summary>
        /// The reason for the block, null when listing the active blocks.
        /// </summary>
        [XmlElement("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// A reference to a user by id and display name.
    /// </summary>
    public class UserReference
    {
        [XmlAttribute("uid")]
        public long Id { get; set; }

        [XmlAttribute("user")]
        public string Name { get; set; }
    }
}
