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
    /// TextFiledEngine
    /// </summary>
    public class TextFiledEngine : FieldEngine<TextFieldAttribute>
    {
        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected override void OnGenerateType(DataObject dataObject)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.TEXT));
        }

        /// <summary>
        /// Generate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, TextFieldAttribute textFieldAttribute, List<FieldsAttribute> fieldsAttributes)
        {
            if (!string.IsNullOrEmpty(textFieldAttribute.DefaultAnalyzer))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("analyzer"), DataValue.DoubleQuotationString(textFieldAttribute.DefaultAnalyzer));
            }

            if (textFieldAttribute.NullValue != null)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), DataValue.DoubleQuotationString(textFieldAttribute.NullValue));
            }

            //analyzer
            var sortedDataObject = new SortedDataObject();
            new AnalyzerEngine().Generate(sortedDataObject, fieldsAttributes);
            if (sortedDataObject.DataItemMap != null && sortedDataObject.DataItemMap.Count > 0)
            {
                dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("fields"), sortedDataObject);
            }
        }
    }
}
