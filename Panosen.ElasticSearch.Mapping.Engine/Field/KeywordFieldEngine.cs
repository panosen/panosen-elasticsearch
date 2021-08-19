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
    /// KeywordFiledEngine
    /// </summary>
    public class KeywordFieldEngine : FieldEngine<KeywordFieldAttribute>
    {
        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected override void OnGenerateType(DataObject dataObject)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.KEYWORD));
        }

        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, KeywordFieldAttribute keywordFieldAttribute, List<FieldsAttribute> fieldsAttributes)
        {
            if (keywordFieldAttribute.IgnoreAbove > 0)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), keywordFieldAttribute.IgnoreAbove);
            }

            if (keywordFieldAttribute.NullValue != null)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), keywordFieldAttribute.NullValue);
            }

            SortedDataObject sortedDataObject = new SortedDataObject();
            new AnalyzerEngine().Generate(sortedDataObject, fieldsAttributes);
            if (sortedDataObject.DataItemMap != null && sortedDataObject.DataItemMap.Count > 0)
            {
                dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("fields"), sortedDataObject);
            }
        }
    }
}
