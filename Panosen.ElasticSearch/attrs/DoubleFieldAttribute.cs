using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// double型
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/number.html
    /// </summary>
    public sealed class DoubleFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Double
        /// </summary>
        public override FieldType FieldType => FieldType.Double;

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public double? NullValue { get; private set; }

        /// <summary>
        /// DoubleFieldAttribute
        /// </summary>
        public DoubleFieldAttribute()
        {
        }

        /// <summary>
        /// DoubleFieldAttribute
        /// </summary>
        /// <param name="nullValue"></param>
        public DoubleFieldAttribute(double nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
