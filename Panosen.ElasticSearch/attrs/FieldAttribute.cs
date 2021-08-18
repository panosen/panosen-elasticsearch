using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// 对应一个文档里面的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否索引该字段
        /// 默认值是true
        /// </summary>
        public Index Index { get; set; }

        /// <summary>
        /// 是否需要doc_values。用于排序、聚合以及脚本操作
        /// 默认值是true
        /// </summary>
        public DocValues DocValues { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public virtual FieldType FieldType { get; }
    }

    /// <summary>
    /// index
    /// </summary>
    public enum Index
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// true
        /// </summary>
        True = 1,

        /// <summary>
        /// false
        /// </summary>
        False = 2
    }

    /// <summary>
    /// index
    /// </summary>
    public enum DocValues
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// true
        /// </summary>
        True = 1,

        /// <summary>
        /// false
        /// </summary>
        False = 2
    }
}
