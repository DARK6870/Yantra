namespace Yantra.ServiceLevelTests.Shared.Responses;

public record OffsetPaginatedResponse<T>(int TotalCount, List<T> Items);