using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Panosen.CodeDom;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// TokenizerEngine
    /// </summary>
    public class TokenizerEngine
    {
        /// <summary>
        /// BuildTokenizerBody
        /// </summary>
        public DataObject BuildTokenizerBody(CustomTokenizerAttribute customTokenizerAttribute)
        {
            if (customTokenizerAttribute is AbstractNGramTokenizerAttribute)
            {
                return BuildNGramTokenizer(customTokenizerAttribute as AbstractNGramTokenizerAttribute);
            }

            if (customTokenizerAttribute is PatternTokenizerAttribute)
            {
                return BuildPatternTokenier(customTokenizerAttribute as PatternTokenizerAttribute);
            }

            if (customTokenizerAttribute is CharGroupTokenizerAttribute)
            {
                return BuildCharGroupTokenizer(customTokenizerAttribute as CharGroupTokenizerAttribute);
            }

            return null;
        }

        private DataObject BuildNGramTokenizer(AbstractNGramTokenizerAttribute abstractNGramTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(abstractNGramTokenizerAttribute.Type));

            if (abstractNGramTokenizerAttribute.MinGram > 0)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("min_gram"), abstractNGramTokenizerAttribute.MinGram);
            }

            if (abstractNGramTokenizerAttribute.MaxGram > 0)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("max_gram"), abstractNGramTokenizerAttribute.MaxGram);
            }

            var tokenChars = abstractNGramTokenizerAttribute.TokenChars.ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();

            if (tokenChars.Count > 0)
            {
                var tokenCharsArray = dataObject.AddDataArray(DataKey.DoubleQuotationString("token_chars"));
                foreach (var item in tokenChars)
                {
                    tokenCharsArray.AddDataValue(DataValue.DoubleQuotationString(item));
                }
            }

            return dataObject;
        }

        private DataObject BuildPatternTokenier(PatternTokenizerAttribute patternTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(patternTokenizerAttribute.Type));

            if (!string.IsNullOrEmpty(patternTokenizerAttribute.Pattern))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("pattern"), DataValue.DoubleQuotationString(patternTokenizerAttribute.Pattern));
            }

            return dataObject;
        }

        private DataObject BuildCharGroupTokenizer(CharGroupTokenizerAttribute charGroupTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(charGroupTokenizerAttribute.Type));

            List<string> tokenizeOChars = new List<string>();

            if (charGroupTokenizerAttribute.Chars != null && charGroupTokenizerAttribute.Chars.Length > 0)
            {
                tokenizeOChars.AddRange(charGroupTokenizerAttribute.Chars.Select(v => v.ToString()));
            }

            var charGroupTokenizeOnChars = charGroupTokenizerAttribute.CharGroupTokenizeOnChars.ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();
            if (charGroupTokenizeOnChars.Count > 0)
            {
                tokenizeOChars.AddRange(charGroupTokenizeOnChars);
            }

            var array = dataObject.AddDataArray(DataKey.DoubleQuotationString("tokenize_on_chars"));
            foreach (var item in tokenizeOChars)
            {
                array.AddDataValue(DataValue.DoubleQuotationString(item));
            }

            return dataObject;
        }
    }
}
