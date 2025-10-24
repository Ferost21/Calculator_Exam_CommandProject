Feature: Public APIs - category entries
  As an API tester
  I want to request public API entries by category
  So that I verify responses from a public API

  Scenario: Get entries for category "Animals"
    When I request entries for category "Animals"
    Then response status should be 200
    And response should contain at least one entry
    And each entry should contain fields "API" and "Link"