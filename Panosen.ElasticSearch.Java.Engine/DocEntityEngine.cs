using Panosen.CodeDom.Java;
using Panosen.CodeDom.Java.Engine;
using Panosen.Reflection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panosen.ElasticSearch.Java.Engine
{
    /// <summary>
    /// DocEntityEngine
    /// </summary>
    public class DocEntityEngine
    {
        /// <summary>
        /// Generate
        /// </summary>
        public string Generate(DocEntity docEntity)
        {
            return PrepareCodeFile(docEntity).TransformText();
        }

        private CodeFile PrepareCodeFile(DocEntity docEntity)
        {
            CodeFile codeFile = new CodeFile();

            codeFile.AddMotto("DO NOT GO GENTLE INTO THAT GOOD NIGHT.");
            codeFile.AddMotto("harriszhang@live.cn");

            codeFile.PackageName = docEntity.JavaRoot;
            codeFile.AddMavenImport("com.google.gson.annotations.SerializedName");

            CodeClass codeClass = codeFile.AddClass(docEntity.ClassNode.Name);
            codeClass.Summary = docEntity.ClassNode.Summary;
            codeClass.AccessModifiers = AccessModifiers.Public;

            if (docEntity.ClassNode.PropertyNodeList != null && docEntity.ClassNode.PropertyNodeList.Count > 0)
            {
                foreach (var propertyNode in docEntity.ClassNode.PropertyNodeList)
                {
                    var codeProperty = codeClass.AddProperty(propertyNode.PropertyType.ToJavaType(docEntity.RootNamespace), propertyNode.Name);
                    codeProperty.Summary = propertyNode.Summary;
                    codeProperty.AddAttribute("SerializedName")
                        .AddStringParam(codeProperty.Name.ToLowerCaseUnderLine());
                }
            }

            return codeFile;
        }
    }
}
