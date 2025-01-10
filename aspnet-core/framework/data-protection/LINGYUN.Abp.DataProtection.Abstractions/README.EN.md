# LINGYUN.Abp.DataProtection.Abstractions

Data protection abstraction module, providing interface definitions and basic types for data protection.

## Features

* `IDataProtected` - Data protection interface, marking entities that need data protection control
* `DataProtectedAttribute` - Data protection attribute, marking methods or classes that need data protection control
* `DisableDataProtectedAttribute` - Disable data protection attribute, marking methods or classes that don't need data protection control

## Data Operation Types

* `DataAccessOperation.Read` - Query operation
* `DataAccessOperation.Write` - Update operation
* `DataAccessOperation.Delete` - Delete operation

## Data Filtering

* `DataAccessFilterLogic` - Data filter logic
  * `And` - Logical AND
  * `Or` - Logical OR
* `DataAccessFilterRule` - Data filter rule
  * `Field` - Field name
  * `Value` - Field value
  * `Operate` - Operator
  * `IsLeft` - Is left parenthesis

## Related Links

* [中文文档](./README.md)
