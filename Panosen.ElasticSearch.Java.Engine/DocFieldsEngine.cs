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
    public class DocFieldsEngine
    {
        /// <summary>
        /// 用于拆分枚举
        /// </summary>
        private static readonly string[] CommaAndWhitespace = new string[] { " ", "," };

        public string Generate(DocFields docFields)
        {
            return PrepareCodeFile(docFields).TransformText();
        }

        private CodeFile PrepareCodeFile(DocFields docFields)
        {
            CodeFile codeFile = new CodeFile();

            codeFile.AddMotto("DO NOT GO GENTLE INTO THAT GOOD NIGHT.");
            codeFile.AddMotto("harriszhang@live.cn");

            codeFile.PackageName = docFields.JavaRoot;

            CodeClass codeClass = codeFile.AddClass($"{docFields.ClassNode.Name}Fields");
            codeClass.Summary = docFields.ClassNode.Summary;
            codeClass.AccessModifiers = AccessModifiers.Public;
            codeClass.IsFinal = true;

            AddProperty(codeClass, docFields);

            return codeFile;
        }

        private void AddProperty(CodeClass codeClass, DocFields docFields)
        {
            if (docFields.ClassNode.PropertyNodeList == null || docFields.ClassNode.PropertyNodeList.Count <= 0)
            {
                return;
            }

            foreach (var propertyNode in docFields.ClassNode.PropertyNodeList)
            {
                if (propertyNode.Attributes != null && propertyNode.Attributes.Count > 0)
                {
                    var fieldAttribute = propertyNode.Attributes[0] as FieldAttribute;
                    if (fieldAttribute != null && !fieldAttribute.Index)
                    {
                        continue;
                    }
                }

                AddField(codeClass, propertyNode.Name.ToUpperCaseUnderLine(), propertyNode.Name.ToLowerCaseUnderLine(), propertyNode.Summary ?? propertyNode.Name);

                ProcessTextField(codeClass, propertyNode);
            }
        }

        private void ProcessTextField(CodeClass codeClass, PropertyNode propertyNode)
        {
            if (propertyNode.Attributes == null || propertyNode.Attributes.Count == 0)
            {
                return;
            }

            var textFieldAttribute = propertyNode.Attributes[0] as TextFieldAttribute;
            if (textFieldAttribute == null)
            {
                return;
            }

            //public final static String Text = "text.keyword";
            AddField(codeClass, $"{propertyNode.Name.ToUpperCaseUnderLine()}_KEYWORD", $"{propertyNode.Name.ToLowerCaseUnderLine()}.keyword", $"{propertyNode.Summary ?? propertyNode.Name}(without analyzer)");

            //分词器
            List<string> analyzers = new List<string>();

            //内置分词器
            var builtInAnalyzers = textFieldAttribute.BuiltInAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
            analyzers.AddRange(builtInAnalyzers);

            //ik分词器
            var ikAnalyzers = textFieldAttribute.IKAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
            analyzers.AddRange(ikAnalyzers);

            //自定义分词器
            if (textFieldAttribute.CustomAnalyzer != null && textFieldAttribute.CustomAnalyzer.Length > 0)
            {
                analyzers.AddRange(textFieldAttribute.CustomAnalyzer);
            }

            foreach (var analyzer in analyzers)
            {
                if ("none".Equals(analyzer, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                //public final static String Text_Analyzer = "text.analyzer";
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
