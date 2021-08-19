using Panosen.CodeDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// DynamicTemplateEngine
    /// </summary>
    public class DynamicTemplateEngine
    {
        /// <summary>
        /// BuildDynamicTemplate
        /// </summary>
        public DataArray BuildDynamicTemplates(DynamicTemplateAttribute[] dynamicTemplateAttributes)
        {
            if (dynamicTemplateAttributes == null || dynamicTemplateAttributes.Length == 0)
            {
                return null;
            }

            DataArray dataArray = new DataArray();
            foreach (var dynamicTemplateAttribute in dynamicTemplateAttributes)
            {
                dataArray.AddDataObject(BuildDynamicTemplate(dynamicTemplateAttribute));
            }

            return dataArray;
        }

        private DataObject BuildDynamicTemplate(DynamicTemplateAttribute dynamicTemplateAttribute)
        {
            DataObject dynamicTemplate = new DataObject();

            var dataObject = dynamicTemplate.AddDataObject(DataKey.DoubleQuotationString(dynamicTemplateAttribute.Name));

            switch (dynamicTemplateAttribute.MatchMappingType)
            {
                case MatchMappingType.None:
                    break;
                case MatchMappingType.All:
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("match_mapping_type"), DataValue.DoubleQuotationString("*"));
                    break;
                default:
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("match_mapping_type"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.MatchMappingType.ToString().ToLower()));
                    break;
            }

            if (!string.IsNullOrEmpty(dynamicTemplateAttribute.NameMatch))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("match"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.NameMatch));
            }

            if (!string.IsNullOrEmpty(dynamicTemplateAttribute.NameUnmatch))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("unmatch"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.NameUnmatch));
            }

            if (!string.IsNullOrEmpty(dynamicTemplateAttribute.NameMatchPattern))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("match_pattern"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.NameMatchPattern));
            }

            if (!string.IsNullOrEmpty(dynamicTemplateAttribute.PathMatch))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("path_match"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.PathMatch));
            }

            if (!string.IsNullOrEmpty(dynamicTemplateAttribute.PathUnmatch))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("path_unmatch"), DataValue.DoubleQuotationString(dynamicTemplateAttribute.PathUnmatch));
            }

            var mapping = new DataObject();

            new PropertiesEngine().ProcessPropertyType(mapping, dynamicTemplateAttribute.Type, dynamicTemplateAttribute.Index, DocValues.None, 0);

            if (mapping != null && mapping.DataItemMap != null && mapping.DataItemMap.Count > 0)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("mapping"), mapping);
            }

            return dynamicTemplate;
        }
    }
}
