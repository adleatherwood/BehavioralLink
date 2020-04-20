# BehavioralLink

Simple framework for executing Gherkin style requirements within the test framework of your choice.

[![pipeline status](https://gitlab.com/adleatherwood/BehavioralLink/badges/master/pipeline.svg)](https://gitlab.com/adleatherwood/BehavioralLink/commits/master)
[![coverage report](https://gitlab.com/adleatherwood/BehavioralLink/badges/master/coverage.svg)](https://gitlab.com/adleatherwood/BehavioralLink/commits/master)
[![NuGet](https://img.shields.io/nuget/v/BehavioralLink.svg?style=flat)](https://www.nuget.org/packages/BehavioralLink/)

## Quick Start

Within the root of the test project, create a feature file:

```gherkin
# Calculator.feature
Feature: This calculator should be able to add things.

@yass
Scenario: Adding two numbers
   Given a first number of 21
     And a second number of 21
    When the add button is pressed
    Then the answer of 42 will be displayed
```

Then create a context object that manages the state and functionality of the test:

```cs
class Context
{
    public int Value1;
    public int Value2;
    public int Answer;

    /* NOTE: Method names are found by convention by default.
     *       Parameters like 21 or "abc" are removed, along
     *       with spaces and other invalid method name characters.
     *       What's left will be the name of the method that the
     *       step will be executed with. 
     */
    public void AFirstNumberOf(int number)
    {
        Value1 = number;
    }

    public void ASecondNumberOf(int number)
    {
        Value2 = number;
    }

    public void TheAddButtonIsPressed()
    {
        Answer = Value1 + Value2;
    }

    public void TheAnswerOfWillBeDisplayed(int value)
    {
        Assert.AreEqual(value, Answer);
    }
}
```

Any number of inline parameters can be passed into the 
step method.  Each parameter is passed in the order it
is found in the step.  An attempt is made to coerce the 
text value into the type of it's associated parameter.

Tables and Doc strings are also supported and are always
the last parameter of the associated method. e.g.

```cs
// Given a number of 12.3 and a string of "abc" and this doc string
// """some big doc string"""

public void ANumberOfAndAStringOfAndThisDocString(decimal number, string text, string doc) { ... }

// Given a setup via a table
// | Id | Name   |
// | 1  | Otto   |
// | 2  | Murray |

public void ASetupViaATable(PickleTable table) { ... }
```

Scenario outlines, Backgrounds, Rules & Examples are also supported.
Finally create a unit test in the framework of your choice.  There's
a variety of ways to organize this.  You can have one unit test to
execute all feature scenarios or a unit test per scenario.  

```cs
[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void AddingTwoNumbers()
    {
        Feature.Load("Calculator.feature", Resolve.InProjectRoot)
            .Where(s => s.IsTagged("@yass"))
            .Where(s => s.IsNamed("Adding two numbers"))
            .Select(s => s.Execute(new Context()))
            .Single();
    }
}
```

Run the test.  As you can see, Features are just an enumerable of Scenarios.  
You can filter (or whatever) these scenarios any way you like and execute them.

There are abstractions available that allow you to define your own method 
resolution/execution conventions and location feature source files from
where ever you like.


[Icon Source](http://www.iconarchive.com/show/macaron-icons-by-goescat/burp-suite-icon.html)
