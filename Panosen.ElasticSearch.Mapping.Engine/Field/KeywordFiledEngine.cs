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
    public class KeywordFiledEngine : FieldEngine<KeywordFieldAttribute>
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
        protected override void OnGenerate(DataObject dataObject, KeywordFieldAttribute keywordFieldAttribute)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), keywordFieldAttribute.IgnoreAbove);

            if (keywordFieldAttribute.NullValue != null)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), keywordFieldAttribute.NullValue);
            }

            if (keywordFieldAttribute.BuiltInAnalyzer != BuiltInAnalyzer.None ||
                keywordFieldAttribute.IKAnalyzer != IKAnalyzer.None ||
                (keywordFieldAttribute.CustomAnalyzer != null && keywordFieldAttribute.CustomAnalyzer.Length > 0))
            {
                SortedDataObject sortedDataObject = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("fields"));

                new AnalyzerEngine().Generate(sortedDataObject, keywordFieldAttribute.BuiltInAnalyzer, keywordFieldAttribute.IKAnalyzer, keywordFieldAttribute.CustomAnalyzer);
            }
        }
    }
}
