using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// AnalyzerEngine
    /// </summary>
    public class AnalyzerEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public void Generate(SortedDataObject dataObject, List<WithFieldsAttribute> fieldsAttributes)
        {
            if (fieldsAttributes == null || fieldsAttributes.Count == 0)
            {
                return;
            }

            foreach (var fieldsAttribute in fieldsAttributes)
            {
                var keywordFieldsAttribute = fieldsAttribute as WithKeywordFieldsAttribute;
                if (keywordFieldsAttribute != null)
                {
                    var mmm = dataObject.AddDataObject(DataKey.DoubleQuotationString((fieldsAttribute.Name ?? "keyword").ToLowerCaseUnderLine()));
                    mmm.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                    if (keywordFieldsAttribute.IgnoreAbove > 0)
                    {
                        mmm.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), keywordFieldsAttribute.IgnoreAbove);
                    }
                }

                var textFieldsAttribute = fieldsAttribute as WithTextFieldsAttribute;
                if (textFieldsAttribute != null)
                {
                    var mmm = dataObject.AddDataObject(DataKey.DoubleQuotationString((fieldsAttribute.Name ?? textFieldsAttribute.Analyzer).ToLowerCaseUnderLine()));
                    mmm.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("text"));
                    mmm.AddDataValue(DataKey.DoubleQuotationString("analyzer"), DataValue.DoubleQuotationString(textFieldsAttribute.Analyzer));
                }
            }
        }
    }
}
