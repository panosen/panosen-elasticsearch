using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Panosen.CodeDom;

namespace Panosen.ElasticSearch.Mapping.Engine
{
    /// <summary>
    /// SettingsEngine
    /// </summary>
    public class SettingsEngine
    {
        /// <summary>
        /// BuildSettings
        /// </summary>
        public DataObject BuildSettings(IndexAttribute indexAttribute,
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

            var settingsDataObject = new DataObject();
            if (indexAttribute.NumberOfShards > -1)
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_shards"), indexAttribute.NumberOfShards);
            }
            if (indexAttribute.NumberOfReplicas > -1)
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_replicas"), indexAttribute.NumberOfReplicas);
            }
            if (indexAttribute.MappingTotalFieldsLimit > -1)
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("mapping.total_fields.limit"), indexAttribute.MappingTotalFieldsLimit);
            }
            if (!string.IsNullOrEmpty(indexAttribute.SearchSlowlogThresholdQueryWarn))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("search.slowlog.threshold.query.warn"), DataValue.DoubleQuotationString(indexAttribute.SearchSlowlogThresholdQueryWarn));
            }
            if (!string.IsNullOrEmpty(indexAttribute.RefreshInterval))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("refresh_interval"), DataValue.DoubleQuotationString(indexAttribute.RefreshInterval));
            }
            if (!string.IsNullOrEmpty(indexAttribute.TranslogSyncInterval))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("translog.sync_interval"), DataValue.DoubleQuotationString(indexAttribute.TranslogSyncInterval));
            }
            if (!string.IsNullOrEmpty(indexAttribute.TranslogDurability))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("translog.durability"), DataValue.DoubleQuotationString(indexAttribute.TranslogDurability));
            }
            if (!string.IsNullOrEmpty(indexAttribute.AnalysisAnalyzerDefaultType))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("analysis.analyzer.default.type"), DataValue.DoubleQuotationString(indexAttribute.AnalysisAnalyzerDefaultType));
            }
            if (!string.IsNullOrEmpty(indexAttribute.AnalysisSearchAnalyzerDefaultType))
            {
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("analysis.search_analyzer.default.type"), DataValue.DoubleQuotationString(indexAttribute.AnalysisSearchAnalyzerDefaultType));
            }

            var withAnalysisDataObject = false;
            var analysisDataObject = new DataObject();

            if (customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var tokenizerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("tokenizer"));
                foreach (var customTokenizerAttribute in customTokenizerAttributeList)
                {
                    var tokenizerBody =new TokenizerEngine().BuildTokenizerBody(customTokenizerAttribute);
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
                settingsDataObject.AddDataObject(DataKey.DoubleQuotationString("analysis"), analysisDataObject);
            }

            return settingsDataObject;
        }

        private DataObject BuildFilterProperties(CustomFilterAttribute customFilterAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(customFilterAttribute.Type));

            if (customFilterAttribute.Properties != null && customFilterAttribute.Properties.Length > 0 && customFilterAttribute.Properties.Length % 2 == 0)
            {
                for (int i = 0; i < customFilterAttribute.Properties.Length; i += 2)
                {
                    dataObject.AddDataValue(DataKey.DoubleQuotationString(customFilterAttribute.Properties[i]), customFilterAttribute.Properties[i + 1]);
                }
            }

            return dataObject;
        }

        private DataObject BuildAnalyzerProperties(CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("custom"));
            dataObject.AddDataValue(DataKey.DoubleQuotationString("tokenizer"), DataValue.DoubleQuotationString(customAnalyzerAttribute.Tokenizer));

            List<string> tokenFilters = new List<string>();

            var builtInTokenFilters = customAnalyzerAttribute.BuiltInTokenFilters.ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();
            tokenFilters.AddRange(builtInTokenFilters);

            if (customAnalyzerAttribute.CustomTokenFilters != null && customAnalyzerAttribute.CustomTokenFilters.Length > 0)
            {
                tokenFilters.AddRange(customAnalyzerAttribute.CustomTokenFilters);
            }

            if (tokenFilters.Count > 0)
            {
                var dataArray = dataObject.AddDataArray(DataKey.DoubleQuotationString("filter"));
                foreach (var item in tokenFilters)
                {
                    dataArray.AddDataValue(DataValue.DoubleQuotationString(item));
                }
            }

            return dataObject;
        }
    }
}
