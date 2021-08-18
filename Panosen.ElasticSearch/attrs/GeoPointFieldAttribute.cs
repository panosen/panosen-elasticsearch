using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch
{
    /// <summary>
    /// GeoPointFieldAttribute
    /// </summary>
    public class GeoPointFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// GeoPoint
        /// </summary>
        public override FieldType FieldType => FieldType.GeoPoint;
    }
}
