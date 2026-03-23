using System;

namespace UrlShortener.Models
{
    public class Link
    {
        public string Stub { get; set; }

        public string TargetUri { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
