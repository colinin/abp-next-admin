using LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;
using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

public class CustomElementGetPagedListSpecification : Specification<CustomElement>
{
    protected CustomElementGetPagedListInput Input { get; }
    public CustomElementGetPagedListSpecification(CustomElementGetPagedListInput input)
    {
        Input = input;
    }
    public override Expression<Func<CustomElement, bool>> ToExpression()
    {
        Expression<Func<CustomElement, bool>> expression = _ => true;

        return expression
            .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x => 
                x.Name.Contains(Input.Filter!) || 
                (x.Description != null && x.Description.Contains(Input.Filter!)));
    }
}
