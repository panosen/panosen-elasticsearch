﻿using System;
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
        public SortedDataObject BuildProperties(Type type, int depth = 0)
        {
            if (depth > 8)
            {
                return null;
            }

            SortedDataObject properties = new SortedDataObject();

            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyName = propertyInfo.Name.ToLowerCaseUnderLine();

                var fieldAttribute = propertyInfo.GetCustomAttribute<FieldAttribute>(false);
                if (fieldAttribute != null && !string.IsNullOrEmpty(fieldAttribute.Name))
                {
                    propertyName = fieldAttribute.Name;
                }

                var fieldsAttributes = propertyInfo.GetCustomAttributes<WithFieldsAttribute>(false).ToList();

                //如果不是 List<T>，则跳过
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
                    if (genericType.FullName != "System.Collections.Generic.List`1")
                    {
                        continue;
                    }

                    var dataObject = BuildProperty(fieldAttribute, fieldsAttributes, propertyInfo.PropertyType.GenericTypeArguments[0], depth);
                    properties.AddDataObject(DataKey.DoubleQuotationString(propertyName), dataObject);
                }
                else
                {
                    var dataObject = BuildProperty(fieldAttribute, fieldsAttributes, propertyInfo.PropertyType, depth);
                    properties.AddDataObject(DataKey.DoubleQuotationString(propertyName), dataObject);
                }
            }

            return properties;
        }

        private DataObject BuildProperty(FieldAttribute fieldAttribute, List<WithFieldsAttribute> fieldsAttributes, Type propertyType, int depth)
        {
            var dataObject = new DataObject();

            if (fieldAttribute != null)
            {
                ProcessFieldAttribute(dataObject, fieldAttribute, fieldsAttributes, propertyType, depth);
            }
            else
            {
                ProcessPropertyType(dataObject, propertyType, Index.None, DocValues.None, depth);
            }

            return dataObject;
        }

        private void ProcessFieldAttribute(DataObject dataObject, FieldAttribute fieldAttribute, List<WithFieldsAttribute> fieldsAttributes, Type propertyType, int depth)
        {
            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        new IntegerFiledEngine().Generate(dataObject, fieldAttribute as IntegerFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.Boolean:
                    {
                        new BooleanFiledEngine().Generate(dataObject, fieldAttribute as BooleanFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.Long:
                    {
                        new LongFiledEngine().Generate(dataObject, fieldAttribute as LongFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.Keyword:
                    {
                        new KeywordFieldEngine().Generate(dataObject, fieldAttribute as KeywordFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.Text:
                    {
                        new TextFiledEngine().Generate(dataObject, fieldAttribute as TextFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.GeoPoint:
                    {
                        new GeoPointFiledEngine().Generate(dataObject, fieldAttribute as GeoPointFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.Object:
                    {
                        new ObjectFiledEngine().Generate(dataObject, fieldAttribute as ObjectFieldAttribute, fieldsAttributes);
                    }
                    break;
                case FieldType.NestedObject:
                    {
                        dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));

                        var properties = BuildProperties(propertyType, depth: depth + 1);
                        if (properties != null && properties.DataItemMap != null && properties.DataItemMap.Count > 0)
                        {
                            dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"), properties);
                        }
                    }
                    break;
                default:
                    {
                        new FieldEngine().Generate(dataObject, fieldAttribute, fieldsAttributes);
                    }
                    break;
            }
        }

        /// <summary>
        /// ProcessPropertyType
        /// </summary>
        public void ProcessPropertyType(DataObject dataObject, Type propertyType, Index index, DocValues docValues, int depth)
        {
            if (propertyType == null)
            {
                ProcessPrimitive(dataObject, index, docValues, null);
                return;
            }

            switch (propertyType.ToString())
            {
                case "System.Int32":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.INTEGER);
                    return;

                case "System.Int64":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.LONG);
                    return;

                case "System.Single":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.FLOAT);
                    return;

                case "System.Double":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.DOUBLE);
                    return;

                case "System.Boolean":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.BOOLEAN);
                    return;

                case "System.DateTime":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.DATE);
                    return;

                case "System.String":
                    ProcessPrimitive(dataObject, index, docValues, MappingTypes.TEXT);

                    var keyword = dataObject.AddDataObject(DataKey.DoubleQuotationString("fields")).AddDataObject(DataKey.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), 256);
                    return;

                default:
                    break;
            }

            if (propertyType == typeof(Keyword))
            {
                ProcessPrimitive(dataObject, index, docValues, MappingTypes.KEYWORD);
            }

            if (!propertyType.IsPrimitive)
            {
                var properties = BuildProperties(propertyType, depth: depth + 1);
                if (properties != null && properties.DataItemMap != null && properties.DataItemMap.Count > 0)
                {
                    dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"), properties);
                }
            }
        }

        private static void ProcessPrimitive(DataObject dataObject, Index index, DocValues docValues, string mappingType)
        {
            if (index != Index.None)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("index"), index.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(mappingType))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(mappingType));
            }

            if (docValues != DocValues.None)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("doc_values"), docValues.ToString().ToLower());
            }
        }
    }
}
