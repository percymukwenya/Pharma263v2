using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Threading.Tasks;

namespace Pharma263.MVC.Helpers
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            decimal result;
            var culture = CultureInfo.InvariantCulture;

            if (decimal.TryParse(valueProviderResult.FirstValue, NumberStyles.AllowDecimalPoint, culture, out result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid decimal value.");
            }

            return Task.CompletedTask;
        }
    }
}
