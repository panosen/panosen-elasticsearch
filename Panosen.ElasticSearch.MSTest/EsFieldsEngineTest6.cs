using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class EsFieldsEngineTest6
    {
        [Index]
        public class Book
        {
            /// <summary>
            /// 只存储，不索引
            /// </summary>
            [Field(Index = Index.False, DocValues = DocValues.False)]
            public string NotIndexMe { get; set; }

            /// <summary>
            /// 只存储，不索引
            /// </summary>
            [TextField(NullValue = "NULL")]
            public string WithNullValue { get; set; }

            /// <summary>
            /// with  analyzer
            /// </summary>
            [TextField(
                IKAnalyzer = IKAnalyzer.IK_SMART | IKAnalyzer.IK_MAX_WORD,
                BuiltInAnalyzer = BuiltInAnalyzer.Simple | BuiltInAnalyzer.Whitespace,
                CustomAnalyzer = new string[] { "ngram_1_1" })]
            public string UseAnalyzer { get; set; }

            /// <summary>
            /// 带默认分析器
            /// </summary>
            [TextField(IKAnalyzer.IK_SMART)]
            public string WithDefaultAnalyzer { get; set; }
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

        private static string PrepareExpected()
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
     * WithNullValue
     */
    public final static String WITH_NULL_VALUE = ""with_null_value"";

    /**
     * UseAnalyzer
     */
    public final static String USE_ANALYZER = ""use_analyzer"";

    /**
     * UseAnalyzer(with `simple` analyzer)
     */
    public final static String USE_ANALYZER_SIMPLE = ""use_analyzer.simple"";

    /**
     * UseAnalyzer(with `whitespace` analyzer)
     */
    public final static String USE_ANALYZER_WHITESPACE = ""use_analyzer.whitespace"";

    /**
     * UseAnalyzer(with `ik_max_word` analyzer)
     */
    public final static String USE_ANALYZER_IK_MAX_WORD = ""use_analyzer.ik_max_word"";

    /**
     * UseAnalyzer(with `ik_smart` analyzer)
     */
    public final static String USE_ANALYZER_IK_SMART = ""use_analyzer.ik_smart"";

    /**
     * UseAnalyzer(with `ngram_1_1` analyzer)
     */
    public final static String USE_ANALYZER_NGRAM_1_1 = ""use_analyzer.ngram_1_1"";

    /**
     * WithDefaultAnalyzer
     */
    public final static String WITH_DEFAULT_ANALYZER = ""with_default_analyzer"";
}
";
        }

        private static string PrepareMappings()
        {
            return @"{
  ""mappings"": {
    ""_doc"": {
      ""properties"": {
        ""not_index_me"": {
          ""index"": false,
          ""doc_values"": false
        },
        ""use_analyzer"": {
          ""type"": ""text"",
          ""fields"": {
            ""ik_max_word"": {
              ""type"": ""text"",
              ""analyzer"": ""ik_max_word""
            },
            ""ik_smart"": {
              ""type"": ""text"",
              ""analyzer"": ""ik_smart""
            },
            ""ngram_1_1"": {
              ""type"": ""text"",
              ""analyzer"": ""ngram_1_1""
            },
            ""simple"": {
              ""type"": ""text"",
              ""analyzer"": ""simple""
            },
            ""whitespace"": {
              ""type"": ""text"",
              ""analyzer"": ""whitespace""
            }
          }
        },
        ""with_default_analyzer"": {
          ""type"": ""text"",
          ""analyzer"": ""ik_smart""
        },
        ""with_null_value"": {
          ""type"": ""text"",
          ""null_value"": ""NULL""
        }
      }
    }
  }
}
";
        }
    }
}
