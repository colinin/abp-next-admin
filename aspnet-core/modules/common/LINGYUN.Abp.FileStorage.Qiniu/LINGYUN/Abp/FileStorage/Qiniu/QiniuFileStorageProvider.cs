using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.FileStorage.Qiniu
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IFileStorageProvider), typeof(FileStorageProvider))]
    public class QiniuFileStorageProvider : FileStorageProvider
    {
        protected QiniuFileStorageOptions Options { get; }
        public QiniuFileStorageProvider(
            IFileStore store,
            IOptions<QiniuFileStorageOptions> options) 
            : base(store)
        {
            Options = options.Value;
        }

        protected override async Task<FileInfo> DownloadFileAsync(FileInfo fileInfo, string saveLocalPath)
        {
            Mac mac = new Mac(Options.AccessKey, Options.SecretKey);

            int expireInSeconds = 3600;
            string accUrl = DownloadManager.CreateSignedUrl(mac, fileInfo.Url, expireInSeconds);

            var saveLocalFile = Path.Combine(saveLocalPath, fileInfo.Name);
            var httpResult = await DownloadManager.DownloadAsync(accUrl, saveLocalFile);
            if(httpResult.Code == 200)
            {
                using (var fs = new FileStream(saveLocalFile, FileMode.Open, FileAccess.Read))
                {
                    fileInfo.Data = new byte[fs.Length];

                    await fs.ReadAsync(fileInfo.Data, 0, fileInfo.Data.Length).ConfigureAwait(false);
                }
            }
            else
            {
                // TODO: 处理响应代码

                Console.WriteLine(httpResult.Code);
            }

            return fileInfo;
        }

        protected override async Task RemoveFileAsync(FileInfo fileInfo, CancellationToken cancellationToken = default)
        {
            Mac mac = new Mac(Options.AccessKey, Options.SecretKey);

            var bucket = fileInfo.Directory + ":" + fileInfo.Name;
            var backetManager = new BucketManager(mac);
            await backetManager.DeleteAsync(bucket, fileInfo.Name);

            throw new NotImplementedException();
        }

        protected override async Task UploadFileAsync(FileInfo fileInfo, int? expireIn = null, CancellationToken cancellationToken = default)
        {
            Mac mac = new Mac(Options.AccessKey, Options.SecretKey);

            PutPolicy putPolicy = new PutPolicy
            {
                Scope = fileInfo.Directory + ":" + fileInfo.Name,
                CallbackBody = Options.UploadCallbackBody,
                CallbackBodyType = Options.UploadCallbackBodyType,
                CallbackHost = Options.UploadCallbackHost,
                CallbackUrl = Options.UploadCallbackUrl
            };
            if (expireIn.HasValue)
            {
                putPolicy.SetExpires(expireIn.Value);
            }
            if (Options.DeleteAfterDays > 0)
            {
                putPolicy.DeleteAfterDays = Options.DeleteAfterDays;
            }


            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);

            UploadProgressHandler handler = (uploadByte, totalByte) =>
            {
                OnFileUploadProgressChanged(uploadByte, totalByte);
            };

            // 带进度的上传
            ResumableUploader uploader = new ResumableUploader();
            var httpResult = await uploader.UploadDataAsync(fileInfo.Data, fileInfo.Name, token, handler);

            // 普通上传
            //FormUploader fu = new FormUploader();
            //var httpResult = await fu.UploadDataAsync(fileInfo.Data, fileInfo.Name, token);

            // TODO: 处理响应代码

            Console.WriteLine(httpResult.Code);
        }
    }
}
