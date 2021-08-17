using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// IK分词器
    /// https://github.com/medcl/elasticsearch-analysis-ik
    /// </summary>
    [Flags]
    public enum IKAnalyzer
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// ik_max_word
        /// </summary>
        IK_MAX_WORD = 1,

        /// <summary>
        /// ik_smart
        /// </summary>
        IK_SMART = 2
    }
}
