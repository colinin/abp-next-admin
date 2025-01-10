# LINGYUN.Linq.Dynamic.Queryable

A basic library for dynamic querying, extending Linq to dynamically build expression trees.

## Features

* Support for dynamic query condition building
* Support for various comparison operators: equal, not equal, greater than, less than, greater than or equal, less than or equal, contains, not contains, starts with, not starts with, ends with, not ends with, etc.
* Support for dynamic type conversion and null value handling
* Support for dynamic querying of enum types

## Configuration and Usage

The module can be referenced as needed, providing extensions only for Linq.

### Basic Usage

```csharp
// Create dynamic query parameter
var parameter = new DynamicParamter 
{
    Field = "Name",
    Comparison = DynamicComparison.Equal,
    Value = "test"
};

// Create query condition
var queryable = new DynamicQueryable
{
    Paramters = new List<DynamicParamter> { parameter }
};

// Apply to Expression
Expression<Func<TEntity, bool>> condition = e => true;
condition = condition.DynamicQuery(queryable);

// Query using the condition
var result = await repository.Where(condition).ToListAsync();
```

### Supported Comparison Operators

* `Equal` - Equals
* `NotEqual` - Not equals
* `LessThan` - Less than
* `LessThanOrEqual` - Less than or equal to
* `GreaterThan` - Greater than
* `GreaterThanOrEqual` - Greater than or equal to
* `Contains` - Contains
* `NotContains` - Does not contain
* `StartsWith` - Starts with
* `NotStartsWith` - Does not start with
* `EndsWith` - Ends with
* `NotEndsWith` - Does not end with

## Related Links

* [LINGYUN.Abp.Dynamic.Queryable.Application](../LINGYUN.Abp.Dynamic.Queryable.Application/README.EN.md)
