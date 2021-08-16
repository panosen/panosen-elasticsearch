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
    partial class MappingsEngine
    {
        private void BuildProperties(SortedDataObject properties, Type type)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var key = propertyInfo.Name.ToLowerCaseUnderLine();
                var field = GetFieldAttribute(propertyInfo, propertyInfo.Name.ToLowerCaseUnderLine());
                if (field != null)
                {
                    key = field.Name;
                }

                var dataObject = properties.AddDataObject(DataKey.DoubleQuotationString(key));

                if (propertyInfo.PropertyType.IsGenericType)
                {
                    //如果不是 List<T>，则跳过
                    var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
                    if (genericType.FullName != "System.Collections.Generic.List`1")
                    {
                        continue;
                    }

                    if (field != null)
                    {
                        if (field is NestedFieldAttribute)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));
                            var _properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
                            BuildProperties(_properties, propertyInfo.PropertyType.GenericTypeArguments[0]);
                        }
                        else
                        {
                            BuildProperty(dataObject, field);
                        }
                    }
                    else
                    {
                        var _properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
                        BuildProperties(_properties, propertyInfo.PropertyType.GenericTypeArguments[0]);
                    }
                }
                else
                {

                    if (field is NestedFieldAttribute)
                    {

                    }
                    else
                    {
                        BuildProperty(dataObject, field);
                    }


                    //var field = GetFieldAttribute(property, property.Name.ToLowerCaseUnderLine());
                    //if (field != null)
                    //{
                    //    returnValue.AddDataObject(DataKey.DoubleQuotationString(field.Name.ToLowerCaseUnderLine()), );
                    //}

                    //DataObject propertyDataObject = new DataObject();
                    //returnValue.AddDataObject(property.Name.ToLowerCaseUnderLine(), propertyDataObject);

                    //DataObject propertiesDataObject = propertyDataObject.AddDataObject("properties");
                    //if (field != null && field is NestedFieldAttribute)
                    //{
                    //    propertiesDataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));
                    //}
                    //foreach (var item in nestProperties.DataItemMap)
                    //{
                    //    propertiesDataObject.Add(item.Key, item.Value);
                    //}
                }
            }
        }

        private void BuildProperty(DataObject dataObject, FieldAttribute fieldAttribute)
        {
            if (dataObject == null || fieldAttribute == null)
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
                default:
                    break;
            }
        }
    }
}
