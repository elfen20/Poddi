using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace Cave.Poddi
{
    [DebuggerDisplay("{Title}")]
    public class PoddiEpisode
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; }

        public DateTime Date { get; private set; }

        public string Summary { get; private set; }

        public object Duration { get; private set; }

        public long Size { get; private set; }

        public string MediaType { get; private set; }

        public Uri MediaUrl { get; private set; }

        public static PoddiEpisode Load(SyndicationItem item)
        {
            var duration = item.ElementExtensions.Where(e => e.OuterName == "duration").FirstOrDefault()?.GetObject<XElement>();
            var link = item.Links.FirstOrDefault(l => l.MediaType?.StartsWith("audio", StringComparison.InvariantCultureIgnoreCase) ?? false);
            return new PoddiEpisode()
            {
                Id = new Guid(item.Id),
                Title = item.Title.Text,
                Date = item.PublishDate.UtcDateTime.ToLocalTime(),
                Summary = item.Summary.Text,
                Duration = TimeSpanParse(duration?.Value),
                Size = link?.Length ?? -1,
                MediaType = link?.MediaType,
                MediaUrl = link?.Uri,
            };
        }

        static TimeSpan? TimeSpanParse(string value)
        {
            if (value == null) return null;
            var parts = value.Split(':');
            switch (parts.Length)
            {
                case 1: return TimeSpan.FromSeconds(int.Parse(parts[0]));
                case 2: return new TimeSpan(0, int.Parse(parts[0]), int.Parse(parts[1]));
                case 3: return new TimeSpan(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[1]));
                default: throw new FormatException();
            }
        }
    }
}
