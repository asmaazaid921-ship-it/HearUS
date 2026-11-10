using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HearUS.Web.Extensions;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, ValidationException exception)
    {
        foreach (var error in exception.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}
