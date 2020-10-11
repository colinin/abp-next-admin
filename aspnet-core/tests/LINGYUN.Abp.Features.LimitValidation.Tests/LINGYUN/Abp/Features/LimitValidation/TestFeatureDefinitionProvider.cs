using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class TestFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var featureGroup = context.AddGroup(TestFeatureNames.GroupName);
            featureGroup.AddFeature(
                name: TestFeatureNames.TestLimitFeature,
                defaultValue: 100.ToString(),
                valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
            featureGroup.AddFeature(
                name: TestFeatureNames.TestIntervalFeature,
                defaultValue: 1.ToString(),
                valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
        }
    }
}
