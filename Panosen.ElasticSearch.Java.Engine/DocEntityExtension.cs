using System;
using System.Collections.Generic;
using System.Text;

namespace Panosen.ElasticSearch.Java.Engine.Engine
{
    /// <summary>
    /// DocEntityExtension
    /// </summary>
    public static class DocEntityExtension
    {
        /// <summary>
        /// TransformText
        /// </summary>
        public static string TransformText(this DocEntity docEntity)
        {
            return new DocEntityEngine().Generate(docEntity);
        }
    }
}
