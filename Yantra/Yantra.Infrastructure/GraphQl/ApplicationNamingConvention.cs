using HotChocolate.Types.Descriptors;

namespace Yantra.Infrastructure.GraphQl;

public class ApplicationNamingConvention : DefaultNamingConventions
{
    public override string GetEnumValueName(object value) => value.ToString() ?? string.Empty;
}