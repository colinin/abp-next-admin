using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace LINGYUN.Platform.Datas
{
    public class DataDictionaryDataSeeder : IDataDictionaryDataSeeder, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected IDataRepository DataRepository { get; }

        public DataDictionaryDataSeeder(
            IGuidGenerator guidGenerator,
            IDataRepository dataRepository)
        {
            GuidGenerator = guidGenerator;
            DataRepository = dataRepository;
        }

        public virtual async Task<Data> SeedAsync(
            string name,
            string code,
            string displayName,
            string description = "",
            Guid? parentId = null,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var data = await DataRepository.FindByNameAsync(name, cancellationToken: cancellationToken);

            if (data == null)
            {
                data = new Data(
                    GuidGenerator.Create(),
                    name,
                    code,
                    displayName,
                    description,
                    parentId,
                    tenantId);

                data = await DataRepository.InsertAsync(data);
            }

            return data;
        }
    }
}
