using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class EsFieldsEngineTest3
    {
        [Index(IndexName = "book-index", Aliases = new string[] { "book-alias" }, Dynamic = Dynamic.False, NumberOfShards = 1, NumberOfReplicas = 4, MappingTotalFieldsLimit = 50000)]
        [NGramTokenizer("trigram_tokenizer", 1, 3, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
        [EdgeNGramTokenizer("edge_ten_tokenizer", 1, 10, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
        [EdgeNGramTokenizer("edge_twenty_tokenizer", 1, 20, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
        [PatternTokenizer("comma_tokenizer", ",")]
        [CustomAnalyzer("trigram_analyzer", "trigram_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
        [CustomAnalyzer("edge_ten_analyzer", "edge_ten_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
        [CustomAnalyzer("edge_twenty_analyzer", "edge_twenty_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
        [CustomAnalyzer("comma_analyzer", "comma_tokenizer")]
        public class Book
        {
            /// <summary>
            /// 只存储，不索引
            /// </summary>
            [Field(Index = false, DocValues = false)]
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
                BuiltInAnalyzer = BuiltInAnalyzer.SIMPLE | BuiltInAnalyzer.WHITESPACE,
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
     * WithNullValue
     */
    public final static String WITH_NULL_VALUE = ""with_null_value"";

    /**
     * WithNullValue(without analyzer)
     */
    public final static String WITH_NULL_VALUE_KEYWORD = ""with_null_value.keyword"";

    /**
     * UseAnalyzer
     */
    public final static String USE_ANALYZER = ""use_analyzer"";

    /**
     * UseAnalyzer(without analyzer)
     */
    public final static String USE_ANALYZER_KEYWORD = ""use_analyzer.keyword"";

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

    /**
     * WithDefaultAnalyzer(without analyzer)
     */
    public final static String WITH_DEFAULT_ANALYZER_KEYWORD = ""with_default_analyzer.keyword"";
}
";
        }

        private string PrepareMappings()
        {
            return @"{
  ""aliases"": {
    ""book-alias"": {}
  },
  ""settings"": {
    ""number_of_shards"": 1,
    ""number_of_replicas"": 4,
    ""mapping.total_fields.limit"": 50000,
    ""analysis"": {
      ""tokenizer"": {
        ""trigram_tokenizer"": {
          ""type"": ""ngram"",
          ""min_gram"": 1,
          ""max_gram"": 3,
          ""token_chars"": [
            ""letter"",
            ""digit""
          ]
        },
        ""edge_ten_tokenizer"": {
          ""type"": ""edge_ngram"",
          ""min_gram"": 1,
          ""max_gram"": 10,
          ""token_chars"": [
            ""letter"",
            ""digit""
          ]
        },
        ""edge_twenty_tokenizer"": {
          ""type"": ""edge_ngram"",
          ""min_gram"": 1,
          ""max_gram"": 20,
          ""token_chars"": [
            ""letter"",
            ""digit""
          ]
        },
        ""comma_tokenizer"": {
          ""type"": ""pattern"",
          ""pattern"": "",""
        }
      },
      ""analyzer"": {
        ""trigram_analyzer"": {
          ""type"": ""custom"",
          ""tokenizer"": ""trigram_tokenizer"",
          ""filter"": [
            ""lowercase""
          ]
        },
        ""edge_ten_analyzer"": {
          ""type"": ""custom"",
          ""tokenizer"": ""edge_ten_tokenizer"",
          ""filter"": [
            ""lowercase""
          ]
        },
        ""edge_twenty_analyzer"": {
          ""type"": ""custom"",
          ""tokenizer"": ""edge_twenty_tokenizer"",
          ""filter"": [
            ""lowercase""
          ]
        },
        ""comma_analyzer"": {
          ""type"": ""custom"",
          ""tokenizer"": ""comma_tokenizer""
        }
      }
    }
  },
  ""mappings"": {
    ""_doc"": {
      ""dynamic"": false,
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
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
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
          ""analyzer"": ""ik_smart"",
          ""fields"": {
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
            }
          }
        },
        ""with_null_value"": {
          ""type"": ""text"",
          ""null_value"": ""NULL"",
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
