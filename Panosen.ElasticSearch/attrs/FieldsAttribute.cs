using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// index the same field in different ways for different purposes.
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/multi-fields.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class FieldsAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// keyword类型的fields
    /// </summary>
    public sealed class KeywordFieldsAttribute : FieldsAttribute
    {
        /// <summary>
        /// IgnoreAbove
        /// </summary>
        public int IgnoreAbove { get; set; }
    }

    /// <summary>
    /// text类型的fields
    /// </summary>
    public sealed class TextFieldsAttribute : FieldsAttribute
    {
        /// <summary>
        /// Analyzer
        /// </summary>
        public string Analyzer { get; private set; }

        /// <summary>
        /// TextFieldsAttribute
        /// </summary>
        public TextFieldsAttribute(BuiltInAnalyzer analyzer)
        {
            if (analyzer == BuiltInAnalyzer.None)
            {
                return;
            }
            this.Analyzer = analyzer.ToString().ToLower();
        }

        /// <summary>
        /// TextFieldsAttribute
        /// </summary>
        public TextFieldsAttribute(IKAnalyzer analyzer)
        {
            if (analyzer == IKAnalyzer.None)
            {
                return;
            }
            this.Analyzer = analyzer.ToString().ToLower();
        }

        /// <summary>
        /// TextFieldsAttribute
        /// </summary>
        public TextFieldsAttribute(string analyzer)
        {
            if (string.IsNullOrEmpty(analyzer))
            {
                return;
            }
            this.Analyzer = analyzer;
        }
    }
}
