using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.AspNetCore.Mvc.Validation
{
    //[Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    //[ExposeServices(typeof(IModelMetadataProvider))]
    public class AbpLocalizerModelMetadataProvider : DefaultModelMetadataProvider
    {
        protected IStringLocalizer StringLocalizer { get; }
        public AbpLocalizerModelMetadataProvider(
            ICompositeMetadataDetailsProvider detailsProvider,
            IStringLocalizer<AbpValidationResource> stringLocalizer) 
            : base(detailsProvider)
        {
            StringLocalizer = stringLocalizer;
        }

        public AbpLocalizerModelMetadataProvider(
            IStringLocalizer<AbpValidationResource> stringLocalizer,
            ICompositeMetadataDetailsProvider detailsProvider, 
            IOptions<MvcOptions> optionsAccessor) 
            : base(detailsProvider, optionsAccessor)
        {
            StringLocalizer = stringLocalizer;
        }

        protected override DefaultMetadataDetails[] CreatePropertyDetails(ModelMetadataIdentity key)
        {
            var details = base.CreatePropertyDetails(key);

            foreach (var detail in details)
            {
                NormalizeMetadataDetail(detail);
            }

            return details;
        }

        private void NormalizeMetadataDetail(DefaultMetadataDetails detail)
        {
            foreach (var validationAttribute in detail.ModelAttributes.Attributes.OfType<ValidationAttribute>())
            {
                NormalizeValidationAttrbute(validationAttribute, detail.DisplayMetadata?.DisplayFormatString ?? detail.Key.Name);
            }
        }

        protected virtual void NormalizeValidationAttrbute(ValidationAttribute validationAttribute, string displayName)
        {
            if (validationAttribute.ErrorMessage == null)
            {
                if (validationAttribute is RequiredAttribute required)
                {
                    validationAttribute.ErrorMessage = StringLocalizer["The field {0} is invalid.", displayName];
                }
            }
        }
    }
}
