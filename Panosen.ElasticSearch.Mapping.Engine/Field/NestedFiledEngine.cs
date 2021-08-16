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
    public class NestedFiledEngine : FieldEngine<NestedFieldAttribute>
    {
        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, NestedFieldAttribute nestedFieldAttribute, PropertyInfo propertyInfo)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));

            var _properties = dataObject.AddDataObject(DataKey.DoubleQuotationString("properties"));

            new PropertiesEngine().BuildProperties(_properties, propertyInfo.PropertyType.GenericTypeArguments[0]);
        }
    }
}
