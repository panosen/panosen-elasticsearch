using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 嵌套类型
    /// </summary>
    public class NestedFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Nested
        /// </summary>
        public override FieldType FieldType => FieldType.NestedObject;

        /// <summary>
        /// nested
        /// </summary>
        public override string Type => "nested";
    }
}
