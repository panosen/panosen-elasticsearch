using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Panosen.CodeDom;

namespace Panosen.ElasticSearch.Mapping.Engine
{

    partial class MappingsEngine
    {
        private DataObject BuildSettings(IndexAttribute indexAttribute,
            List<CustomTokenizerAttribute> customTokenizerAttributeList,
            List<CustomFilterAttribute> customFilterAttributeList,
            List<CustomAnalyzerAttribute> customAnalyzerAttributeList)
        {
            if (indexAttribute.NumberOfReplicas == 0
                && indexAttribute.NumberOfShards == 0
                && (customAnalyzerAttributeList == null || customAnalyzerAttributeList.Count == 0)
                && (customFilterAttributeList == null || customFilterAttributeList.Count == 0)
                && (customTokenizerAttributeList == null || customTokenizerAttributeList.Count == 0))
            {
                return null;
            }

            var returnSettingsDataObject = false;
            var settingsDataObject = new DataObject();
            if (indexAttribute.NumberOfShards > -1)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_shards"), indexAttribute.NumberOfShards);
            }
            if (indexAttribute.NumberOfReplicas > -1)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_replicas"), indexAttribute.NumberOfReplicas);
            }
            if (indexAttribute.MappingTotalFieldsLimit > -1)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("mapping.total_fields.limit"), indexAttribute.MappingTotalFieldsLimit);
            }

            var withAnalysisDataObject = false;
            var analysisDataObject = new DataObject();

            if (customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var tokenizerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("tokenizer"));
                foreach (var customTokenizerAttribute in customTokenizerAttributeList)
                {
                    var tokenizerBody = BuildTokenizerBody(customTokenizerAttribute);
                    tokenizerDataObject.AddDataObject(DataKey.DoubleQuotationString(customTokenizerAttribute.Name), tokenizerBody);
                }
            }

            if (customFilterAttributeList != null && customFilterAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var analyzerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("filter"));
                foreach (var customFilterAttribute in customFilterAttributeList)
                {
                    var filterProperties = BuildFilterProperties(customFilterAttribute);
                    analyzerDataObject.AddDataObject(DataKey.DoubleQuotationString(customFilterAttribute.Name), filterProperties);
                }
            }

            if (customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var analyzerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("analyzer"));
                foreach (var customAnalyzerAttribute in customAnalyzerAttributeList)
                {
                    var analyzerProperties = BuildAnalyzerProperties(customAnalyzerAttribute);
                    analyzerDataObject.AddDataObject(DataKey.DoubleQuotationString(customAnalyzerAttribute.Name), analyzerProperties);
                }
            }

            if (withAnalysisDataObject)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataObject(DataKey.DoubleQuotationString("analysis"), analysisDataObject);
            }

            if (returnSettingsDataObject)
            {
                return settingsDataObject;
            }
            else
            {
                return null;
            }
        }
    }
}
