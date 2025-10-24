Feature: Add item to cart manually
  Scenario: User manually logs in and adds a product to the cart
    Given the user manually logs in to the Sauce Demo site
    When the user adds the first item to the cart
    Then the shopping cart badge should show "1" item and open the cart
