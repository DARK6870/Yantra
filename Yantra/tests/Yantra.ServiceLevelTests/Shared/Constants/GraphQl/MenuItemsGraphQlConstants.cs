namespace Yantra.ServiceLevelTests.Shared.Constants.GraphQl;

public static class MenuItemsGraphQlConstants
{
    public const string GetMenuItemsQuery =
        """
        query getMenuItems {
          menuItems {
            id
            name
            description
            image
            type
            price
            dateUpdated
          }
        }
        """;

    public const string GetMenuItemByIdQuery =
        """
        query getMenuItemById($id: String!) {
          menuItemById(id: $id) {
            id
            name
            description
            image
            type
            price
            dateUpdated
          }
        }
        """;
    
    public const string CreateMenuItemMutation =
        """
        mutation createMenuItem($request: CreateMenuItemCommandInput!) {
          createMenuItem(request: $request)
        }
        """;

    public const string UpdateMenuItemMutation =
        """
        mutation updateMenuItem($request: UpdateMenuItemCommandInput!) {
          updateMenuItem(request: $request)
        }
        """;

    public const string DeleteMenuItemMutation =
        """
        mutation deleteMenuItem($id: String!){
          deleteMenuItem(id: $id)
        }
        """;
}