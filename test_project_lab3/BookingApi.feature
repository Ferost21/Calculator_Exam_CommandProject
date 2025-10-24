Feature: Booking API CRUD
  As an API tester
  I want to test CRUD operations for the restful-booker API
  So that I verify create/read/update/delete behavior

  Scenario: Create, read, update and delete a booking
    Given I have a valid booking payload
    When I create a new booking
    Then the booking is created successfully

    When I get the created booking
    Then the booking data matches the payload

    When I update the booking
    Then the booking is updated successfully

    When I delete the booking
    Then the booking is deleted successfully