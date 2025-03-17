namespace Yantra.ServiceLevelTests.Shared.Constants.GraphQl;

public static class UsersGraphQlConstants
{
    public const string GetUsersQuery =
        """
        query getUsers {
          users {
            id
            userName
            email
            lastName
            firstName
            role
            dateCreated
            dateUpdated
          }
        }
        """;

    public const string GetUserByIdQuery =
      """
      query getUserById($id: String!){
        userById(id: $id){
          id
          userName
          email
          lastName
          firstName
          role
          dateCreated
          dateUpdated
        }
      }
      """;

    public const string CreateUserMutation =
      """
      mutation createUser($request: CreateUserCommandInput!) {
        createUser(request: $request)
      }
      """;

    public const string UpdateUserMutation =
      """
      mutation updateUser($request: UpdateUserCommandInput!){
        updateUser(request: $request)
      }
      """;

    public const string DeleteUserMutation =
      """
      mutation deleteUser($id: String!){
        deleteUser(id: $id)
      }
      """;
}