using System;
using System.Collections.Generic;
using System.Text;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// MappingsExtension
    /// </summary>
    public static class MappingsExtension
    {
        /// <summary>
        /// TransformText
        /// </summary>
        public static string TransformText(this Mappings mappings)
        {
            return new MappingsEngine().Generate(mappings);
        }
    }
}
