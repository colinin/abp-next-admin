using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Elasticsearch;

public class TestDocument
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public TestEnum Status { get; set; }
    public TestEnum StringValueStatus { get; set; }
    public TestEnum? NullableStatus { get; set; }
    public List<SubDocument>? Items { get; set; }
    public List<string>? Tags { get; set; }
    public Address? Address { get; set; }
    public string? Exceptions { get; set; }
}

public class SubDocument
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
}

public class Address
{
    public string? City { get; set; }
    public string? Street { get; set; }
}

public enum TestEnum
{
    Active = 1,
    Inactive = 2,
    Pending = 3
}
