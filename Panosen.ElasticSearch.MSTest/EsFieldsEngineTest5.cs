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
    public class EsFieldsEngineTest5
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
            [ObjectField]
            public object Brand { get; set; }

            /// <summary>
            /// 推断 long
            /// </summary>
            [ObjectField(Dynamic = Dynamic.Strict)]
            public object Category { get; set; }

            /// <summary>
            /// 推断 long
            /// </summary>
            [ObjectField(Enabled = Enabled.False)]
            public object Tag { get; set; }
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

                var expected = PrepareJava();

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
     * Id
     */
    public final static String ID = ""id"";

    /**
     * Brand
     */
    public final static String BRAND = ""brand"";

    /**
     * Category
     */
    public final static String CATEGORY = ""category"";

    /**
     * Tag
     */
    public final static String TAG = ""tag"";
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

    @SerializedName(""id"")
    private Integer id;

    @SerializedName(""brand"")
    private Object brand;

    @SerializedName(""category"")
    private Object category;

    @SerializedName(""tag"")
    private Object tag;

    /**
     * Get 
     */
    public Integer getId() {
        return id;
    }

    /**
     * Set 
     */
    public void setId(Integer id) {
        this.id = id;
    }

    /**
     * Get 
     */
    public Object getBrand() {
        return brand;
    }

    /**
     * Set 
     */
    public void setBrand(Object brand) {
        this.brand = brand;
    }

    /**
     * Get 
     */
    public Object getCategory() {
        return category;
    }

    /**
     * Set 
     */
    public void setCategory(Object category) {
        this.category = category;
    }

    /**
     * Get 
     */
    public Object getTag() {
        return tag;
    }

    /**
     * Set 
     */
    public void setTag(Object tag) {
        this.tag = tag;
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
        ""brand"": {
          ""type"": ""object""
        },
        ""category"": {
          ""type"": ""object"",
          ""dynamic"": ""strict""
        },
        ""id"": {
          ""type"": ""integer""
        },
        ""tag"": {
          ""type"": ""object"",
          ""enabled"": false
        }
      }
    }
  }
}
";
        }
    }
}
