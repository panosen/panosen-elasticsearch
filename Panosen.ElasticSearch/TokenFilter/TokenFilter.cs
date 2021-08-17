using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// TokenFilter
    /// </summary>
    public abstract class TokenFilter
    {
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-asciifolding-tokenfilter.html
    /// </summary>
    public sealed class AsciifoldingTokenFilter : TokenFilter
    {

    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-length-tokenfilter.html
    /// </summary>
    public sealed class LengthTokenFilter : TokenFilter
    {
        /// <summary>
        /// Min
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Max
        /// </summary>
        public int Max { get; set; }
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-lowercase-tokenfilter.html
    /// </summary>
    public sealed class LowercaseTokenFilter : TokenFilter
    {
        /// <summary>
        /// Language
        /// </summary>
        public LowercaseLanguage Language { get; set; }
    }

    /// <summary>
    /// LowercaseLanguage
    /// </summary>
    public enum LowercaseLanguage
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// greek
        /// </summary>
        Greek,

        /// <summary>
        /// irish
        /// </summary>
        Irish,

        /// <summary>
        /// tukish
        /// </summary>
        Turkish
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenfilter.html
    /// </summary>
    public sealed class NGramTokenFilter : TokenFilter
    {
        /// <summary>
        /// MinGram
        /// </summary>
        public int? MinGram { get; set; }

        /// <summary>
        /// MaxGram
        /// </summary>
        public int? MaxGram { get; set; }
    }
}
