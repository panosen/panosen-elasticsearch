using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Panosen.CodeDom;
using Panosen.CodeDom.JavaScript.Engine;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// MappingsEngine
    /// </summary>
    public partial class MappingsEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public string Generate(Mappings mappings)
        {
            if (mappings == null)
            {
                return null;
            }

            var indexAttribute = mappings.Type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return null;
            }

            var mappingsEngine = new MappingsEngine();

            var dataObject = mappingsEngine.BuildMappingsFile(mappings.Type);

            var builder = new StringBuilder();

            new JsCodeEngine().GenerateDataObject(dataObject, builder, new GenerateOptions
            {
                TabString = "  ",
                DataArrayItemBreakLine = true
            });

            return builder.ToString();
        }

        private DataObject BuildMappingsFile(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);

            var customAnalyzerAttributeList = type.GetCustomAttributes<CustomAnalyzerAttribute>(false).ToList();
            var customFilterAttributeList = type.GetCustomAttributes<CustomFilterAttribute>(false).ToList();
            var customTokenizerAttributeList = type.GetCustomAttributes<CustomTokenizerAttribute>(false).ToList();

            var dataObject = new DataObject();

            //alias
            if (indexAttribute.Aliases != null && indexAttribute.Aliases.Length > 0)
            {
                var aliasDataObject = dataObject.AddDataObject(DataKey.DoubleQuotationString("aliases"));
                foreach (var alias in indexAttribute.Aliases)
                {
                    aliasDataObject.AddDataObject(DataKey.DoubleQuotationString(alias));
                }
            }

            //settings
            var settingsDataObject = new SettingsEngine().BuildSettings(indexAttribute, customTokenizerAttributeList, customFilterAttributeList, customAnalyzerAttributeList);
            if (settingsDataObject != null && settingsDataObject.DataItemMap != null && settingsDataObject.DataItemMap.Count > 0)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("settings"), settingsDataObject);
            }

            //mappings
            var mappingsDataObject = BuildMappings(type);
            if (mappingsDataObject != null && mappingsDataObject.DataItemMap != null && mappingsDataObject.DataItemMap.Count > 0)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("mappings"), mappingsDataObject);
            }

            return dataObject;
        }

        private DataObject BuildMappings(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return null;
            }

            DataObject dataObject = new DataObject();
            var _doc = new DataObject();

            //_all
            if (indexAttribute.AllEnabled != Enabled.None)
            {
                _doc.AddDataObject(DataKey.DoubleQuotationString("_all")).AddDataValue(DataKey.DoubleQuotationString("enabled"), indexAttribute.AllEnabled.ToString().ToLower());
            }

            //"dynamic": true
            switch (indexAttribute.Dynamic)
            {
                case Dynamic.False:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("false"));
                    break;
                case Dynamic.Strict:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("strict"));
                    break;
                case Dynamic.True:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("true"));
                    break;
                default:
                    break;
            }

            var dynamicTemplateAttributes = type.GetCustomAttributes(typeof(DynamicTemplateAttribute), false) as DynamicTemplateAttribute[];
            if (dynamicTemplateAttributes != null && dynamicTemplateAttributes.Length > 0)
            {
                var dynamicTemplates = new DynamicTemplateEngine().BuildDynamicTemplates(dynamicTemplateAttributes);
                if (dynamicTemplates != null && dynamicTemplates.Items != null && dynamicTemplates.Items.Count > 0)
                {
                    _doc.AddDataArray(DataKey.DoubleQuotationString("dynamic_templates"), dynamicTemplates);
                }
            }

            var properties = new PropertiesEngine().BuildProperties(type);
            if (properties != null && properties.DataItemMap != null && properties.DataItemMap.Count > 0)
            {
                _doc.AddSortedDataObject(DataKey.DoubleQuotationString("properties"), properties);
            }

            if (_doc.DataItemMap != null && _doc.DataItemMap.Count > 0)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString(indexAttribute.TypeName ?? "_doc"), _doc);
            }

            return dataObject;
        }
    }
}
