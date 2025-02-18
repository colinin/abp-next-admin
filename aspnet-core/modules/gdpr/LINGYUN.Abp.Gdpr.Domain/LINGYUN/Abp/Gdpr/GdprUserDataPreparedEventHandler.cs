using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Gdpr;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Gdpr;

public class GdprUserDataPreparedEventHandler(
    IGdprRequestRepository gdprRequestRepository,
    IJsonSerializer jsonSerializer,
    IGuidGenerator guidGenerator
):  IDistributedEventHandler<GdprUserDataPreparedEto>, 
    ITransientDependency
{
    [UnitOfWork]
    public async virtual Task HandleEventAsync(GdprUserDataPreparedEto eventData)
    {
        if (eventData.Data.Any())
        {
            var gdprRequest = await gdprRequestRepository.FindAsync(eventData.RequestId);
            if (gdprRequest != null)
            {
                var data = jsonSerializer.Serialize(eventData.Data);

                gdprRequest.AddData(
                    guidGenerator, 
                    data, 
                    eventData.Provider);

                await gdprRequestRepository.UpdateAsync(gdprRequest);
            }
        }
    }
}
