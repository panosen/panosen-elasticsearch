using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-pattern-tokenizer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class PatternTokenizerAttribute : CustomTokenizerAttribute
    {
        /// <summary>
        /// PatternTokenizerAttribute
        /// </summary>
        public PatternTokenizerAttribute(string name) : base(name, "pattern")
        {
        }

        /// <summary>
        /// PatternTokenizerAttribute
        /// </summary>
        public PatternTokenizerAttribute(string name, string pattern) : base(name, "pattern")
        {
            this.Pattern = pattern;
        }

        /// <summary>
        /// Param
        /// </summary>
        public string Pattern { get; set; }
    }
}
