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
    /// ObjectFiledEngine
    /// </summary>
    public class ObjectFiledEngine : FieldEngine<ObjectFieldAttribute>
    {
        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected override void OnGenerateType(DataObject dataObject)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.OBJECT));
        }

        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, ObjectFieldAttribute objectFieldAttribute, List<FieldsAttribute> fieldsAttributes)
        {
            switch (objectFieldAttribute.Dynamic)
            {
                case Dynamic.False:
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("dynamic"), false);
                    break;
                case Dynamic.Strict:
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("strict"));
                    break;
                case Dynamic.True:
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("dynamic"), true);
                    break;
                default:
                    break;
            }

            if (objectFieldAttribute.Enabled != Enabled.None)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("enabled"), objectFieldAttribute.Enabled.ToString().ToLower());
            }
        }
    }
}
