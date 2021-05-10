using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Validation;
using Xunit;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class TestMultipleRuleInputTests : AbpRulesEngineTestBase
    {
        private readonly IRuleProvider _ruleProvider;

        public TestMultipleRuleInputTests()
        {
            _ruleProvider = GetRequiredService<IRuleProvider>();
        }

        [Fact]
        public async Task Multiple_Rule_Input_Should_Failed()
        {
            var input = new TestMultipleRuleInput
            {
                Length = "12345",
                Integer1 = 100,
                Integer2 = 100
            };

            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _ruleProvider.ExecuteAsync(input);
            });

            exception.Message.ShouldBe("一个或多个规则未通过");
            exception.ValidationErrors.Count.ShouldBe(2);
            exception.ValidationErrors[0].ErrorMessage.ShouldBe("输入字段验证无效!");
            exception.ValidationErrors[1].ErrorMessage.ShouldBe("长度与求和验证无效!");
        }
    }
}
