using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 内置分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-analyzers.html
    /// </summary>
    public enum BuiltInAnalyzer
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// 【内置分词器】standard
        /// </summary>
        Standard = 1,

        /// <summary>
        /// 【内置分词器】simple
        /// </summary>
        Simple = 2,

        /// <summary>
        /// 【内置分词器】whitespace
        /// </summary>
        Whitespace = 3,

        /// <summary>
        /// 【内置分词器】stop
        /// </summary>
        Stop = 4,

        /// <summary>
        /// 【内置分词器】keyword
        /// </summary>
        Keyword = 5,

        /// <summary>
        /// 【内置分词器】pattern
        /// </summary>
        Pattern = 6,

        /// <summary>
        /// 【内置分词器】english
        /// </summary>
        English = 7,

        /// <summary>
        /// 【内置分词器】french
        /// </summary>
        French = 8,

        /// <summary>
        /// 【内置分词器】chinese
        /// </summary>
        Chinese = 9,

        /// <summary>
        /// 【被指分词器】fingerprint
        /// </summary>
        Fingerprint = 10
    }
}
