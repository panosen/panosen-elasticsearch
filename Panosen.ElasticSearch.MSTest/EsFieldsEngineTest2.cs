using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class EsFieldsEngineTest2
    {
        [Index]
        public class Book
        {
            /// <summary>
            /// 使用 integer
            /// </summary>
            [IntegerField(123)]
            public int UseInteger { get; set; }

            /// <summary>
            /// 使用 long
            /// </summary>
            [LongField]
            public long UseLong { get; set; }

            /// <summary>
            /// 使用 keyword
            /// </summary>
            [KeywordField(IgnoreAbove = 128)]
            public string UseKeyword { get; set; }

            /// <summary>
            /// 使用 text
            /// </summary>
            [TextField]
            public string UseText { get; set; }
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

                var expected = PrepareExpected();

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

        private string PrepareExpected()
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
     * UseInteger
     */
    public final static String USE_INTEGER = ""use_integer"";

    /**
     * UseLong
     */
    public final static String USE_LONG = ""use_long"";

    /**
     * UseKeyword
     */
    public final static String USE_KEYWORD = ""use_keyword"";

    /**
     * UseText
     */
    public final static String USE_TEXT = ""use_text"";

    /**
     * UseText(without analyzer)
     */
    public final static String USE_TEXT_KEYWORD = ""use_text.keyword"";
}
";
        }

        private string PrepareMappings()
        {
            return @"{
  ""mappings"": {
    ""_doc"": {
      ""properties"": {
        ""use_integer"": {
          ""type"": ""integer"",
          ""null_value"": 123
        },
        ""use_keyword"": {
          ""type"": ""keyword"",
          ""ignore_above"": 128
        },
        ""use_long"": {
          ""type"": ""long""
        },
        ""use_text"": {
          ""type"": ""text"",
          ""fields"": {
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
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
