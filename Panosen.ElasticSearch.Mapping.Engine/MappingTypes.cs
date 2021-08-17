using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// ElasticSearchTypes
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.7/mapping-types.html
    /// </summary>
    public static class MappingTypes
    {

        /// <summary>
        /// text
        /// </summary>
        public static readonly string TEXT = "text";

        /// <summary>
        /// keyword
        /// </summary>
        public static readonly string KEYWORD = "keyword";

        /// <summary>
        /// long
        /// </summary>
        public static readonly string LONG = "long";

        /// <summary>
        /// integer
        /// </summary>
        public static readonly string INTEGER = "integer";

        /// <summary>
        /// short
        /// </summary>
        public static readonly string SHORT = "short";

        /// <summary>
        /// byte
        /// </summary>
        public static readonly string BYTE = "byte";

        /// <summary>
        /// double
        /// </summary>
        public static readonly string DOUBLE = "double";

        /// <summary>
        /// float
        /// </summary>
        public static readonly string FLOAT = "float";

        /// <summary>
        /// half_float
        /// </summary>
        public static readonly string HALF_FLOAT = "half_float";

        /// <summary>
        /// scaled_float
        /// </summary>
        public static readonly string SCALED_FLOAT = "scaled_float";

        /// <summary>
        /// date
        /// </summary>
        public static readonly string DATE = "date";

        /// <summary>
        /// boolean
        /// </summary>
        public static readonly string BOOLEAN = "boolean";

        /// <summary>
        /// binary
        /// </summary>
        public static readonly string BINARY = "binary";

        /// <summary>
        /// integer_range
        /// </summary>
        public static readonly string INTEGER_RANGE = "integer_range";

        /// <summary>
        /// float_range
        /// </summary>
        public static readonly string FLOAT_RANGE = "float_range";

        /// <summary>
        /// long_range
        /// </summary>
        public static readonly string LONG_RANGE = "long_range";

        /// <summary>
        /// double_range
        /// </summary>
        public static readonly string DOUBLE_RANGE = "double_range";

        /// <summary>
        /// date_range
        /// </summary>
        public static readonly string DATE_RANGE = "date_range";

        /// <summary>
        /// object 
        /// </summary>
        public static readonly string OBJECT  = "object";

        /// <summary>
        /// nested 
        /// </summary>
        public static readonly string NESTED = "nested";

        /// <summary>
        /// geo_point
        /// </summary>
        public static readonly string GEO_POINT = "geo_point";

        /// <summary>
        /// geo_shape 
        /// </summary>
        public static readonly string GEO_SHAPE = "geo_shape";

        /// <summary>
        /// ip
        /// </summary>
        public static readonly string IP = "ip";

        /// <summary>
        /// completion 
        /// </summary>
        public static readonly string COMPLETION = "completion";

        /// <summary>
        /// token_count
        /// </summary>
        public static readonly string TOKEN_COUNT = "token_count";

        /// <summary>
        /// murmur3
        /// </summary>
        public static readonly string MURMUR3 = "murmur3";

        /// <summary>
        /// annotated-text
        /// </summary>
        public static readonly string ANNOTATED_TEXT = "annotated-text";
    }
}
