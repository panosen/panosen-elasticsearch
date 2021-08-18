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
                ProcessProperties(codeClass, propertyNode);
            }
        }

        private void ProcessProperties(CodeClass codeClass, PropertyNode propertyNode)
        {
            var indexMe = CalcIndexMe(propertyNode);
            if (indexMe == Index.False)
            {
                return;
            }

            AddField(codeClass, propertyNode.Name.ToUpperCaseUnderLine(), propertyNode.Name.ToLowerCaseUnderLine(), propertyNode.Summary ?? propertyNode.Name);

            ProcessAnalyzer(codeClass, propertyNode);
        }

        private static Index CalcIndexMe(PropertyNode propertyNode)
        {
            if (propertyNode.Attributes == null || propertyNode.Attributes.Count <= 0)
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

        private void ProcessAnalyzer(CodeClass codeClass, PropertyNode propertyNode)
        {
            if (propertyNode.Attributes == null || propertyNode.Attributes.Count == 0)
            {
                return;
            }

            var textFieldAttribute = propertyNode.Attributes[0] as TextFieldAttribute;
            if (textFieldAttribute != null)
            {
                //AddField(codeClass, $"{propertyNode.Name.ToUpperCaseUnderLine()}_KEYWORD", $"{propertyNode.Name.ToLowerCaseUnderLine()}.keyword", $"{propertyNode.Summary ?? propertyNode.Name}(without analyzer)");

                ProcessAnalyzer(codeClass, propertyNode, textFieldAttribute.BuiltInAnalyzer, textFieldAttribute.IKAnalyzer, textFieldAttribute.CustomAnalyzer);
            }

            var keywordFieldAttribute = propertyNode.Attributes[0] as KeywordFieldAttribute;
            if (keywordFieldAttribute != null)
            {
                ProcessAnalyzer(codeClass, propertyNode, keywordFieldAttribute.BuiltInAnalyzer, keywordFieldAttribute.IKAnalyzer, keywordFieldAttribute.CustomAnalyzer);
            }
        }

        private void ProcessAnalyzer(CodeClass codeClass, PropertyNode propertyNode, BuiltInAnalyzer builtInAnalyzer, IKAnalyzer iKAnalyzer, string[] customAnalyzer)
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

                AddField(codeClass, $"{propertyNode.Name.ToUpperCaseUnderLine()}_{analyzer.ToUpperCaseUnderLine()}", $"{propertyNode.Name.ToLowerCaseUnderLine()}.{analyzer}", $"{propertyNode.Summary ?? propertyNode.Name}(with `{analyzer}` analyzer)");
            }
        }

        private void AddField(CodeClass codeClass, string key, string value, string summary)
        {
            var codeField = codeClass.AddField(JavaTypeConstant.STRING, key);
            codeField.AddStringValue(value);
            codeField.AccessModifiers = AccessModifiers.Public;
            codeField.IsStatic = true;
            codeField.IsFinal = true;
            codeField.Summary = summary;
        }
    }
}
