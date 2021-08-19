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
    public abstract class WithFieldsAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// keyword类型的fields
    /// </summary>
    public sealed class WithKeywordFieldsAttribute : WithFieldsAttribute
    {
        /// <summary>
        /// IgnoreAbove
        /// </summary>
        public int IgnoreAbove { get; set; }
    }

    /// <summary>
    /// text类型的fields
    /// </summary>
    public sealed class WithTextFieldsAttribute : WithFieldsAttribute
    {
        /// <summary>
        /// Analyzer
        /// </summary>
        public string Analyzer { get; private set; }

        /// <summary>
        /// TextFieldsAttribute
        /// </summary>
        public WithTextFieldsAttribute(BuiltInAnalyzer analyzer)
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
        public WithTextFieldsAttribute(IKAnalyzer analyzer)
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
        public WithTextFieldsAttribute(string analyzer)
        {
            if (string.IsNullOrEmpty(analyzer))
            {
                return;
            }
            this.Analyzer = analyzer;
        }
    }
}
