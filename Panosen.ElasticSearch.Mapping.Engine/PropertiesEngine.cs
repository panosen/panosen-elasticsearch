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
    /// PropertiesEngine
    /// </summary>
    public class PropertiesEngine
    {
        /// <summary>
        /// BuildProperties
        /// </summary>
        public void BuildProperties(DataObject dataObject, Type type, bool nested = false, int depth = 0)
        {
            if (depth > 8)
            {
                return;
            }

            if (nested)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));
            }

            var properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));

            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyName = propertyInfo.Name.ToLowerCaseUnderLine();
                var fieldAttribute = propertyInfo.GetCustomAttribute<FieldAttribute>(false);

                //如果不是 List<T>，则跳过
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
                    if (genericType.FullName != "System.Collections.Generic.List`1")
                    {
                        continue;
                    }

                    BuildProperty(properties, propertyName, fieldAttribute, propertyInfo.PropertyType.GenericTypeArguments[0], depth);

                    continue;
                }

                BuildProperty(properties, propertyName, fieldAttribute, propertyInfo.PropertyType, depth);
            }
        }

        private void BuildProperty(SortedDataObject properties, string propertyName, FieldAttribute fieldAttribute, Type propertyType, int depth)
        {
            if (fieldAttribute != null && !string.IsNullOrEmpty(fieldAttribute.Name))
            {
                propertyName = fieldAttribute.Name;
            }

            var sortedDataObject = properties.AddDataObject(DataKey.DoubleQuotationString(propertyName));

            if (fieldAttribute != null)
            {
                ProcessFieldAttribute(sortedDataObject, fieldAttribute, propertyType, depth);
            }
            else
            {
                ProcessPropertyInfo(sortedDataObject, propertyType, depth);
            }
        }

        private void ProcessFieldAttribute(DataObject dataObject, FieldAttribute fieldAttribute, Type propertyType, int depth)
        {
            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        new IntegerFiledEngine().Generate(dataObject, fieldAttribute as IntegerFieldAttribute);
                    }
                    break;
                case FieldType.Long:
                    {
                        new LongFiledEngine().Generate(dataObject, fieldAttribute as LongFieldAttribute);
                    }
                    break;
                case FieldType.Keyword:
                    {
                        new KeywordFiledEngine().Generate(dataObject, fieldAttribute as KeywordFieldAttribute);
                    }
                    break;
                case FieldType.Text:
                    {
                        new TextFiledEngine().Generate(dataObject, fieldAttribute as TextFieldAttribute);
                    }
                    break;
                case FieldType.NestedObject:
                    {
                        BuildProperties(dataObject, propertyType, nested: true, depth: depth + 1);
                    }
                    break;
                default:
                    {
                        new FieldEngine().Generate(dataObject, fieldAttribute);
                    }
                    break;
            }
        }

        private void ProcessPropertyInfo(DataObject dataObject, Type propertyType, int depth)
        {
            switch (propertyType.ToString())
            {
                case "System.Int32":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.INTEGER));
                    return;

                case "System.Int64":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.LONG));
                    return;

                case "System.Double":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.DOUBLE));
                    return;

                case "System.Boolean":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.BOOLEAN));
                    return;

                case "System.DateTime":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.DATE));
                    return;

                case "System.String":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.TEXT));

                    var keyword = dataObject.AddDataObject(DataKey.DoubleQuotationString("fields")).AddDataObject(DataKey.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), 256);
                    return;

                default:
                    break;
            }

            if (!propertyType.IsPrimitive)
            {
                BuildProperties(dataObject, propertyType, depth: depth + 1);
            }
        }
    }
}
