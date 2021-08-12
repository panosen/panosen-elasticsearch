using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panosen.ElasticSearch.Mapping;
using Panosen.ElasticSearch.Mapping.Engine;
using Panosen.Reflection;

namespace Panosen.ElasticSearch.MSTest
{
    [TestClass]
    public class IndexMappingsTest
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

        }

        [TestMethod]
        public void TestMethod1()
        {
            var type = typeof(Book);

            var mappings = new Mappings();
            mappings.Type = type;

            var actual = mappings.TransformText();

            var expected = PrepareExpected();

            Assert.AreEqual(expected, actual);
        }

        private string PrepareExpected()
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
      ""properties"": {}
    }
  }
}
";
        }
    }
}
