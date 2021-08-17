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

        private static string PrepareJava()
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
     * Timestamp
     */
    public final static String TIMESTAMP = ""timestamp"";
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
