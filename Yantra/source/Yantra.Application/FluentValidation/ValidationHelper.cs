using MongoDB.Bson;

namespace Yantra.Application.FluentValidation;

public static class ValidationHelper
{
    public static bool IsValidObjectId(string arg)
    {
        return ObjectId.TryParse(arg, out _);
    }
}