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

        public class UrlProviderTargetFormatter
        {
            public abstract class TargetFormatterStep
            {
                public abstract string Apply(string target);
            }

            public class TargetFormatterReplaceStep : TargetFormatterStep
            {
                [XmlAttribute("pattern")]
                public string Pattern { get; set; }

                [XmlAttribute("with")]
                public string Replacement { get; set; }

                public override string Apply(string target)
                {
                    if (Pattern is string pattern)
                        return new Regex(pattern).Replace(target, Replacement ?? string.Empty);
                    return target;
                }
            }

            [XmlArray("steps")]
            [XmlArrayItem("replace", typeof(TargetFormatterReplaceStep))]
            public Collection<TargetFormatterStep> Steps { get; set; }
        }

        [XmlElement("targetFormatter")]
        public UrlProviderTargetFormatter TargetFormatter { get; set; }

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