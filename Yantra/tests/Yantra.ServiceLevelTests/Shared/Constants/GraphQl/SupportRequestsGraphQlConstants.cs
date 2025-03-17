namespace Yantra.ServiceLevelTests.Shared.Constants.GraphQl;

public static class SupportRequestsGraphQlConstants
{
    public const string SubmitSupportRequestCommand =
        """
        mutation submitSupportRequest($request: CreateSupportRequestCommandInput!){
          submitSupportRequest(request: $request)
        }
        """;
}