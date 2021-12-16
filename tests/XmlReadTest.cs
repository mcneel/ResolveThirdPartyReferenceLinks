using System;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;

using Xunit;

namespace ResolveThirdPartyReferenceLinks.Tests
{
    public class XmlReadTest
    {
        [Fact]
        public void ConfigReadTest()
        {
            using var cfgStream = Assembly.GetExecutingAssembly()
                                          .GetManifestResourceStream("ResolveThirdPartyReferenceLinks.Tests.TestConfig.xml");
            new ResolveThirdPartyReferenceLinksComponent
            .Factory()
            .Create()
            .Initialize(
                new XPathDocument(cfgStream)
                .CreateNavigator()?
                .SelectSingleNode(
                    XPathExpression.Compile("//component")
                )
            );
        }

        [Fact]
        public void UrlResolveTest()
        {
            var comp = new ResolveThirdPartyReferenceLinksComponent
                        .Factory()
                        .Create();

            using var cfgStream = Assembly.GetExecutingAssembly()
                                          .GetManifestResourceStream("ResolveThirdPartyReferenceLinks.Tests.TestConfig.xml");

            comp.Initialize(
                new XPathDocument(cfgStream)
                .CreateNavigator()?
                .SelectSingleNode(
                    XPathExpression.Compile("//component")
                )
            );

            using var testStream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("ResolveThirdPartyReferenceLinks.Tests.ResolveTest.xml");

            var doc = new XmlDocument();
            doc.Load(testStream);

            comp.Apply(doc, "T:Rhino.Geometry.Brep");
            comp.Apply(doc, "T:Autodesk.Revit.DB.Face");

            Console.WriteLine(doc.OuterXml);
        }
    }
}