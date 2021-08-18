using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// GeoPointFiledEngine
    /// </summary>
    public class GeoPointFiledEngine : FieldEngine<GeoPointFieldAttribute>
    {
        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected override void OnGenerateType(DataObject dataObject)
        {
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.GEO_POINT));
        }

        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, GeoPointFieldAttribute integerFieldAttribute)
        {
        }
    }
}
