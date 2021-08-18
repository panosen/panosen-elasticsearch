using System;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// 分片数 number_of_shards，默认值是5
        /// </summary>
        public int NumberOfShards { get; set; } = -1;

        /// <summary>
        /// 副本数 number_of_replicas，默认值是1
        /// </summary>
        public int NumberOfReplicas { get; set; } = -1;

        /// <summary>
        /// 最大字段数
        /// </summary>
        public int MappingTotalFieldsLimit { get; set; } = -1;

        /// <summary>
        /// index.max_ngram_diff
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenfilter.html
        /// </summary>
        [Obsolete("暂不支持设置")]
        public int IndexMaxNGramDiff { get; set; }

        /// <summary>
        /// 索引别名
        /// </summary>
        public string[] Aliases { get; set; }

        /// <summary>
        /// 类型名(7.x已经废除)。建议不要设置，默认会使用"_doc"
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 是否支持动态字段
        /// </summary>
        public Dynamic Dynamic { get; set; }

        /// <summary>
        /// "_all": { "enabled": true }
        /// es内部默认值是 false
        /// </summary>
        public Enabled AllEnabled { get; set; }

        /// <summary>
        /// search.slowlog.threshold.query.warn
        /// </summary>
        public string SearchSlowlogThresholdQueryWarn { get; set; }

        /// <summary>
        /// refresh_interval
        /// </summary>
        public string RefreshInterval { get; set; }

        /// <summary>
        /// translog.sync_interval
        /// </summary>
        public string TranslogSyncInterval { get; set; }

        /// <summary>
        /// translog.durability
        /// </summary>
        public string TranslogDurability { get; set; }

        /// <summary>
        /// analysis.analyzer.default.type
        /// </summary>
        public string AnalysisAnalyzerDefaultType { get; set; }

        /// <summary>
        /// analysis.search_analyzer.default.type
        /// </summary>
        public string AnalysisSearchAnalyzerDefaultType { get; set; }
    }

    /// <summary>
    /// 动态索引相关配置
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/dynamic.html
    /// </summary>
    public enum Dynamic
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// 默认值，表示允许选自动新增字段
        /// Newly detected fields are added to the mapping. (default)
        /// </summary>
        True = 1,

        /// <summary>
        /// 不允许自动新增字段，但是文档可以正常写入，但无法对字段进行查询等操作
        /// Newly detected fields are ignored.
        /// These fields will not be indexed so will not be searchable but will still appear in the _source field of returned hits.
        /// These fields will not be added to the mapping, new fields must be added explicitly.
        /// </summary>
        False = 2,

        /// <summary>
        /// 严格模式，文档不能写入，报错
        /// If new fields are detected, an exception is thrown and the document is rejected.
        /// New fields must be explicitly added to the mapping.
        /// </summary>
        Strict = 3,

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/7.x/dynamic.html
        /// New fields are added to the mapping as runtime fields.
        /// These fields are not indexed, and are loaded from _source at query time.
        /// </summary>
        Runtime = 4
    }

    /// <summary>
    /// "_all": { "enabled": true }
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/enabled.html
    /// </summary>
    public enum Enabled
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// 是
        /// </summary>
        True = 1,

        /// <summary>
        /// 否(默认值)
        /// </summary>
        False = 2
    }
}
