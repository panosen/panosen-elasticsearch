using Panosen.ElasticSearch;
using Panosen.CodeDom.Java;
using Panosen.CodeDom.Java.Engine;
using Panosen.Language.Java;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panosen.Reflection.Model;

namespace Panosen.ElasticSearch.Java.Engine
{
    /// <summary>
    /// DocFieldsEngine
    /// </summary>
    public class DocFieldsEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public string Generate(DocFields docFields)
        {
            CodeFile codeFile = new CodeFile();

            codeFile.AddMotto("DO NOT GO GENTLE INTO THAT GOOD NIGHT.");
            codeFile.AddMotto("harriszhang@live.cn");

            codeFile.PackageName = docFields.JavaRoot;

            CodeClass codeClass = codeFile.AddClass($"{docFields.ClassNode.Name}Fields");
            codeClass.Summary = docFields.ClassNode.Summary;
            codeClass.AccessModifiers = AccessModifiers.Public;
            codeClass.IsFinal = true;

            ProcessPropertyList(codeClass, docFields);

            return codeFile.TransformText();
        }

        private void ProcessPropertyList(CodeClass codeClass, DocFields docFields)
        {
            if (docFields.ClassNode.PropertyNodeList == null || docFields.ClassNode.PropertyNodeList.Count == 0)
            {
                return;
            }

            foreach (var propertyNode in docFields.ClassNode.PropertyNodeList)
            {
                var propertyName = propertyNode.Name;
                var propertySummary = propertyNode.Summary;
                var propertyType = propertyNode.PropertyType;

                var fieldAttribute = propertyNode.Attributes.FirstOrDefault(v => v is FieldAttribute) as FieldAttribute;
                var fieldsAttribute = propertyNode.Attributes.Where(v => v is FieldsAttribute).Select(v => v as FieldsAttribute).ToList();

                codeClass.AddField(JavaTypeConstant.STRING, propertyName.ToUpperCaseUnderLine(),
                    accessModifiers: AccessModifiers.Public,
                    isStatic: true,
                    isFinal: true,
                    summary: propertySummary ?? propertyName)
                    .AddStringValue(propertyName.ToLowerCaseUnderLine());

                if (fieldAttribute != null)
                {
                    ProcessFieldAttribute(codeClass, propertyName, propertySummary, fieldAttribute, fieldsAttribute);
                }
                else
                {
                    ProcessProperty(codeClass, propertyName, propertySummary, propertyType);
                }
            }
        }

        private void ProcessFieldAttribute(CodeClass codeClass, string propertyName, string propertySummary, FieldAttribute fieldAttribute, List<FieldsAttribute> fieldsAttributes)
        {
            if (fieldAttribute == null)
            {
                return;
            }

            if (fieldsAttributes == null || fieldsAttributes.Count == 0)
            {
                return;
            }

            if (fieldAttribute is KeywordFieldAttribute || fieldAttribute is TextFieldAttribute)
            {
                List<CodeField> temps = new List<CodeField>();
                foreach (var item in fieldsAttributes)
                {
                    var temp = ProcessFields(codeClass, propertyName, propertySummary, item);
                    if (temp == null)
                    {
                        continue;
                    }
                    temps.Add(temp);
                }
                codeClass.AddFields(temps.OrderBy(x => x.Name).ToList());
            }
        }

        private void ProcessProperty(CodeClass codeClass, string propertyName, string propertySummary, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                codeClass.AddField(JavaTypeConstant.STRING, $"{propertyName.ToUpperCaseUnderLine()}_KEYWORD",
                    accessModifiers: AccessModifiers.Public,
                    isStatic: true,
                    isFinal: true,
                    summary: $"{propertySummary ?? propertyName}.Keyword")
                     .AddStringValue($"{propertyName.ToLowerCaseUnderLine()}.keyword");
            }
        }

        private CodeField ProcessFields(CodeClass codeClass, string propertyName, string propertySummary, FieldsAttribute fieldsAttribute)
        {
            var keywordFieldsAttribute = fieldsAttribute as KeywordFieldsAttribute;
            if (keywordFieldsAttribute != null)
            {
                var field = new CodeField();
                field.Name = $"{propertyName.ToUpperCaseUnderLine()}_{(keywordFieldsAttribute.Name ?? "Keyword").ToUpperCaseUnderLine()}";
                field.Summary = $"{propertySummary ?? propertyName}(type: keyword)";
                field.AddStringValue($"{propertyName.ToLowerCaseUnderLine()}.{(keywordFieldsAttribute.Name ?? "Keyword").ToLowerCaseUnderLine()}");
                field.AccessModifiers = AccessModifiers.Public;
                field.Type = JavaTypeConstant.STRING;
                field.IsStatic = true;
                field.IsFinal = true;

                return field;
            }

            var textFieldsAttribute = fieldsAttribute as TextFieldsAttribute;
            if (textFieldsAttribute != null)
            {
                var analyzer = textFieldsAttribute.Analyzer;

                var field = new CodeField();
                field.Name = $"{propertyName.ToUpperCaseUnderLine()}_{analyzer.ToUpperCaseUnderLine()}";
                field.Summary = $"{propertySummary ?? propertyName}(with `{analyzer}` analyzer)";
                field.AddStringValue($"{propertyName.ToLowerCaseUnderLine()}.{analyzer}");
                field.AccessModifiers = AccessModifiers.Public;
                field.Type = JavaTypeConstant.STRING;
                field.IsStatic = true;
                field.IsFinal = true;

                return field;
            }

            return null;
        }

        private static Index CalcIndexMe(PropertyNode propertyNode)
        {
            if (propertyNode.Attributes == null || propertyNode.Attributes.Count == 0)
            {
                return Index.None;
            }

            foreach (var attribute in propertyNode.Attributes)
            {
                var fieldAttribute = attribute as FieldAttribute;
                if (fieldAttribute == null)
                {
                    continue;
                }

                return fieldAttribute.Index;
            }

            return Index.None;
        }
    }
}
