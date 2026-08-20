using Elastic.Clients.Elasticsearch.QueryDsl;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace LINGYUN.Abp.Elasticsearch.Tests;

public abstract class ExpressionQueryTranslatorTests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IExpressionQueryTranslator _expressionQueryTranslator;

    public ExpressionQueryTranslatorTests()
    {
        _expressionQueryTranslator = GetRequiredService<IExpressionQueryTranslator>();
    }

    #region 基础查询测试

    [Fact]
    public async virtual Task Translate_ConstantTrue_ReturnsMatchAllQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => true);

        // Assert
        query.ShouldNotBeNull();
        query.MatchAll.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_ConstantFalse_ReturnsMatchNoneQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => false);

        // Assert
        query.ShouldNotBeNull();
        query.MatchNone.ShouldNotBeNull();
    }

    #endregion

    #region 相等性查询测试

    [Fact]
    public async virtual Task Translate_Equal_NumericField_ReturnsTermQuery()
    {
        // Act
        var idValue = 100;
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Id == idValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Id");
        query.Term.Value.ShouldBe(idValue);
    }

    [Fact]
    public async virtual Task Translate_Equal_StringField_ReturnsTermQueryWithKeyword()
    {
        // Act
        var stringValue = "test";
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name == stringValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Name.keyword");
        query.Term.Value.ShouldBe(stringValue);
    }

    [Fact]
    public async virtual Task Translate_Equal_BooleanField_ReturnsTermQuery()
    {
        // Act
        var boolValue = true;
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.IsActive == boolValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("IsActive");
        query.Term.Value.ShouldBe(boolValue);
    }

    [Fact]
    public async virtual Task Translate_Equal_EnumField_ReturnsTermQuery()
    {
        // Act
        var enumValue = TestEnum.Active;
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Status == enumValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Status");
        query.Term.Value.ShouldBe((long)enumValue);
    }

    [Fact]
    public async virtual Task Translate_Equal_StringEnumField_ReturnsTermQueryWithString()
    {
        // Act
        var enumValue = TestEnum.Pending;
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.StringValueStatus == enumValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("StringValueStatus.keyword");
        query.Term.Value.ShouldBe(enumValue.ToString());
    }

    [Fact]
    public async virtual Task Translate_NotEqual_StringField_ReturnsBoolMustNot()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name != "test");

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);

        var mustNot = query.Bool.MustNot.First();
        mustNot.ShouldNotBeNull();
        mustNot.Term.ShouldNotBeNull();
        mustNot.Term.Field.ToString().ShouldBe("Name.keyword");
    }

    [Fact]
    public async virtual Task Translate_Equal_NullableEnumField_ReturnsTermQuery()
    {
        // Act
        var enumValue = TestEnum.Inactive;
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.NullableStatus == enumValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("NullableStatus");
        query.Term.Value.ShouldBe((long)enumValue);
    }

    [Fact]
    public async virtual Task Translate_Equal_DecimalField_ReturnsTermQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Salary == 5000.50m);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Salary");
    }

    [Fact]
    public async virtual Task Translate_Equal_NullableDateField_ReturnsDateRangeQuery()
    {
        // Arrange
        var date = new DateTime(2024, 3, 15, 10, 30, 0);

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.UpdatedTime == date);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<DateRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("UpdatedTime");
        rangeQuery.Gte.ShouldNotBeNull();
        rangeQuery.Lte.ShouldNotBeNull();
    }

    #endregion

    #region Null 比较测试

    [Fact]
    public async virtual Task Translate_EqualNull_ReturnsMustNotExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name == null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);

        var mustNot = query.Bool.MustNot.First();
        mustNot.ShouldNotBeNull();
        mustNot.Exists.ShouldNotBeNull();
        mustNot.Exists.Field.ToString().ShouldBe("Name.keyword");
    }

    [Fact]
    public async virtual Task Translate_NotEqualNull_ReturnsExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name != null);

        // Assert
        query.ShouldNotBeNull();
        query.Exists.ShouldNotBeNull();
        query.Exists.Field.ToString().ShouldBe("Name.keyword");
    }

    [Fact]
    public async virtual Task Translate_NullableType_IsNull_ReturnsMustNotExists()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.UpdatedTime == null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);
        query.Bool.MustNot.First().Exists.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_NullableType_IsNotNull_ReturnsExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.UpdatedTime != null);

        // Assert
        query.ShouldNotBeNull();
        query.Exists.ShouldNotBeNull();
        query.Exists.Field.ToString().ShouldBe("UpdatedTime");
    }

    [Fact]
    public async virtual Task Translate_NullableEnum_IsNull_ReturnsMustNotExists()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.NullableStatus == null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);
        query.Bool.MustNot.First().Exists.ShouldNotBeNull();
    }

    #endregion

    #region 逻辑运算测试

    [Fact]
    public async virtual Task Translate_AndAlso_TwoConditions_ReturnsBoolFilterWithTwoQueries()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > 18 && x.IsActive == true);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(2);
    }

    [Fact]
    public async virtual Task Translate_AndAlso_ThreeConditions_ReturnsBoolFilterWithThreeQueries()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > 18 && x.IsActive == true && x.Name != null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(3);
    }

    [Fact]
    public async virtual Task Translate_OrElse_TwoConditions_ReturnsBoolShouldWithTwoQueries()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > 18 || x.IsActive == true);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Should.ShouldNotBeNull();
        query.Bool.Should.Count.ShouldBe(2);
        query.Bool.MinimumShouldMatch.ShouldNotBeNull();
        query.Bool.MinimumShouldMatch.Value1.ShouldBe(1);
    }

    [Fact]
    public async virtual Task Translate_OrElse_ThreeConditions_ReturnsBoolShouldWithThreeQueries()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > 18 || x.IsActive == true || x.Name != null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Should.ShouldNotBeNull();
        query.Bool.Should.Count.ShouldBe(3);
        query.Bool.MinimumShouldMatch.ShouldNotBeNull();
        query.Bool.MinimumShouldMatch.Value1.ShouldBe(1);
    }

    [Fact]
    public async virtual Task Translate_AndAlsoWithOrElse_ReturnsNestedBoolQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => (x.Age > 18 || x.IsActive == true) && x.Name != null);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(2);

        // 检查第一个 filter 是否为 Should 查询
        var shouldQuery = query.Bool.Filter.FirstOrDefault(q => q.Bool?.Should != null);
        shouldQuery.ShouldNotBeNull();
        shouldQuery.Bool.ShouldNotBeNull();
        shouldQuery.Bool.Should.ShouldNotBeNull();
        shouldQuery.Bool.Should.Count.ShouldBe(2);
    }

    [Fact]
    public async virtual Task Translate_Not_ReturnsNegatedBooleanQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => !x.IsActive);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("IsActive");
        query.Term.Value.TryGetBool(out var value).ShouldBeTrue();
        value.Value.ShouldBeFalse();
    }

    [Fact]
    public async virtual Task Translate_Not_OnComparison_ReturnsMustNotQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => !(x.Age > 18));

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);
        query.Bool.MustNot.First().Range.ShouldNotBeNull();
    }

    #endregion

    #region 范围查询测试

    [Fact]
    public async virtual Task Translate_GreaterThan_ReturnsRangeQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > 18);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Age");
        rangeQuery.Gt.ShouldNotBeNull();
        rangeQuery.Gt!.Value.ShouldBe(18);
    }

    [Fact]
    public async virtual Task Translate_GreaterThanOrEqual_ReturnsRangeQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age >= 18);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Age");
        rangeQuery.Gte.ShouldNotBeNull();
        rangeQuery.Gte!.Value.ShouldBe(18);
    }

    [Fact]
    public async virtual Task Translate_LessThan_ReturnsRangeQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age < 65);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Age");
        rangeQuery.Lt.ShouldNotBeNull();
        rangeQuery.Lt!.Value.ShouldBe(65);
    }

    [Fact]
    public async virtual Task Translate_LessThanOrEqual_ReturnsRangeQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age <= 65);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Age");
        rangeQuery.Lte.ShouldNotBeNull();
        rangeQuery.Lte!.Value.ShouldBe(65);
    }

    [Fact]
    public async virtual Task Translate_DateComparison_GreaterThan_ReturnsDateRangeQuery()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.CreatedTime > date);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<DateRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("CreatedTime");
        rangeQuery.Gt.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_DateComparison_BetweenDates_ReturnsDateRangeQuery()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.CreatedTime >= startDate && x.CreatedTime <= endDate);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(2);

        // 两个 filter 都应该是 DateRangeQuery
        query.Bool.Filter.ShouldAllBe(filter => filter.Range is DateRangeQuery);
    }

    [Fact]
    public async virtual Task Translate_DecimalComparison_ReturnsRangeQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Salary >= 3000.50m);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Salary");
        rangeQuery.Gte.ShouldNotBeNull();
    }

    #endregion

    #region 字符串方法测试

    [Fact]
    public async virtual Task Translate_StringContains_KeywordField_ReturnsWildcardQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Contains("test"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Field.ToString().ShouldBe("Name.keyword");
        query.Wildcard.Value.ShouldBe("*test*");
    }

    [Fact]
    public async virtual Task Translate_StringStartsWith_KeywordField_ReturnsWildcardQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.StartsWith("test"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Field.ToString().ShouldBe("Name.keyword");
        query.Wildcard.Value.ShouldBe("test*");
    }

    [Fact]
    public async virtual Task Translate_StringEndsWith_KeywordField_ReturnsWildcardQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.EndsWith("test"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Field.ToString().ShouldBe("Name.keyword");
        query.Wildcard.Value.ShouldBe("*test");
    }

    [Fact]
    public async virtual Task Translate_StringContains_WildcardField_ReturnsWildcardQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Exceptions!.Contains("error"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Field.ToString().ShouldBe("Exceptions");
        query.Wildcard.Value.ShouldBe("*error*");
    }

    [Fact]
    public async virtual Task Translate_StringContains_TextWithoutKeyword_ReturnsMatchPhraseQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Description!.Contains("hello world"));

        // Assert
        query.ShouldNotBeNull();
        query.MatchPhrase.ShouldNotBeNull();
        query.MatchPhrase.Field.ToString().ShouldBe("Description");
        query.MatchPhrase.Query.ShouldBe("hello world");
    }

    [Fact]
    public async virtual Task Translate_StringStartsWith_TextWithoutKeyword_ReturnsMatchPhrasePrefixQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Description!.StartsWith("hello"));

        // Assert
        query.ShouldNotBeNull();
        query.MatchPhrasePrefix.ShouldNotBeNull();
        query.MatchPhrasePrefix.Field.ToString().ShouldBe("Description");
        query.MatchPhrasePrefix.Query.ShouldBe("hello");
    }

    [Fact]
    public async virtual Task Translate_StringContains_EscapesWildcardCharacters()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Contains("test*value"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Value.ShouldBe("*test\\*value*");
    }

    [Fact]
    public async virtual Task Translate_StringContains_EscapesQuestionMark()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Contains("test?value"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Value.ShouldBe("*test\\?value*");
    }

    [Fact]
    public async virtual Task Translate_StringEquals_ReturnsTermQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Equals("test"));

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Name.keyword");
        query.Term.Value.TryGetString(out var value).ShouldBeTrue();
        value.ShouldBe("test");
    }

    #endregion

    #region 集合方法测试

    [Fact]
    public async virtual Task Translate_EnumerableContains_Tags_ReturnsTermsQuery()
    {
        // Arrange
        var tags = new List<string> { "tag1", "tag2", "tag3" };

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => tags.Contains(x.Name!));

        // Assert
        query.ShouldNotBeNull();
        query.Terms.ShouldNotBeNull();
        query.Terms.Field.ToString().ShouldBe("Name.keyword");
    }

    [Fact]
    public async virtual Task Translate_CollectionContains_SingleItem_ReturnsTermQuery()
    {
        // Arrange
        var tags = new List<string> { "tag1" };

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => tags.Contains(x.Name!));

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Name.keyword");
    }

    [Fact]
    public async virtual Task Translate_EnumerableContains_EmptyCollection_ReturnsMatchNoneQuery()
    {
        // Arrange
        var tags = new List<string>();

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => tags.Contains(x.Name!));

        // Assert
        query.ShouldNotBeNull();
        query.MatchNone.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_EnumerableAny_WithoutPredicate_ReturnsNestedExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Items!.Any());

        // Assert
        query.ShouldNotBeNull();
        query.Nested.ShouldNotBeNull();
        query.Nested.Path.ShouldBe("Items");
        query.Nested.Query.Exists.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_EnumerableAny_WithPredicate_ReturnsNestedQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Items!.Any(i => i.Id == 10));

        // Assert
        query.ShouldNotBeNull();
        query.Nested.ShouldNotBeNull();
        query.Nested.Path.ShouldBe("Items");
        query.Nested.Query.Term.ShouldNotBeNull();
        query.Nested.Query.Term.Field.ToString().ShouldBe("Items.Id");
    }

    [Fact]
    public async virtual Task Translate_EnumerableAny_WithComplexPredicate_ReturnsNestedBoolQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Items!.Any(i => i.Price > 100 && i.Price < 1000));

        // Assert
        query.ShouldNotBeNull();
        query.Nested.ShouldNotBeNull();
        query.Nested.Path.ShouldBe("Items");
        query.Nested.Query.Bool.ShouldNotBeNull();
        query.Nested.Query.Bool.Filter.ShouldNotBeNull();
        query.Nested.Query.Bool.Filter.Count.ShouldBe(2);
    }

    [Fact]
    public async virtual Task Translate_EnumerableAll_ReturnsNestedQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Items!.All(i => i.Price > 0));

        // Assert
        query.ShouldNotBeNull();
        query.Nested.ShouldNotBeNull();
        query.Nested.Path.ShouldBe("Items");
        query.Nested.Query.Range.ShouldNotBeNull();
    }

    #endregion

    #region 嵌套属性测试

    [Fact]
    public async virtual Task Translate_NestedObjectProperty_ReturnsQueryWithNestedPath()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Address!.City == "Beijing");

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Address.City.keyword");
    }

    [Fact]
    public async virtual Task Translate_NestedObjectStringContains_ReturnsWildcardQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Address!.City!.Contains("bei"));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Field.ToString().ShouldBe("Address.City.keyword");
        query.Wildcard.Value.ShouldBe("*bei*");
    }

    [Fact]
    public async virtual Task Translate_NestedObjectProperty_NotEqualNull_ReturnsExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Address!.City != null);

        // Assert
        query.ShouldNotBeNull();
        query.Exists.ShouldNotBeNull();
        query.Exists.Field.ToString().ShouldBe("Address.City.keyword");
    }

    #endregion

    #region Nullable 属性测试

    [Fact]
    public async virtual Task Translate_NullableDate_HasValue_ReturnsExistsQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.UpdatedTime.HasValue);

        // Assert
        query.ShouldNotBeNull();
        query.Exists.ShouldNotBeNull();
        query.Exists.Field.ToString().ShouldBe("UpdatedTime");
    }

    #endregion

    #region 复杂表达式测试

    [Fact]
    public async virtual Task Translate_ComplexExpression_ReturnsCorrectQuery()
    {
        // Arrange
        var minAge = 18;
        var maxAge = 65;
        var namePrefix = "test";

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x =>
            (x.Age >= minAge && x.Age <= maxAge) &&
            (x.Name != null && x.Name.StartsWith(namePrefix)) &&
            x.IsActive == true &&
            (x.Status == TestEnum.Active || x.Status == TestEnum.Pending));

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(6);

        // 1. 验证年龄范围查询
        var rangeQueries = query.Bool.Filter.Where(q => q.Range != null).ToList();
        rangeQueries.Count.ShouldBe(2);

        var minAgeRange = rangeQueries[0].Range.ShouldBeOfType<NumberRangeQuery>();
        minAgeRange.Field.ToString().ShouldBe("Age");
        minAgeRange.Gte.ShouldNotBeNull();
        minAgeRange.Gte!.Value.ShouldBe(minAge);

        var maxAgeRange = rangeQueries[1].Range.ShouldBeOfType<NumberRangeQuery>();
        maxAgeRange.Field.ToString().ShouldBe("Age");
        maxAgeRange.Lte.ShouldNotBeNull();
        maxAgeRange.Lte!.Value.ShouldBe(maxAge);

        // 2. 验证 Exists 查询
        var existsQuery = query.Bool.Filter.FirstOrDefault(q => q.Exists != null);
        existsQuery.ShouldNotBeNull();
        existsQuery.Exists.ShouldNotBeNull();
        existsQuery.Exists.Field.ToString().ShouldBe("Name.keyword");

        // 3. 验证通配符查询
        var wildcardQuery = query.Bool.Filter.FirstOrDefault(q => q.Wildcard != null);
        wildcardQuery.ShouldNotBeNull();
        wildcardQuery.Wildcard.ShouldNotBeNull();
        wildcardQuery.Wildcard.Value.ShouldBe("test*");
        wildcardQuery.Wildcard.Field.ToString().ShouldBe("Name.keyword");

        // 4. 验证 Term 查询
        var termQuery = query.Bool.Filter.FirstOrDefault(q => q.Term != null);
        termQuery.ShouldNotBeNull();
        termQuery.Term.ShouldNotBeNull();
        termQuery.Term.Field.ToString().ShouldBe("IsActive");
        termQuery.Term.Value.TryGetBool(out var boolValue).ShouldBeTrue();
        boolValue.Value.ShouldBeTrue();

        // 5. 验证 Should 查询
        var shouldQuery = query.Bool.Filter.FirstOrDefault(q => q.Bool?.Should != null);
        shouldQuery.ShouldNotBeNull();
        shouldQuery.Bool.ShouldNotBeNull();
        shouldQuery.Bool.Should.ShouldNotBeNull();
        shouldQuery.Bool.Should.Count.ShouldBe(2);
        shouldQuery.Bool.MinimumShouldMatch.ShouldNotBeNull();
        shouldQuery.Bool.MinimumShouldMatch.Value1.ShouldBe(1);
    }

    [Fact]
    public async virtual Task Translate_ExpressionWithCapturedVariable_ReturnsCorrectQuery()
    {
        // Arrange
        var targetName = "John";
        var targetAge = 30;

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name == targetName && x.Age == targetAge);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(2);

        var nameTerm = query.Bool.Filter.First().Term;
        nameTerm.ShouldNotBeNull();
        nameTerm.Value.TryGetString(out var nameValue).ShouldBeTrue();
        nameValue.ShouldBe("John");

        var ageTerm = query.Bool.Filter.Last().Term;
        ageTerm.ShouldNotBeNull();
        ageTerm.Value.ShouldBe(targetAge);
    }

    [Fact]
    public async virtual Task Translate_ExpressionWithDateTimeVariable_ReturnsDateRangeQuery()
    {
        // Arrange
        var targetDate = new DateTime(2024, 6, 15);

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.CreatedTime == targetDate);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<DateRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("CreatedTime");
        rangeQuery.Gte.ShouldNotBeNull();
        rangeQuery.Lte.ShouldNotBeNull();
    }

    #endregion

    #region 边界情况测试

    [Fact]
    public async virtual Task Translate_UnsupportedMethod_ThrowsNotSupportedException()
    {
        // Act & Assert
        await Should.ThrowAsync<NotSupportedException>(async () => 
            await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.ToUpper() == "TEST"));
    }

    [Fact]
    public async virtual Task Translate_MultipleWhereConditions_ReturnsCorrectQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x =>
            (x.Age > 18 || x.Age < 65) &&
            x.Name != null &&
            !x.IsActive);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.Filter.ShouldNotBeNull();
        query.Bool.Filter.Count.ShouldBe(3);
    }

    [Fact]
    public async virtual Task Translate_NestedAnyWithContains_ReturnsNestedTermsQuery()
    {
        // Arrange
        var validIds = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Items!.Any(i => validIds.Contains(i.Id)));

        // Assert
        query.ShouldNotBeNull();
        query.Nested.ShouldNotBeNull();
        query.Nested.Path.ShouldBe("Items");
        query.Nested.Query.Terms.ShouldNotBeNull();
        query.Nested.Query.Terms.Field.ToString().ShouldBe("Items.Id");
    }

    [Fact]
    public async virtual Task Translate_StringEquals_NullValue_ReturnsMustNotExists()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Equals(null));

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);
        query.Bool.MustNot.First().Exists.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Translate_NotEqual_NullableEnum_ReturnsCorrectQuery()
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.NullableStatus != TestEnum.Active);

        // Assert
        query.ShouldNotBeNull();
        query.Bool.ShouldNotBeNull();
        query.Bool.MustNot.ShouldNotBeNull();
        query.Bool.MustNot.Count.ShouldBe(1);
        
        var mustNot = query.Bool.MustNot.First();
        mustNot.Term.ShouldNotBeNull();
        mustNot.Term.Field.ToString().ShouldBe("NullableStatus");
    }

    #endregion

    #region 使用 Theory 的参数化测试

    [Theory]
    [InlineData(TestEnum.Active, 1)]
    [InlineData(TestEnum.Inactive, 2)]
    [InlineData(TestEnum.Pending, 3)]
    public async virtual Task Translate_Equal_EnumField_ReturnsCorrectValue(TestEnum enumValue, long expectedValue)
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Status == enumValue);

        // Assert
        query.ShouldNotBeNull();
        query.Term.ShouldNotBeNull();
        query.Term.Field.ToString().ShouldBe("Status");
        query.Term.Value.ShouldBe(expectedValue);
    }

    [Theory]
    [InlineData(18)]
    [InlineData(30)]
    [InlineData(65)]
    public async virtual Task Translate_GreaterThan_VariousAges_ReturnsCorrectRange(int age)
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Age > age);

        // Assert
        query.ShouldNotBeNull();
        var rangeQuery = query.Range.ShouldBeOfType<NumberRangeQuery>();
        rangeQuery.Field.ToString().ShouldBe("Age");
        rangeQuery.Gt.ShouldNotBeNull();
        rangeQuery.Gt!.Value.ShouldBe(age);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("hello")]
    [InlineData("world")]
    public async virtual Task Translate_StringContains_VariousValues_ReturnsCorrectWildcard(string searchValue)
    {
        // Act
        var query = await _expressionQueryTranslator.TranslateAsync<TestDocument>(TestDocumentIndexNames.Index, x => x.Name!.Contains(searchValue));

        // Assert
        query.ShouldNotBeNull();
        query.Wildcard.ShouldNotBeNull();
        query.Wildcard.Value.ShouldBe($"*{searchValue}*");
    }

    #endregion
}