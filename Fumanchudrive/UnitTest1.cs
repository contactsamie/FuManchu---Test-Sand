using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FuManchu;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fumanchudrive
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            LoadHelpers();
        }
        [TestMethod]
        public void TestMethod1()
        {
            Handlebars.Compile("<template-name>", "Hello {{world}}!");
            string result = Handlebars.Run("<template-name>", new { world = "you" });
            Assert.AreEqual("Hello you!", result);
        }
        [TestMethod]
        public void TestMethod2()
        {
            var template = @"{{#if world}}True{{/if}}{{#if world}}True{{else}}False{{/if}}{{#if maybe}}Value 1{{#elseif world}}Value 2{{else}}None{{/if}}";
            Handlebars.Compile("<template-name>1", template);
            string result = Handlebars.Run("<template-name>1", new { world = "you" });
            Assert.AreEqual("TrueTrueValue 2", result);
        }
        [TestMethod]
        public void TestMethod3()
        {
            var template = @"{{#equals world ""you""}}True{{/equals}}";
            Handlebars.Compile("<template-name>2", template);
            string result = Handlebars.Run("<template-name>2", new { world = "you" });
            Assert.AreEqual("True", result);
        }
        [TestMethod]
        public void TestMethod4()
        {

            var template = @"{{#equals world ""you""}}{{#if maybe}}Value 1{{#elseif world}}Value 2{{else}}None{{/if}}{{/equals}}";
            Handlebars.Compile("<template-name>3", template);
            string result = Handlebars.Run("<template-name>3", new { world = "you" });
            Assert.AreEqual("Value 2", result);
        }
        [TestMethod]
        public void TestMethod5()
        {

            var template = @"{{#equals world.Today.What ""you""}}{{#if maybe}}Value 1{{#elseif world.Today.What}}Value 2{{else}}None{{/if}}{{/equals}}";
            Handlebars.Compile("<template-name>4", template);
            string result = Handlebars.Run("<template-name>4", new { world = new { Today=new { What= "you" } } });
            Assert.AreEqual("Value 2", result);
        }
        [TestMethod]
        public void TestMethod6()
        {

            var template = @"{{#equals world.Today.What ""you""}}{{#if maybe}}Value 1{{#elseif world.Today.What}}Value 2{{#equals world.Today.What ""you""}}{{#if maybe}}Value 1{{#elseif world.Today.What}}Value 2{{else}}None{{/if}}{{/equals}}{{else}}None{{/if}}{{/equals}}";
            Handlebars.Compile("<template-name>5", template);
            string result = Handlebars.Run("<template-name>5", new { world = new { Today = new { What = "you" } } });
            Assert.AreEqual("Value 2Value 2", result);
        }

        [TestMethod]
        public void TestMethod7()
        {
           
            string template = "<ul>{{#each this}}<li>{{this}}</li>{{/each}}</ul>";

            // result: <ul><li>Matt</li><li>Stuart</li></ul>
            string result = Handlebars.CompileAndRun("name", template, new[] { 1,2,3,4,5 });
        }
        public virtual void LoadHelpers()
        {


            Handlebars.RegisterHelper("multiply", (options) =>
            {
                var result = 0;
                try
                {
                    result = Convert.ToInt32(options.Arguments[0]) * Convert.ToInt32(options.Arguments[1]);
                }
                catch (Exception)
                {
                }
                return result.ToString();
            });

            Handlebars.RegisterHelper("equals", (options) =>
            {
                var areEqual = false;
                try
                {
                    areEqual = options.Arguments[0].ToString() == options.Arguments[1].ToString();
                }
                catch (Exception)
                {
                }

                if (areEqual)
                {
                    var enumerable = options.Data as IEnumerable ?? new[] { (object)options.Data };
                    var result = options.Fn(options.RenderContext.TemplateData.Model);
                    return result;
                }
                else
                {
                    return "";
                }
            });
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }
}
