using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Java;
using Panosen.ElasticSearch.Java.Engine.Engine;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class DynamicTemplateTest
    {
        [Index]
        [DynamicTemplate("zero", Type = typeof(int),
            MatchMappingType = MatchMappingType.Long,
            NameMatch = "b", NameUnmatch = "c", NameMatchPattern = "d",
            PathMatch = "e", PathUnmatch = "f")]
        [DynamicTemplate("one", Type = typeof(int), MatchMappingType = MatchMappingType.Long)]
        [DynamicTemplate("two", Type = typeof(long), NameMatch = "zhangsan")]
        [DynamicTemplate("three", Type = typeof(long), MatchMappingType = MatchMappingType.String)]
        [DynamicTemplate("four", Type = typeof(int), MatchMappingType = MatchMappingType.String, Index = Index.False)]
        [DynamicTemplate("five", Index = Index.False, PathMatch = "*_part_set.*")]
        [DynamicTemplate("six", Type = typeof(Keyword), Index = Index.False)]
        public class Book
        {
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
}
";
        }

        private static string PrepareMappings()
        {
            return @"{
  ""mappings"": {
    ""_doc"": {
      ""dynamic_templates"": [
        {
          ""zero"": {
            ""match_mapping_type"": ""long"",
            ""match"": ""b"",
            ""unmatch"": ""c"",
            ""match_pattern"": ""d"",
            ""path_match"": ""e"",
            ""path_unmatch"": ""f"",
            ""mapping"": {
              ""type"": ""integer""
            }
          }
        },
        {
          ""one"": {
            ""match_mapping_type"": ""long"",
            ""mapping"": {
              ""type"": ""integer""
            }
          }
        },
        {
          ""two"": {
            ""match"": ""zhangsan"",
            ""mapping"": {
              ""type"": ""long""
            }
          }
        },
        {
          ""three"": {
            ""match_mapping_type"": ""string"",
            ""mapping"": {
              ""type"": ""long""
            }
          }
        },
        {
          ""four"": {
            ""match_mapping_type"": ""string"",
            ""mapping"": {
              ""index"": false,
              ""type"": ""integer""
            }
          }
        },
        {
          ""five"": {
            ""path_match"": ""*_part_set.*"",
            ""mapping"": {
              ""index"": false
            }
          }
        },
        {
          ""six"": {
            ""mapping"": {
              ""index"": false,
              ""type"": ""keyword""
            }
          }
        }
      ]
    }
  }
}
";
        }
    }
}
