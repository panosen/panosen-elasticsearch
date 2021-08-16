using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 字符串(不分词)
    /// </summary>
    public sealed class KeywordFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Keyword
        /// </summary>
        public override FieldType FieldType => FieldType.Keyword;

        /// <summary>
        /// keyword
        /// </summary>
        public override string Type => "keyword";

        /// <summary>
        /// `ignore_above`
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/ignore-above.html
        /// </summary>
        public int IgnoreAbove { get; set; } = 256;

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public string NullValue { get; set; }

        /// <summary>
        /// 内置分词器
        /// </summary>
        public BuiltInAnalyzer BuiltInAnalyzer { get; set; } = BuiltInAnalyzer.NONE;

        /// <summary>
        /// IK分词器
        /// </summary>
        public IKAnalyzer IKAnalyzer { get; set; } = IKAnalyzer.NONE;

        /// <summary>
        /// 自定义分词器
        /// </summary>
        public string[] CustomAnalyzer { get; set; }
    }
}
