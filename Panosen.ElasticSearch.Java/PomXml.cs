using System.Collections.Generic;

namespace Panosen.ElasticSearch.Java
{
    public class PomXml
    {
        /// <summary>
        /// parent.groupId
        /// </summary>
        public string ParentGroupId { get; set; }

        /// <summary>
        /// parent.artifactId
        /// </summary>
        public string ParentArtifactId { get; set; }

        /// <summary>
        /// parent.version
        /// </summary>
        public string ParentVersion { get; set; }

        /// <summary>
        /// groupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// artifactId
        /// </summary>
        public string ArtifactId { get; set; }

        /// <summary>
        /// version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// properties
        /// </summary>
        public Dictionary<string, string> PropertyMap { get; set; }
    }
}
