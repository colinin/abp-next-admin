using Dapr.Actors.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class TestActor : Actor, ITestActor, ISingletonDependency
    {
        public TestActor(ActorHost host) : base(host)
        {
        }

        public async Task<List<NameValue>> GetAsync()
        {
            var values = await GetValuesByStateAsync();

            return values;
        }

        public async Task<NameValue> UpdateAsync()
        {
            var values = await GetValuesByStateAsync();

            var inctement = await StateManager.AddOrUpdateStateAsync(
                "test:actors:increment", 
                1,
                (key, value) =>
                {
                    return value + 1;
                });

            values[0].Value = $"value:updated:{inctement}";

            return values[0];
        }

        public async Task ClearAsync()
        {
            await StateManager.TryRemoveStateAsync("test:actors:increment");
            await StateManager.TryRemoveStateAsync("test:actors");
        }

        protected virtual async Task<List<NameValue>> GetValuesByStateAsync()
        {
            return await StateManager
                .GetOrAddStateAsync(
                    "test:actors",
                    new List<NameValue>
                    {
                        new NameValue("name1", "value1"),
                        new NameValue("name2", "value2"),
                        new NameValue("name3", "value3"),
                        new NameValue("name4", "value4"),
                        new NameValue("name5", "value5")
                    });
        }
    }
}
