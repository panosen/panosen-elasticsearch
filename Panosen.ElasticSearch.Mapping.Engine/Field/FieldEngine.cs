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
    /// FieldEngine
    /// </summary>
    public abstract class FieldEngine<TFieldAttribute>
        where TFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Generate
        /// </summary>
        public void Generate(DataObject dataObject, TFieldAttribute fieldAttribute)
        {
            if (fieldAttribute == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(fieldAttribute.Type))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(fieldAttribute.Type));
            }

            if (!fieldAttribute.Index)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("index"), false);
            }

            if (!fieldAttribute.DocValues)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("doc_values"), false);
            }

            OnGenerate(dataObject, fieldAttribute);
        }

        /// <summary>
        /// OnGenerate
        /// </summary>
        protected abstract void OnGenerate(DataObject dataObject, TFieldAttribute value);
    }

    /// <summary>
    /// FieldEngine
    /// </summary>
    public class FieldEngine : FieldEngine<FieldAttribute>
    {
        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, FieldAttribute value)
        {
        }
    }
}
