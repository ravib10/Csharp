Feature: 22User API

  Scenario: 22Get user by ID
    Given I have user ID 1
    When I request the user details
    Then the user name should be "Leanne Graham1"



Scenario: 122Get user by ID
    Given I have user ID 2
    When I request the user details
    Then the user name should be "Leanne Graham2"


	Scenario: 222Get user by ID
    Given I have user ID 3
    When I request the user details
    Then the user name should be "Leanne Graham3"