using LINGYUN.Abp.PrintDesigner.Templates.Dtos;
using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public class TemplateGetPagedListSpecification : Specification<Template>
{
    protected TemplateGetPagedListInput Input { get; }
    public TemplateGetPagedListSpecification(TemplateGetPagedListInput input)
    {
        Input = input;
    }
    public override Expression<Func<Template, bool>> ToExpression()
    {
        Expression<Func<Template, bool>> expression = _ => true;

        return expression
            .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x => 
                x.Name.Contains(Input.Filter!) || 
                (x.Description != null && x.Description.Contains(Input.Filter!)));
    }
}
