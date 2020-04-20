Feature: Calculator

As a math idiot, in order to avoid silly mistakes, I want to be told the sum of two numbers

    @mytag
    Scenario Outline: Add two numbers
       Given I have entered <NumA> into the calculator
         And I have also entered <NumB> into the calculator
        When I press add
        Then the result should be <Answer> on the screen

    Examples:
         | NumA | NumB | Answer |
         | 1    | 1    | 2      |
         | 2    | 2    | 4      |
         | 2    | 3    | 5      |

