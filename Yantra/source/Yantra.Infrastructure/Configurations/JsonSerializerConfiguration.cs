using System.Text.Json;

namespace Yantra.Infrastructure.Configurations;

public static class JsonSerializerConfiguration
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true,
        WriteIndented = true
    };
}