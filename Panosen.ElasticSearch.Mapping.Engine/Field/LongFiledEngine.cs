using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// LongFiledEngine
    /// </summary>
    public class LongFiledEngine : FieldEngine<LongFieldAttribute>
    {
        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, LongFieldAttribute longFieldAttribute)
        {
            if (longFieldAttribute.NullValue.HasValue)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), longFieldAttribute.NullValue.Value);
            }
        }
    }
}
