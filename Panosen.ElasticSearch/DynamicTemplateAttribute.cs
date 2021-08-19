using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/7.9/dynamic-templates.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DynamicTemplateAttribute : Attribute
    {
        /// <summary>
        /// Name
        /// The template name can be any string value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// DynamicTemplateAttribute
        /// </summary>
        public DynamicTemplateAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Mapping
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// match_mapping_type
        /// data type detected by the JSON parser.
        /// </summary>
        public MatchMappingType MatchMappingType { get; set; }

        /// <summary>
        /// match
        /// </summary>
        public string NameMatch { get; set; }

        /// <summary>
        /// unmatch
        /// </summary>
        public string NameUnmatch { get; set; }

        /// <summary>
        /// Match_pattern
        /// </summary>
        public string NameMatchPattern { get; set; }

        /// <summary>
        /// path_match
        /// </summary>
        public string PathMatch { get; set; }

        /// <summary>
        /// path_unmatch
        /// </summary>
        public string PathUnmatch { get; set; }

        /// <summary>
        /// disable indexing on those fields to save disk space and also maybe gain some indexing speed
        /// </summary>
        public Index Index { get; set; }
    }

    /// <summary>
    /// MatchMappingType
    /// </summary>
    public enum MatchMappingType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// when true or false are encountered.
        /// </summary>
        Boolean,

        /// <summary>
        /// when date detection is enabled and a string matching any of the configured date formats is found.
        /// </summary>
        Date,

        /// <summary>
        /// for numbers with a decimal part.
        /// </summary>
        Double,

        /// <summary>
        /// for numbers without a decimal part.
        /// </summary>
        Long,

        /// <summary>
        /// for objects, also called hashes.
        /// </summary>
        Object,

        /// <summary>
        /// for character strings.
        /// </summary>
        String,

        /// <summary>
        /// used in order to match all data types.
        /// </summary>
        All
    }
}
