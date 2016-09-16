# Proposal description

## Scope
 * Provide an alternative to Unit of Work with Repository Pattern usage

## Why
 * **Unit of Work with Repository Pattern** requires the **Unit of Work** to handle the instantiation of all repositories or say the least attach the shared **DbContext** to all of them known as [Property Injection](https://jeremybytes.blogspot.ro/2014/01/dependency-injection-property-injection.html)
 * Each usage of **Unit of Work** ends up beginning with the creation of one and saving the changes when finished which leads to code duplication. I reference [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself)
 * This never lets you see clearly the dependencies which your class has directly from .ctor
 * Usage of each repository usage is inferred from the **Unit of Work** which acts more like a Service Locator with specific service resolving properties. I reference [Service Locator anti-pattern](http://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/)

## How
  * I propose removing the Unit of Work and replacing it with an **Ambient Transaction** by using the [TransactionScope](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope(v=vs.110).aspx)
  * I am going to provide metadata regarding which service method requires a transaction using [Attributes](https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx)
  * I am going to use the [Decorator pattern](https://en.wikipedia.org/wiki/Decorator_pattern) to wrap the service calls inside a [TransactionScope](https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope(v=vs.110).aspx) using information provided by [Attributes](https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx)
  * I will be able to provider the generation of the **Decorator** dynamically inside the [Composition Root](http://blog.ploeh.dk/2011/07/28/CompositionRoot/) at [Runtime](https://en.wikipedia.org/wiki/Run_time_(program_lifecycle_phase))

## Implications
  * Queries will become transactional
  * Transactional queries might run into deadlocks(this may be avoided with proper care)
  * This looks more like how WCF handles transactions but these are inProc ones

## Pending investigations
  * Looking at the actual queries that get run against the database using the SQL Profiler
  
  

 
