using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ResolveThirdPartyReferenceLinks.Providers
{
    public class FormattedUrlProvider : UrlProviderBase
    {
        public class UrlFormatterAction
        {
            [XmlAttribute("format")]
            public string Format { get; set; }
        }

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

        [XmlElement("urlFormatter")]
        public UrlFormatterAction UrlFormatter { get; set; }

        public override Uri CreateUrl(string target)
        {
            if (UrlFormatter?.Format is string urlFormat)
            {
                // generate title
                string formattedTarget = target;
                foreach (var step in TargetFormatter?.Steps ?? new Collection<UrlProviderTargetFormatter.TargetFormatterStep>())
                    formattedTarget = step.Apply(formattedTarget);

                // generate url
                string url = urlFormat.Replace("{target}", formattedTarget);
                foreach (UrlProviderParameter param in Parameters ?? new Collection<UrlProviderBase.UrlProviderParameter>())
                    url = url.Replace($"{{{param.Name}}}", param.Value);

                return new Uri(url);
            }

            return new Uri(string.Empty);
        }
    }
}