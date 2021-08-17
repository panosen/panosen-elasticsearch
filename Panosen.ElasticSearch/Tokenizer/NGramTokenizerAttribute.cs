using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// NGram 元分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenizer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class NGramTokenizerAttribute : AbstractNGramTokenizerAttribute
    {
        /// <summary>
        /// NGramTokenizerAttribute
        /// </summary>
        public NGramTokenizerAttribute(string name) : base(name, "ngram")
        {
        }

        /// <summary>
        /// NGramTokenizerAttribute
        /// </summary>
        public NGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar) : base(name, minGram, maxGram, nGramTokenChar, "ngram")
        {
        }
    }

    /// <summary>
    /// EdgeNGram 前缀分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-edgengram-tokenizer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class EdgeNGramTokenizerAttribute : AbstractNGramTokenizerAttribute
    {
        /// <summary>
        /// EdgeNGramTokenizerAttribute
        /// </summary>
        public EdgeNGramTokenizerAttribute(string name) : base(name, "edge_ngram")
        {
        }

        /// <summary>
        /// EdgeNGramTokenizerAttribute
        /// </summary>
        public EdgeNGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar) : base(name, minGram, maxGram, nGramTokenChar, "edge_ngram")
        {
        }
    }

    /// <summary>
    /// AbstractNGramTokenizerAttribute
    /// </summary>
    public abstract class AbstractNGramTokenizerAttribute : CustomTokenizerAttribute
    {
        /// <summary>
        /// AbstractNGramTokenizerAttribute
        /// </summary>
        public AbstractNGramTokenizerAttribute(string name, string type) : base(name, type)
        {
        }

        /// <summary>
        /// AbstractNGramTokenizerAttribute
        /// </summary>
        public AbstractNGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar, string type) : base(name, type)
        {
            this.MinGram = minGram;
            this.MaxGram = maxGram;
            this.TokenChars = nGramTokenChar;
        }

        /// <summary>
        /// min_gram es默认值是1
        /// </summary>
        public int MinGram { get; set; }

        /// <summary>
        /// max_gram es默认值是 2
        /// </summary>
        public int MaxGram { get; set; }

        /// <summary>
        /// token_chars es默认值是 []
        /// </summary>
        public NGramTokenChar TokenChars { get; set; }
    }

    /// <summary>
    /// ngram tokenizer token_chars
    /// </summary>
    [Flags]
    public enum NGramTokenChar
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// letter
        /// </summary>
        Letter = 1,

        /// <summary>
        /// digit
        /// </summary>
        Digit = 2,

        /// <summary>
        /// whitespace
        /// </summary>
        Whitespace = 4,

        /// <summary>
        /// punctuation
        /// </summary>
        Punctuation = 8,

        /// <summary>
        /// symbol
        /// </summary>
        Symbol = 16
    }
}
