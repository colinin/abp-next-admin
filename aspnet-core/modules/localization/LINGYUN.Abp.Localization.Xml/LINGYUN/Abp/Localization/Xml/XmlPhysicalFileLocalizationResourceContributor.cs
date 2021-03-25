using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Xml
{
    public class XmlPhysicalFileLocalizationResourceContributor : XmlFileLocalizationResourceContributorBase
    {
        private readonly string _filePath;

        public XmlPhysicalFileLocalizationResourceContributor(string filePath)
            : base(filePath)
        {
            _filePath = filePath;
        }

        protected override IFileProvider BuildFileProvider(LocalizationResourceInitializationContext context)
        {
            return new PhysicalFileProvider(_filePath);
        }

        protected override Dictionary<string, ILocalizationDictionary> CreateDictionaries(IFileProvider fileProvider, string filePath)
        {
            var dictionaries = new Dictionary<string, ILocalizationDictionary>();

            foreach (var file in fileProvider.GetDirectoryContents(string.Empty))
            {
                if (file.IsDirectory || !CanParseFile(file))
                {
                    continue;
                }

                var dictionary = CreateDictionaryFromFile(file);
                if (dictionaries.ContainsKey(dictionary.CultureName))
                {
                    throw new AbpException($"{file.GetVirtualOrPhysicalPathOrNull()} dictionary has a culture name '{dictionary.CultureName}' which is already defined!");
                }

                dictionaries[dictionary.CultureName] = dictionary;
            }

            return dictionaries;
        }
    }
}
