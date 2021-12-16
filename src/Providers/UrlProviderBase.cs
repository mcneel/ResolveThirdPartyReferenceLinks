using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ResolveThirdPartyReferenceLinks.Providers
{
    public abstract class UrlProviderBase
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public class UrlProviderTargetMatcher
        {
            [XmlAttribute("pattern")]
            public string Pattern { get; set; }
        }

        [XmlElement("targetMatcher")]
        public UrlProviderTargetMatcher TargetMatcher { get; set; }

        public class UrlProviderParameter
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("default")]
            public string DefaultValue { get; set; }

            public string Value { get; set; }
        }

        [XmlArray("parameters")]
        [XmlArrayItem("parameter", typeof(UrlProviderParameter))]
        public Collection<UrlProviderParameter> Parameters { get; set; }

        public virtual bool IsMatch(string target)
        {
            if (TargetMatcher?.Pattern is string pattern)
                return new Regex(pattern).IsMatch(target);
            return false;
        }

        public abstract Uri CreateUrl(string target);
    }
}