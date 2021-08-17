using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// ObjectFieldAttribute
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/object.html
    /// </summary>
    public class ObjectFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// FieldType
        /// </summary>
        public override FieldType FieldType => FieldType.Object;

        /// <summary>
        /// Whether or not new properties should be added dynamically to an existing object. Accepts true (default), runtime, false and strict.
        /// </summary>
        public Dynamic Dynamic { get; set; }

        /// <summary>
        /// Whether the JSON value given for the object field should be parsed and indexed (true, default) or completely ignored (false).
        /// </summary>
        public Enabled Enabled { get; set; }
    }
}
