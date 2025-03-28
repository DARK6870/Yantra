namespace Yantra.ServiceLevelTests.Shared.Constants.GraphQl;

public static class AuthenticationGraphQlConstants
{
    public const string LoginMutation =
        """
        mutation login($request: LoginCommandInput!) {
          login(request: $request) {
            accessToken
            refreshToken
          }
        }
        """;

    public const string SetPasswordMutation =
        """
        mutation setPassword($request: SetPasswordCommandInput!) {
          setPassword(request: $request)
        }
        """;

    public const string ChangePasswordMutation =
        """
        mutation changePassword($request: ChangePasswordCommandInput!){
          changePassword(request: $request)
        }
        """;

    public const string RefreshAccessTokenMutation =
        """
        query refreshAccessToken {
          refreshAccessToken
        }
        """;

    public const string LogoutMutation =
        """
        mutation logout{
          logout
        }
        """;
}