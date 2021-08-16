using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// IntegerFiledEngine
    /// </summary>
    public class IntegerFiledEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public void Generate(DataObject dataObject, IntegerFieldAttribute integerFieldAttribute)
        {
            if (integerFieldAttribute.NullValue.HasValue)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), integerFieldAttribute.NullValue.Value);
            }
        }
    }
}
