using LINGYUN.Abp.OssManagement.Localization;
using LINGYUN.Abp.OssManagement.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.OssManagement
{
    public class FileValidater : IFileValidater, ISingletonDependency
    {
        private readonly IMemoryCache _cache;
        private readonly ISettingProvider _settingProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer _stringLocalizer;

        public FileValidater(
            IMemoryCache cache,
            ISettingProvider settingProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<AbpOssManagementResource> stringLocalizer)
        {
            _cache = cache;
            _settingProvider = settingProvider;
            _serviceProvider = serviceProvider;
            _stringLocalizer = stringLocalizer;
        }

        public virtual async Task ValidationAsync(UploadFile input)
        {
            var validation = await GetByCacheItemAsync();
            if (validation.SizeLimit * 1024 * 1024 < input.TotalSize)
            {
                throw new UserFriendlyException(_stringLocalizer["UploadFileSizeBeyondLimit", validation.SizeLimit]);
            }
            var fileExtensionName = FileHelper.GetExtension(input.FileName);
            if (!validation.AllowedExtensions
                    .Any(fe => fe.Equals(fileExtensionName, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new UserFriendlyException(_stringLocalizer["NotAllowedFileExtensionName", fileExtensionName]);
            }
        }

        protected virtual async Task<FileValidation> GetByCacheItemAsync()
        {
            var fileValidation = _cache.Get<FileValidation>(FileValidation.CacheKey);
            if (fileValidation == null)
            {
                fileValidation = await GetBySettingAsync();
                _cache.Set(FileValidation.CacheKey,
                    fileValidation,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)
                    });
            }
            return fileValidation;
        }

        protected virtual async Task<FileValidation> GetBySettingAsync()
        {
            var fileSizeLimited = await _settingProvider
                .GetAsync(
                    AbpOssManagementSettingNames.FileLimitLength,
                    AbpOssManagementSettingNames.DefaultFileLimitLength);
            var fileAllowExtension = await _settingProvider
                .GetOrDefaultAsync(AbpOssManagementSettingNames.AllowFileExtensions, _serviceProvider);

            return new FileValidation(fileSizeLimited, fileAllowExtension.Split(','));
        }
    }

    public class FileValidation
    {
        public const string CacheKey = "Abp.OssManagement.FileValidation";
        public long SizeLimit { get; set; }
        public string[] AllowedExtensions { get; set; }
        public FileValidation()
        {

        }

        public FileValidation(
            long sizeLimit,
            string[] allowedExtensions)
        {
            SizeLimit = sizeLimit;
            AllowedExtensions = allowedExtensions;
        }
    }
}
