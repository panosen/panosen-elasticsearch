using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// TextFiledEngine
    /// </summary>
    public class TextFiledEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public void Generate(DataObject dataObject, TextFieldAttribute textFieldAttribute)
        {
            if (!string.IsNullOrEmpty(textFieldAttribute.DefaultAnalyzer))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("analyzer"), DataValue.DoubleQuotationString(textFieldAttribute.DefaultAnalyzer));
            }

            if (textFieldAttribute.NullValue != null)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), DataValue.DoubleQuotationString(textFieldAttribute.NullValue));
            }

            var sortedDataObject = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("fields"));

            //keyword
            {
                var keyword = sortedDataObject.AddDataObject(DataKey.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), 256);
            }

            //analyzer
            new AnalyzerEngine().Generate(sortedDataObject, textFieldAttribute.BuiltInAnalyzer, textFieldAttribute.IKAnalyzer, textFieldAttribute.CustomAnalyzer);
        }
    }
}
