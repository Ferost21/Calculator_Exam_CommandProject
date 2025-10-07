@web
Feature: Add to Cart Functionality

  As a standard user
  I want to be able to add a product to the cart
  So that I can purchase the item

  Scenario: Successful addition of an item to the cart
    Given the user is on the Sauce Demo login page
    When the user logs in with username "standard_user" and password "secret_sauce"
    And the user adds the first item to the cart
    Then the shopping cart badge should show "1" item