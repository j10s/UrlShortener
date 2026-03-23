using System.Diagnostics.CodeAnalysis;

namespace UrlShortener.DataAccess;

[ExcludeFromCodeCoverage(Justification = "Not used in production")]
internal class Program
{
    internal static void Main()
    {
        // Workaround so we can store migrations in this project
        // See: https://docs.microsoft.com/en-gb/ef/core/miscellaneous/cli/dotnet#other-target-frameworks
    }
}