using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace ECommerce.Presentation.Presentation.ModelBinders;

public class ListModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext modelBindingContext)
    {
        if (!modelBindingContext.ModelMetadata.IsEnumerableType)
        {
            modelBindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var providedValue = modelBindingContext.ValueProvider
            .GetValue(modelBindingContext.ModelName)
            .ToString();
        if (string.IsNullOrEmpty(providedValue))
        {
            modelBindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var genericType =
            modelBindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(genericType);

        var objectArray = providedValue.Split(new[] { "," },
                StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim()))
            .ToArray();

        var genericArray = Array.CreateInstance(genericType, objectArray.Length);

        objectArray.CopyTo(genericArray, 0);

        modelBindingContext.Model = genericArray switch
        {
            short[] => genericArray.OfType<short>().ToList(),
            int[] => genericArray.OfType<int>().ToList(),
            long[] => genericArray.OfType<long>().ToList(),
            _ => modelBindingContext.Model
        };

        modelBindingContext.Result = ModelBindingResult.Success(modelBindingContext.Model);

        return Task.CompletedTask;
    }
}