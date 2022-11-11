using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Linq.Dynamic.Queryable;

public class LinqTestClass
{
    public string StringNull { get; set; }
    public string StringRequired { get; set; }
    public long? Int64Null { get; set; }
    public long Int64Required { get; set; }
    public DateTime? DateTimeNull { get; set; }
    public DateTime DateTimeRequired { get; set; }
    public DateOnly? DateOnlyNull { get; set; }
    public DateOnly DateOnlyRequired { get; set; }
    public TimeOnly? TimeOnlyNull { get; set; }
    public TimeOnly TimeOnlyRequired { get; set; }
}

public class DynamicQueryableTests : DynamicQueryableTestBase
{
    private readonly static List<LinqTestClass> _testClasses;
    static DynamicQueryableTests()
    {
        _testClasses = new List<LinqTestClass>
        {
            new LinqTestClass
            {
                StringNull = null,
                StringRequired = "3211",
                DateOnlyNull = new DateOnly(2022, 1, 1),
                DateOnlyRequired = new DateOnly(2022, 10, 1),
                TimeOnlyNull = null,
                TimeOnlyRequired = new TimeOnly(12, 0, 0),
                DateTimeNull = new DateTime(2021, 1, 1, 0, 0, 0),
                DateTimeRequired = new DateTime(2022, 10, 1, 12, 0, 0),
                Int64Null = null,
                Int64Required = 1024L
            },
            new LinqTestClass
            {
                StringNull = "not null",
                StringRequired = "1123",
                DateOnlyNull = null,
                DateOnlyRequired = new DateOnly(2021, 10, 1),
                TimeOnlyNull = new TimeOnly(0, 0, 0),
                TimeOnlyRequired = new TimeOnly(1, 0, 0),
                DateTimeNull = null,
                DateTimeRequired = new DateTime(2021, 1, 1, 0, 0, 0),
                Int64Null = null,
                Int64Required = 2048L
            },
        };
    }

    [Fact]
    public void Should_Null()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Null,
            Field = nameof(LinqTestClass.Int64Null),
            Logic = DynamicLogic.And
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Not_Null()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotNull,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Equal()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Equal,
            Field = nameof(LinqTestClass.StringRequired),
            Logic = DynamicLogic.And,
            Value = "1123"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Equal()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotEqual,
            Field = nameof(LinqTestClass.StringRequired),
            Logic = DynamicLogic.And,
            Value = "1123"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Less_Than()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.LessThan,
            Field = nameof(LinqTestClass.Int64Required),
            Logic = DynamicLogic.And,
            Value = 2048L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Less_Than_Or_Equal()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.LessThanOrEqual,
            Field = nameof(LinqTestClass.Int64Required),
            Logic = DynamicLogic.And,
            Value = 2048L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Greater_Than()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.GreaterThan,
            Field = nameof(LinqTestClass.Int64Null),
            Logic = DynamicLogic.And,
            Value = 1024L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void Should_Greater_Than_Or_Equal()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.GreaterThanOrEqual,
            Field = nameof(LinqTestClass.Int64Required),
            Logic = DynamicLogic.And,
            Value = 1024L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Starts_With()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.StartsWith,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "not"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Starts_With()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotStartsWith,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "not"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Ends_With()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.EndsWith,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "null"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Ends_With()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotEndsWith,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "null"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Contains()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Contains,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "null"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Contains()
    {
        Expression<Func<LinqTestClass, bool>> exp = (_) => true;

        var dynamicQueryable = new DynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotContains,
            Field = nameof(LinqTestClass.StringNull),
            Logic = DynamicLogic.And,
            Value = "test"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = _testClasses.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }
}
