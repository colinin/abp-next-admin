using System.Linq;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelStateExtensions
{
    public static void RemoveModelErrors(this ModelStateDictionary modelState, string modelName)
    {
        var keys = modelState.Keys
            .Where(k => k.StartsWith(modelName + ".") || k == modelName)
            .ToList();

        foreach (var key in keys)
        {
            modelState.Remove(key);
        }
    }

    public static bool IsValidForModel(this ModelStateDictionary modelState, string modelName)
    {
        modelState.RemoveModelErrors(modelName);

        return modelState.Keys
            .Where(k => k.StartsWith(modelName + ".") || k == modelName)
            .All(key => modelState[key].ValidationState == ModelValidationState.Valid);
    }
}
