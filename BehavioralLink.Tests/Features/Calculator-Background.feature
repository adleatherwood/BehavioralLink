Feature: Calculator

As a math idiot, in order to avoid silly mistakes, I want to be told the sum of two numbers

    Background:
       Given I have entered '50' into the calculator

    Scenario: Add two numbers
         And I have also entered 70 into the calculator
        When I press add
        Then the result should be 120 on the screen
