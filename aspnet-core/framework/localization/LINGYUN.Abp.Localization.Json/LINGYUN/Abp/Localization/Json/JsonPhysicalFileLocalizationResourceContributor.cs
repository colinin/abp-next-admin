﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Internal;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Json;

namespace LINGYUN.Abp.Localization.Json
{
    public class JsonPhysicalFileLocalizationResourceContributor : ILocalizationResourceContributor
    {
        private readonly string _filePath;

        private IFileProvider _fileProvider;
        private Dictionary<string, ILocalizationDictionary> _dictionaries;
        private bool _subscribedForChanges;
        private readonly object _syncObj = new object();

        public JsonPhysicalFileLocalizationResourceContributor(string filePath)
        {
            _filePath = filePath;
        }

        public void Initialize(LocalizationResourceInitializationContext context)
        {
            _fileProvider = new PhysicalFileProvider(_filePath);
        }

        public LocalizedString GetOrNull(string cultureName, string name)
        {
            return GetDictionaries().GetOrDefault(cultureName)?.GetOrNull(name);
        }

        public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
        {
            GetDictionaries().GetOrDefault(cultureName)?.Fill(dictionary);
        }

        protected virtual Dictionary<string, ILocalizationDictionary> GetDictionaries()
        {
            var dictionaries = _dictionaries;
            if (dictionaries != null)
            {
                return dictionaries;
            }

            lock (_syncObj)
            {
                dictionaries = _dictionaries;
                if (dictionaries != null)
                {
                    return dictionaries;
                }

                if (!_subscribedForChanges)
                {
                    ChangeToken.OnChange(() => _fileProvider.Watch(_filePath.EnsureEndsWith('/') + "*.*"),
                        () =>
                        {
                            _dictionaries = null;
                        });

                    _subscribedForChanges = true;
                }

                dictionaries = _dictionaries = CreateDictionaries();
            }

            return dictionaries;
        }

        protected virtual Dictionary<string, ILocalizationDictionary> CreateDictionaries()
        {
            var dictionaries = new Dictionary<string, ILocalizationDictionary>();

            foreach (var file in _fileProvider.GetDirectoryContents(string.Empty))
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

        protected virtual bool CanParseFile(IFileInfo file)
        {
            return file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
        }

        protected virtual ILocalizationDictionary CreateDictionaryFromFile(IFileInfo file)
        {
            using (var stream = file.CreateReadStream())
            {
                return CreateDictionaryFromFileContent(Utf8Helper.ReadStringFromStream(stream));
            }
        }

        protected virtual ILocalizationDictionary CreateDictionaryFromFileContent(string fileContent)
        {
            return JsonLocalizationDictionaryBuilder.BuildFromJsonString(fileContent);
        }
    }
}
