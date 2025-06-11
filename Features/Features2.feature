Feature: Feature 2

 


	Scenario: 2wewe22Get user by ID
    Given I have user ID 3
    When I request the user details
    Then the user name should be "Leanne Graham1"