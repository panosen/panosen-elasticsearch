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
                ProcessProperty(codeClass, propertyNode);
            }
        }

        private void ProcessProperty(CodeClass codeClass, PropertyNode propertyNode)
        {
            var indexMe = CalcIndexMe(propertyNode);
            if (indexMe == Index.False)
            {
                return;
            }

            codeClass.AddField(JavaTypeConstant.STRING, propertyNode.Name.ToUpperCaseUnderLine(),
                accessModifiers: AccessModifiers.Public,
                isStatic: true,
                isFinal: true,
                summary: propertyNode.Summary ?? propertyNode.Name)
                .AddStringValue(propertyNode.Name.ToLowerCaseUnderLine());

            if (propertyNode.Attributes != null && propertyNode.Attributes.Count != 0)
            {
                ProcessFieldAttribute(codeClass, propertyNode.Name, propertyNode.Summary, propertyNode.Attributes[0] as FieldAttribute);
            }
            else
            {
                ProcessType(codeClass, propertyNode.Name, propertyNode.Summary, propertyNode.PropertyType);
            }
        }

        private void ProcessType(CodeClass codeClass, string propertyName, string propertySummary, Type propertyType)
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

        private void ProcessFieldAttribute(CodeClass codeClass, string propertyName, string propertySummary, FieldAttribute fieldAttribute)
        {
            if (fieldAttribute == null)
            {
                return;
            }

            var textFieldAttribute = fieldAttribute as TextFieldAttribute;
            if (textFieldAttribute != null)
            {
                ProcessAnalyzer(codeClass, propertyName, propertySummary,
                    textFieldAttribute.BuiltInAnalyzer, textFieldAttribute.IKAnalyzer, textFieldAttribute.CustomAnalyzer);
            }

            var keywordFieldAttribute = fieldAttribute as KeywordFieldAttribute;
            if (keywordFieldAttribute != null)
            {
                ProcessAnalyzer(codeClass, propertyName, propertySummary,
                    keywordFieldAttribute.BuiltInAnalyzer, keywordFieldAttribute.IKAnalyzer, keywordFieldAttribute.CustomAnalyzer);
            }
        }
        /// <summary>
        /// ProcessAnalyzer
        /// </summary>
        public void ProcessAnalyzer(CodeClass codeClass, string propertyName, string propertySummary, BuiltInAnalyzer builtInAnalyzer, IKAnalyzer iKAnalyzer, string[] customAnalyzer)
        {
            //分词器
            List<string> analyzers = new List<string>();

            //内置分词器
            var builtInAnalyzers = builtInAnalyzer
                .ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(x => x)
                .ToList();
            analyzers.AddRange(builtInAnalyzers);

            //ik分词器
            var ikAnalyzers = iKAnalyzer
                .ToString()
                .ToLower()
                .Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(x => x)
                .ToList();
            analyzers.AddRange(ikAnalyzers);

            //自定义分词器
            if (customAnalyzer != null && customAnalyzer.Length > 0)
            {
                analyzers.AddRange(customAnalyzer);
            }

            foreach (var analyzer in analyzers)
            {
                if ("none".Equals(analyzer, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                codeClass.AddField(JavaTypeConstant.STRING, $"{propertyName.ToUpperCaseUnderLine()}_{analyzer.ToUpperCaseUnderLine()}",
                    accessModifiers: AccessModifiers.Public,
                    isStatic: true,
                    isFinal: true,
                    summary: $"{propertySummary ?? propertyName}(with `{analyzer}` analyzer)")
                    .AddStringValue($"{propertyName.ToLowerCaseUnderLine()}.{analyzer}");
            }
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
