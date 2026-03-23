using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DataAccess;

public class Link
{
    public long Id { get; set; }

    [Required]
    public string TargetUri { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}