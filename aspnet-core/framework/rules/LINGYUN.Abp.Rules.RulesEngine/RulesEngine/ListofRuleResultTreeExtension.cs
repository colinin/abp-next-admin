using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Volo.Abp.Validation;

namespace RulesEngine
{
    public static class ListofRuleResultTreeExtension
    {
        public static void ThrowOfFaildExecute(this IEnumerable<RuleResultTree> ruleResultTrees)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            ruleResultTrees.WrapValidationResult(validationResults);

            if (validationResults.Any())
            {
                throw new AbpValidationException("一个或多个规则未通过", validationResults);
            }
        }

        private static void WrapValidationResult(this IEnumerable<RuleResultTree> ruleResultTrees, List<ValidationResult> validationResults)
        {
            var failedResults = ruleResultTrees.Where(rule => !rule.IsSuccess).ToArray();

            foreach (var failedResult in failedResults)
            {
                string member = null;
                var errorBuilder = new StringBuilder(36);
                if (!failedResult.ExceptionMessage.IsNullOrWhiteSpace())
                {
                    errorBuilder.AppendLine(failedResult.ExceptionMessage);
                }

                // TODO: 需要修改源代码, ChildResults的验证错误个人认为有必要展示出来
                if (failedResult.ChildResults != null && failedResult.ChildResults.Any())
                {
                    errorBuilder.Append(failedResult.ChildResults.GetErrorMessage(out member));
                }

                validationResults.Add(new ValidationResult(
                    errorBuilder.ToString().TrimEnd(),
                    new string[] { member ?? failedResult.Rule?.Properties?.GetOrDefault("Property")?.ToString() ?? "input" }));
            }
        }



        private static string GetErrorMessage(this IEnumerable<RuleResultTree> ruleResultTrees, out string member)
        {
            member = null;
            var errorBuilder = new StringBuilder(36);
            var failedResults = ruleResultTrees.Where(rule => !rule.IsSuccess).ToArray();

            for (int index = 0; index < failedResults.Length; index++)
            {
                member = failedResults[index].Rule?.Properties?.GetOrDefault("Property")?.ToString();

                if (!failedResults[index].ExceptionMessage.IsNullOrWhiteSpace())
                {
                    errorBuilder.AppendLine(failedResults[index].ExceptionMessage);
                }

                if (failedResults[index].ChildResults != null && failedResults[index].ChildResults.Any())
                {
                    errorBuilder.Append(failedResults[index].ChildResults.GetErrorMessage(out member));
                }
            }

            return errorBuilder.ToString();
        }
    }
}
