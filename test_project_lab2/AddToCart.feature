Feature: Add to cart
  As a standard user
  I want to add a product to the cart
  So that I can buy it

Scenario: Standard user adds product to cart
  Given I am on the SauceDemo login page
  When I login as "standard_user" with password "secret_sauce"
  And I add the "Sauce Labs Backpack" to the cart
  Then the cart badge should show "1"