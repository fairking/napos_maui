using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;

namespace Napos.Web.Filters
{
    public class ModelBindingMetadataProvider : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Key.Name == null && context.Key.ModelType.IsClass)
            {
                context.BindingMetadata.BindingSource = BindingSource.Body;
            }
        }
    }
}
