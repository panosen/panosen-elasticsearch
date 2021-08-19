using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;
using System;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class EsFieldsEngineTest1
    {
        [Index]
        public class Book
        {
            /// <summary>
            /// ÍÆ¶Ï int
            /// </summary>
            public int AssumeInt { get; set; }

            /// <summary>
            /// ÍÆ¶Ï long
            /// </summary>
            public long AssumeLong { get; set; }

            /// <summary>
            /// ÍÆ¶Ï text
            /// </summary>
            public string AssumeText { get; set; }

            /// <summary>
            /// Assume Date
            /// </summary>
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            public Keyword Name { get; set; }
        }

        [TestMethod]
        public void TestMethod()
        {
            var type = typeof(Book);

            var classNode = ClassLoader.LoadClass(type);

            {
                DocFields docFields = new DocFields();
                docFields.ClassNode = classNode;
                docFields.JavaRoot = "Sample";
                docFields.RootNamespace = type.ReflectedType.FullName;

                var actual = docFields.TransformText();

                var expected = PrepareFields();

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

        private static string PrepareFields()
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
     * AssumeInt
     */
    public final static String ASSUME_INT = ""assume_int"";

    /**
     * AssumeLong
     */
    public final static String ASSUME_LONG = ""assume_long"";

    /**
     * AssumeText
     */
    public final static String ASSUME_TEXT = ""assume_text"";

    /**
     * AssumeText.Keyword
     */
    public final static String ASSUME_TEXT_KEYWORD = ""assume_text.keyword"";

    /**
     * Timestamp
     */
    public final static String TIMESTAMP = ""timestamp"";

    /**
     * Name
     */
    public final static String NAME = ""name"";
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

    @SerializedName(""assume_int"")
    private Integer assumeInt;

    @SerializedName(""assume_long"")
    private Long assumeLong;

    @SerializedName(""assume_text"")
    private String assumeText;

    @SerializedName(""timestamp"")
    private java.util.Date timestamp;

    @SerializedName(""name"")
    private String name;

    /**
     * Get 
     */
    public Integer getAssumeInt() {
        return assumeInt;
    }

    /**
     * Set 
     */
    public void setAssumeInt(Integer assumeInt) {
        this.assumeInt = assumeInt;
    }

    /**
     * Get 
     */
    public Long getAssumeLong() {
        return assumeLong;
    }

    /**
     * Set 
     */
    public void setAssumeLong(Long assumeLong) {
        this.assumeLong = assumeLong;
    }

    /**
     * Get 
     */
    public String getAssumeText() {
        return assumeText;
    }

    /**
     * Set 
     */
    public void setAssumeText(String assumeText) {
        this.assumeText = assumeText;
    }

    /**
     * Get 
     */
    public java.util.Date getTimestamp() {
        return timestamp;
    }

    /**
     * Set 
     */
    public void setTimestamp(java.util.Date timestamp) {
        this.timestamp = timestamp;
    }

    /**
     * Get 
     */
    public String getName() {
        return name;
    }

    /**
     * Set 
     */
    public void setName(String name) {
        this.name = name;
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
        ""assume_int"": {
          ""type"": ""integer""
        },
        ""assume_long"": {
          ""type"": ""long""
        },
        ""assume_text"": {
          ""type"": ""text"",
          ""fields"": {
            ""keyword"": {
              ""type"": ""keyword"",
              ""ignore_above"": 256
            }
          }
        },
        ""name"": {
          ""type"": ""keyword""
        },
        ""timestamp"": {
          ""type"": ""date""
        }
      }
    }
  }
}
";
        }
    }
}
