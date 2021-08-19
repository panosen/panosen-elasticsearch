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
        public void Generate(DataObject dataObject, TFieldAttribute fieldAttribute, List<WithFieldsAttribute> fieldsAttributes)
        {
            if (fieldAttribute == null)
            {
                return;
            }

            OnGenerateType(dataObject);

            if (fieldAttribute.Index != Index.None)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("index"), fieldAttribute.Index.ToString().ToLower());
            }

            if (fieldAttribute.DocValues != DocValues.None)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("doc_values"), fieldAttribute.DocValues.ToString().ToLower());
            }

            OnGenerate(dataObject, fieldAttribute, fieldsAttributes);
        }

        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected abstract void OnGenerateType(DataObject dataObject);

        /// <summary>
        /// OnGenerate
        /// </summary>
        protected abstract void OnGenerate(DataObject dataObject, TFieldAttribute value, List<WithFieldsAttribute> fieldsAttributes);
    }

    /// <summary>
    /// FieldEngine
    /// </summary>
    public class FieldEngine : FieldEngine<FieldAttribute>
    {
        /// <summary>
        /// OnGenerate
        /// </summary>
        protected override void OnGenerate(DataObject dataObject, FieldAttribute value, List<WithFieldsAttribute> fieldsAttributes)
        {
        }

        /// <summary>
        /// OnGenerateType
        /// </summary>
        protected override void OnGenerateType(DataObject dataObject)
        {
        }
    }
}
