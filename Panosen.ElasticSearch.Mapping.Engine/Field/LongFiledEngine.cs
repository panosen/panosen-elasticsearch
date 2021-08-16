using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// LongFiledEngine
    /// </summary>
    public class LongFiledEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public void Generate(DataObject dataObject, LongFieldAttribute longFieldAttribute)
        {
            if (longFieldAttribute.NullValue.HasValue)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), longFieldAttribute.NullValue.Value);
            }
        }
    }
}
