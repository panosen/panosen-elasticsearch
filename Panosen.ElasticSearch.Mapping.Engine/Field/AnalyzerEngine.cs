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
        public void Generate(SortedDataObject dataObject, BuiltInAnalyzer builtInAnalyzer, IKAnalyzer iKAnalyzer, string[] customAnalyzer)
        {
            //分词器
            List<string> analyzers = new List<string>();

            //内置分词器
            var builtInAnalyzers = builtInAnalyzer
                .ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(x => x)
                .ToList();
            analyzers.AddRange(builtInAnalyzers);

            //ik分词器
            var ikAnalyzers = iKAnalyzer
                .ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(x => x)
                .ToList();
            analyzers.AddRange(ikAnalyzers);

            //自定义分词器
            if (customAnalyzer != null && customAnalyzer.Length > 0)
            {
                analyzers.AddRange(customAnalyzer);
            }

            foreach (var analyzer in analyzers)
            {
                if ("none".Equals(analyzer, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var mmm = dataObject.AddDataObject(DataKey.DoubleQuotationString(analyzer));
                mmm.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("text"));
                mmm.AddDataValue(DataKey.DoubleQuotationString("analyzer"), DataValue.DoubleQuotationString(analyzer));
            }
        }
    }
}
