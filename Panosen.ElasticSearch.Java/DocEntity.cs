using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Panosen.Reflection.Model;

namespace Panosen.ElasticSearch.Java
{
    /// <summary>
    /// es doc
    /// </summary>
    public class DocEntity
    {
        /// <summary>
        /// java root package
        /// </summary>
        public string RootNamespace { get; set; }

        /// <summary>
        /// JavaRoot
        /// </summary>
        public string JavaRoot { get; set; }

        /// <summary>
        /// Class
        /// </summary>
        public ClassNode ClassNode { get; set; }
    }
}
