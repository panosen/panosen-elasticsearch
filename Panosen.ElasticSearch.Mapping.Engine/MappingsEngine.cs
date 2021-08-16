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
            var settingsDataObject = BuildSettings(indexAttribute, customTokenizerAttributeList, customFilterAttributeList, customAnalyzerAttributeList);
            if (settingsDataObject != null)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("settings"), settingsDataObject);
            }

            //mappings
            var mappingsDataObject = BuildMappings(type);
            if (mappingsDataObject != null)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("mappings"), mappingsDataObject);
            }

            return dataObject;
        }


        private DataObject BuildFilterProperties(CustomFilterAttribute customFilterAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(customFilterAttribute.Type));

            if (customFilterAttribute.Properties != null && customFilterAttribute.Properties.Length > 0 && customFilterAttribute.Properties.Length % 2 == 0)
            {
                for (int i = 0; i < customFilterAttribute.Properties.Length; i += 2)
                {
                    dataObject.AddDataValue(DataKey.DoubleQuotationString(customFilterAttribute.Properties[i]), DataValue.DoubleQuotationString(customFilterAttribute.Properties[i + 1]));
                }
            }

            return dataObject;
        }

        private DataObject BuildAnalyzerProperties(CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("custom"));
            dataObject.AddDataValue(DataKey.DoubleQuotationString("tokenizer"), DataValue.DoubleQuotationString(customAnalyzerAttribute.Tokenizer));

            {
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
            var _doc = dataObject.AddDataObject(DataKey.DoubleQuotationString(indexAttribute.TypeName ?? "_doc"));

            switch (indexAttribute.Dynamic)
            {
                case Dynamic.False:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), false);
                    break;
                case Dynamic.Strict:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("strict"));
                    break;
                case Dynamic.True:
                default:
                    break;
            }

            var _properties = _doc.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
            BuildProperties(_properties, type);

            return dataObject;
        }


        private FieldAttribute GetFieldAttribute(PropertyInfo field, string fieldName)
        {
            //处理简单类型
            FieldAttribute fieldAttribute = field.GetCustomAttribute<FieldAttribute>(false);
            if (fieldAttribute != null)
            {
                if (fieldAttribute.Name == null)
                {
                    fieldAttribute.Name = fieldName;
                }
                return fieldAttribute;
            }

            return PropertyTypeAsFieldAttribute(field.PropertyType, fieldName);
        }

        /// <summary>
        /// 根据c#类型，推断field类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private FieldAttribute PropertyTypeAsFieldAttribute(Type type, string fieldName)
        {
            var fieldAttribute = BasicPropertyTypeAsFieldAttribute(type, fieldName);
            if (fieldAttribute != null)
            {
                return fieldAttribute;
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType.FullName == "System.Collections.Generic.List`1")
                {
                    return BasicPropertyTypeAsFieldAttribute(type.GenericTypeArguments[0], fieldName);
                }
            }

            return null;
        }

        private FieldAttribute BasicPropertyTypeAsFieldAttribute(Type type, string fieldName)
        {
            switch (type.ToString())
            {
                case "System.Int32":
                    return new IntegerFieldAttribute { Name = fieldName };
                case "System.Int64":
                    return new LongFieldAttribute { Name = fieldName };
                case "System.Double":
                    return new DoubleFieldAttribute { Name = fieldName };
                case "System.Boolean":
                    return new BooleanFieldAttribute { Name = fieldName };
                case "System.String":
                    return new TextFieldAttribute { Name = fieldName };
                default:
                    return null;
            }
        }
    }
}
