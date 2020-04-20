Feature: Calculator

I'd like doc strings and tables to be included in the step arguments.

    Scenario: doc strings matter
       Given The text of "This" and a number of 123 and the doc string
        """
        is a test
        """
        When The words are all listed in order
        Then They match this table
         | This |
         | is   |
         | a    |
         | test |
         | 123  |
