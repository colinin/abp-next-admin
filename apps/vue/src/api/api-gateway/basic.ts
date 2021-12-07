import { defAbpHttp } from '/@/utils/http/abp';
import { LoadBalancerDescriptor } from './model/basicModel';
import { ListResultDto } from '../model/baseModel';

enum Api {
  GetLoadBalancerProviders = '/api/ApiGateway/Basic/LoadBalancers',
  GetDefinedAggregatorProvicers = '/api/ApiGateway/Basic/Aggregators',
}

export const getLoadBalancerProviders = () => {
  return defAbpHttp.get<ListResultDto<LoadBalancerDescriptor>>({
    url: Api.GetLoadBalancerProviders,
  });
};

export async function getDefinedAggregatorProviders() {
  const { items } = await defAbpHttp.get<ListResultDto<string>>({
    url: Api.GetDefinedAggregatorProvicers,
  });
  // 需要对返回格式转换以适配 ApiSelect组件
  return items.map((item) => {
    return {
      provider: item,
    };
  });
}
