using System;
using System.Collections.Generic;
using System.Text;

namespace Panosen.ElasticSearch.Java.Engine.Engine
{
    /// <summary>
    /// DocFieldsExtension
    /// </summary>
    public static class DocFieldsExtension
    {
        /// <summary>
        /// TransformText
        /// </summary>
        /// <param name="docFields"></param>
        /// <returns></returns>
        public static string TransformText(this DocFields docFields)
        {
            return new DocFieldsEngine().Generate(docFields);
        }
    }
}
