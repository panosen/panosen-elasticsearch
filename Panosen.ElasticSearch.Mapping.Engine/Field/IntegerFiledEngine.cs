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
    /// IntegerFiledEngine
    /// </summary>
    public class IntegerFiledEngine : FieldEngine<IntegerFieldAttribute>
    {
        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, IntegerFieldAttribute integerFieldAttribute, PropertyInfo propertyInfo)
        {
            if (integerFieldAttribute.NullValue.HasValue)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), integerFieldAttribute.NullValue.Value);
            }
        }
    }
}
