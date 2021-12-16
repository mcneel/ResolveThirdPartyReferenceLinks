using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ResolveThirdPartyReferenceLinks.Providers
{
    public class FormattedUrlProvider : UrlProviderBase
    {
        public class UrlFormatterAction
        {
            [XmlAttribute("format")]
            public string Format { get; set; }
        }

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