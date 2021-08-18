using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 字符串(分词)
    /// </summary>
    public sealed class TextFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Text
        /// </summary>
        public override FieldType FieldType => FieldType.Text;

        /// <summary>
        /// 内置分词器
        /// </summary>
        public BuiltInAnalyzer BuiltInAnalyzer { get; set; }

        /// <summary>
        /// IK分词器
        /// </summary>
        public IKAnalyzer IKAnalyzer { get; set; }

        /// <summary>
        /// 自定义分词器
        /// </summary>
        public string[] CustomAnalyzer { get; set; }

        /// <summary>
        /// 默认分析器
        /// </summary>
        public string DefaultAnalyzer { get; private set; }

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public string NullValue { get; set; }

        /// <summary>
        /// 使用`标准分析器`作为基础分析器
        /// </summary>
        public TextFieldAttribute(BuiltInAnalyzer builtInAnalyzer)
        {
            if (builtInAnalyzer == BuiltInAnalyzer.None)
            {
                return;
            }

            this.DefaultAnalyzer = builtInAnalyzer.ToString().ToLower();
        }

        /// <summary>
        /// 使用`IK分析器`作为基础分析器
        /// </summary>
        /// <param name="defaultAnalyzer">基础分析器</param>
        public TextFieldAttribute(IKAnalyzer defaultAnalyzer)
        {
            if (defaultAnalyzer == IKAnalyzer.None)
            {
                return;
            }

            this.DefaultAnalyzer = defaultAnalyzer.ToString().ToLower();
        }

        /// <summary>
        /// 使用`自定义分析器`作为基础分析器
        /// </summary>
        /// <param name="defaultAnalyzer">基础分析器</param>
        public TextFieldAttribute(string defaultAnalyzer)
        {
            this.DefaultAnalyzer = defaultAnalyzer;
        }

        /// <summary>
        /// 不指定基础分析器
        /// </summary>
        public TextFieldAttribute()
        {
        }
    }
}
