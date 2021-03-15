# DCdays
**Calculate Working days and business days between two given dates:**

**How I implemented:**
1)	Followed industry standard coding standards and practices. Proper naming conventions,detailed comments both inline and function/method level comments are provided.
2)	Design emphasises on SOLID based principles. This can observed in the implementation of IHoliday interface. General holidays and dynamic holidays have separate behaviour and rules. Leveraged Dependency injection for using the behaviour between objects.
3)	Built two rule engines ApplyHolidayRules and ApplyDynamicHolidayRules which are reusable.
4)	Implemented proper exception handling at all levels and details can be captured in the other layers easily via throw.Innerexceptoin
5)	Code has good test coverage. Used XUnit unit testing framework and covered key functionalities and business rules.
6)	Implemented functional programming. Leveraged several reusable functions for applying the rules.
7)	Passed all the test cases provided in the requirement document and added few based on some custom conditions. Like the dynamic holiday falls on a Sunday and gets moved to Monday and that day is again a holiday.
	
**Design:**
Interfaces and classes used:
IHoliday – properties for date and description, method to set date.
Holiday – Implements Holiday.
DynamicHoliday – Implements Holiday and has additional behaviours like FindDynamicHoliday, CreateDynamicHolidaysBetweenyears.
DayCounter – has the core logic for date calcluations and rules.
XUnitTest Project covers all the key features and rules in the project.

**Technologies:**
C#, .NET Core 3.1, XUnit, Console based app, Visual Studio Community 2019

**Steps to setup:**
           Clone or download the project to your local, build and run.

