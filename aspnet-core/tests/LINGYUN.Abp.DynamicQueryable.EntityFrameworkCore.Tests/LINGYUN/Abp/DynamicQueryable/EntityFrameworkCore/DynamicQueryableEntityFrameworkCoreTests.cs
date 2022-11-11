using LINGYUN.Abp.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using ExecDynamicQueryable = LINGYUN.Linq.Dynamic.Queryable.DynamicQueryable;

namespace LINGYUN.Abp.DynamicQueryable.EntityFrameworkCore;

public class DynamicQueryableEntityFrameworkCoreTests : AbpDynamicQueryableEntityFrameworkCoreTestBase
{
    [Fact]
    public void Should_Null()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Null,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Not_Null()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotNull,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Equal()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Equal,
            Field = nameof(EfCoreTestEntity.PropInt32),
            Logic = DynamicLogic.And,
            Value = 2048
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Equal()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotEqual,
            Field = nameof(EfCoreTestEntity.PropInt32),
            Logic = DynamicLogic.And,
            Value = null
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Less_Than()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.LessThan,
            Field = nameof(EfCoreTestEntity.PropInt32),
            Logic = DynamicLogic.And,
            Value = 2048
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Less_Than_Or_Equal()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.LessThanOrEqual,
            Field = nameof(EfCoreTestEntity.PropInt32),
            Logic = DynamicLogic.And,
            Value = 2048
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Greater_Than()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.GreaterThan,
            Field = nameof(EfCoreTestEntity.PropInt64),
            Logic = DynamicLogic.And,
            Value = 1024L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Greater_Than_Or_Equal()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.GreaterThanOrEqual,
            Field = nameof(EfCoreTestEntity.PropInt64),
            Logic = DynamicLogic.And,
            Value = 1024L
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void Should_Starts_With()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.StartsWith,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "1"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Starts_With()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotStartsWith,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "1"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void Should_Ends_With()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.EndsWith,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "1"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Ends_With()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotEndsWith,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "1"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void Should_Contains()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.Contains,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "22"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Should_Not_Contains()
    {
        using var dbContext = GetRequiredService<EfCoreTestDbContext>();
        Expression<Func<EfCoreTestEntity, bool>> exp = (_) => true;

        var dynamicQueryable = new ExecDynamicQueryable();
        dynamicQueryable.Paramters.Add(new DynamicParamter
        {
            Comparison = DynamicComparison.NotContains,
            Field = nameof(EfCoreTestEntity.PropString),
            Logic = DynamicLogic.And,
            Value = "23"
        });

        exp = exp.DynamicQuery(dynamicQueryable);

        var result = dbContext.TestEntities.Local.Where(exp.Compile()).ToList();
        result.Count.ShouldBe(3);
    }
}
