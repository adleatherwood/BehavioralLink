Feature: Calculator

As a math idiot, in order to avoid silly mistakes, I want to be told the sum of two numbers

    @mytag
    Scenario: Add two numbers
       Given I have entered '50' into the calculator
         And I have also entered '70' into the calculator
        When I press add
        Then the result should be '120' on the screen

    @mytag
    Scenario: Add two more numbers
       Given I have entered '10' into the calculator
         And I have also entered '30' into the calculator
        When I press add
        Then the result should be '40' on the screen
