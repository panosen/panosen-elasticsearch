using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;
using System.Collections.Generic;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class EsFieldsEngineTest4
    {
        [Index]
        public class Book
        {
            /// <summary>
            /// 推断 int
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// 推断 long
            /// </summary>
            public Brand Brand { get; set; }

            /// <summary>
            /// 推断 long
            /// </summary>
            public List<Brand> BrandList { get; set; }

            /// <summary>
            /// 推断 long
            /// </summary>
            [NestedField]
            public List<Brand> NestedBrandList { get; set; }
        }

        public class Brand
        {
            public int BrandId { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var type = typeof(Book);

            {
                var classNode = ClassLoader.LoadClass(type);

                DocFields docFields = new DocFields();
                docFields.ClassNode = classNode;
                docFields.JavaRoot = "Sample";
                docFields.RootNamespace = "ok";

                var actual = docFields.TransformText();

                var expected = PrepareJava();

                Assert.AreEqual(expected, actual);
            }

            {
                var mappings = new Mappings();
                mappings.Type = type;

                var actual = mappings.TransformText();

                var expected = PrepareMappings();

                Assert.AreEqual(expected, actual);
            }
        }

        private string PrepareJava()
        {
            return @"package Sample;

/*
 *------------------------------------------------------------------------------
 *     DO NOT GO GENTLE INTO THAT GOOD NIGHT.
 *
 *     harriszhang@live.cn
 *------------------------------------------------------------------------------
 */

public final class BookFields {

    /**
     * Id
     */
    public final static String ID = ""id"";

    /**
     * Brand
     */
    public final static String BRAND = ""brand"";

    /**
     * BrandList
     */
    public final static String BRAND_LIST = ""brand_list"";

    /**
     * NestedBrandList
     */
    public final static String NESTED_BRAND_LIST = ""nested_brand_list"";
}
";
        }

        private string PrepareMappings()
        {
            return @"{
  ""mappings"": {
    ""_doc"": {
      ""properties"": {
        ""brand"": {
          ""properties"": {
            ""brand_id"": {
              ""type"": ""integer""
            }
          }
        },
        ""brand_list"": {
          ""properties"": {
            ""brand_id"": {
              ""type"": ""integer""
            }
          }
        },
        ""id"": {
          ""type"": ""integer""
        },
        ""nested_brand_list"": {
          ""type"": ""nested"",
          ""properties"": {
            ""brand_id"": {
              ""type"": ""integer""
            }
          }
        }
      }
    }
  }
}
";
        }
    }
}
