using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Cave.Poddi
{
    [DebuggerDisplay("{Title}")]
    public class PoddiFeed
    {
        public static PoddiFeed Load(string source)
        {
            var xml = XmlReader.Create(source);
            var feed = SyndicationFeed.Load(xml);
            var result = new PoddiFeed()
            {
                Title = feed.Title.Text,
                Description = feed.Description.Text,
                ImageUrl = feed.ImageUrl,
                Language = new CultureInfo(feed.Language),
            };

            var episodes = new List<PoddiEpisode>();
            foreach (var item in feed.Items)
            {
                episodes.Add(PoddiEpisode.Load(item));
            }
            result.Episodes = episodes.AsReadOnly();
            return result;
        }

        public string Title { get; private set; }
        
        public string Description { get; private set; }

        public Uri ImageUrl { get; private set; }

        public CultureInfo Language { get; private set; }

        public IList<PoddiEpisode> Episodes { get; private set; }
    }
}
