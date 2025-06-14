import { gql } from '@apollo/client';

const GET_MENU_ITEMS = gql`
query getMenuItems {
  menuItems {
    dateUpdated
    description
    id
    image
    name
    price
    type
  }
}
`

export { GET_MENU_ITEMS };
