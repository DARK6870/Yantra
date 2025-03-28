namespace Yantra.Application.Constants;

public static class HttpConstants
{
    public static string SetPasswordPath(string token) => $"/set-password/{token}";
    
    public static string OrderDetailsPath(string id) => $"/order/{id}";
}