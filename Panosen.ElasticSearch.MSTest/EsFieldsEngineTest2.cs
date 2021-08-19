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
            [KeywordField(Index = Index.False, IgnoreAbove = 128)]
            [WithTextFields(IKAnalyzer.IK_SMART)]
            public string UseKeyword { get; set; }

            /// <summary>
            /// 使用 text
            /// </summary>
            [TextField]
            [WithTextFields(IKAnalyzer.IK_MAX_WORD)]
            [WithKeywordFields]
            [WithKeywordFields(Name = "TestCase")]
            [WithKeywordFields(Name = "SampleCase", IgnoreAbove = 256)]
            public string UseText { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var type = typeof(Book);

            var classNode = ClassLoader.LoadClass(type);

            {
                DocFields docFields = new DocFields();
                docFields.ClassNode = classNode;
                docFields.JavaRoot = "Sample";
                docFields.RootNamespace = type.ReflectedType.FullName;

                var actual = docFields.TransformText();

                var expected = PrepareExpected();

                Assert.AreEqual(expected, actual);
            }

            {
                DocEntity docEntity = new DocEntity();
                docEntity.ClassNode = classNode;
                docEntity.JavaRoot = "Sample";
                docEntity.RootNamespace = type.ReflectedType.FullName;

                var actual = docEntity.TransformText();

                var expected = PrepareEntity();

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
     * UseKeyword(with `ik_smart` analyzer)
     */
    public final static String USE_KEYWORD_IK_SMART = ""use_keyword.ik_smart"";

    /**
     * UseText
     */
    public final static String USE_TEXT = ""use_text"";

    /**
     * UseText(with `ik_max_word` analyzer)
     */
    public final static String USE_TEXT_IK_MAX_WORD = ""use_text.ik_max_word"";

    /**
     * UseText(type: keyword)
     */
    public final static String USE_TEXT_KEYWORD = ""use_text.keyword"";

    /**
     * UseText(type: keyword)
     */
    public final static String USE_TEXT_SAMPLE_CASE = ""use_text.sample_case"";

    /**
     * UseText(type: keyword)
     */
    public final static String USE_TEXT_TEST_CASE = ""use_text.test_case"";
}
";
        }

        private static string PrepareEntity()
        {
            return @"package Sample;

/*
 *------------------------------------------------------------------------------
 *     DO NOT GO GENTLE INTO THAT GOOD NIGHT.
 *
 *     harriszhang@live.cn
 *------------------------------------------------------------------------------
 */

import com.google.gson.annotations.SerializedName;

public class Book {

    @SerializedName(""use_integer"")
    private Integer useInteger;

    @SerializedName(""use_long"")
    private Long useLong;

    @SerializedName(""use_keyword"")
    private String useKeyword;

    @SerializedName(""use_text"")
    private String useText;

    /**
     * Get 
     */
    public Integer getUseInteger() {
        return useInteger;
    }

    /**
     * Set 
     */
    public void setUseInteger(Integer useInteger) {
        this.useInteger = useInteger;
    }

    /**
     * Get 
     */
    public Long getUseLong() {
        return useLong;
    }

    /**
     * Set 
     */
    public void setUseLong(Long useLong) {
        this.useLong = useLong;
    }

    /**
     * Get 
     */
    public String getUseKeyword() {
        return useKeyword;
    }

    /**
     * Set 
     */
    public void setUseKeyword(String useKeyword) {
        this.useKeyword = useKeyword;
    }

    /**
     * Get 
     */
    public String getUseText() {
        return useText;
    }

    /**
     * Set 
     */
    public void setUseText(String useText) {
        this.useText = useText;
    }
}
";
        }

        private static string PrepareMappings()
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
          ""index"": false,
          ""ignore_above"": 128,
          ""fields"": {
            ""ik_smart"": {
              ""type"": ""text"",
              ""analyzer"": ""ik_smart""
            }
          }
        },
        ""use_long"": {
          ""type"": ""long""
        },
        ""use_text"": {
          ""type"": ""text"",
          ""fields"": {
            ""ik_max_word"": {
              ""type"": ""text"",
              ""analyzer"": ""ik_max_word""
            },
            ""keyword"": {
              ""type"": ""keyword""
            },
            ""sample_case"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
            },
            ""test_case"": {
              ""type"": ""keyword""
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
