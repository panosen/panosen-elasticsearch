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
        public void BuildProperties(DataObject dataObject, Type type)
        {
            var properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));

            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                //如果不是 List<T>，则跳过
                if (propertyInfo.PropertyType.IsGenericType)
                {
                    var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
                    if (genericType.FullName != "System.Collections.Generic.List`1")
                    {
                        continue;
                    }
                }

                BuildProperty(properties, propertyInfo);
            }
        }

        private void BuildProperty(SortedDataObject properties, PropertyInfo propertyInfo)
        {
            var propertyName = propertyInfo.Name.ToLowerCaseUnderLine();

            var fieldAttribute = propertyInfo.GetCustomAttribute<FieldAttribute>(false);
            if (fieldAttribute != null && !string.IsNullOrEmpty(fieldAttribute.Name))
            {
                propertyName = fieldAttribute.Name;
            }

            var sortedDataObject = properties.AddDataObject(DataKey.DoubleQuotationString(propertyName));

            if (fieldAttribute != null)
            {
                ProcessFieldAttribute(sortedDataObject, fieldAttribute, propertyInfo);
            }
            else
            {
                ProcessPropertyInfo(sortedDataObject, propertyInfo);
            }
        }

        private void ProcessFieldAttribute(DataObject dataObject, FieldAttribute fieldAttribute, PropertyInfo propertyInfo)
        {
            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        new IntegerFiledEngine().Generate(dataObject, fieldAttribute as IntegerFieldAttribute, propertyInfo);
                    }
                    break;
                case FieldType.Long:
                    {
                        new LongFiledEngine().Generate(dataObject, fieldAttribute as LongFieldAttribute, propertyInfo);
                    }
                    break;
                case FieldType.Keyword:
                    {
                        new KeywordFiledEngine().Generate(dataObject, fieldAttribute as KeywordFieldAttribute, propertyInfo);
                    }
                    break;
                case FieldType.Text:
                    {
                        new TextFiledEngine().Generate(dataObject, fieldAttribute as TextFieldAttribute, propertyInfo);
                    }
                    break;
                case FieldType.NestedObject:
                    {
                        new NestedFiledEngine().Generate(dataObject, fieldAttribute as NestedFieldAttribute, propertyInfo);
                    }
                    break;
                default:
                    {
                        new FieldEngine().Generate(dataObject, fieldAttribute, propertyInfo);
                    }
                    break;
            }
        }

        private void ProcessPropertyInfo(DataObject dataObject, PropertyInfo propertyInfo)
        {
            switch (propertyInfo.PropertyType.ToString())
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

                case "System.String":
                    dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(MappingTypes.TEXT));

                    var keyword = dataObject.AddDataObject(DataKey.DoubleQuotationString("fields")).AddDataObject(DataKey.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                    keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), 256);
                    return;

                default:
                    break;
            }

            if (!propertyInfo.PropertyType.IsPrimitive)
            {
                BuildProperties(dataObject, propertyInfo.PropertyType);
            }
        }
    }
}
