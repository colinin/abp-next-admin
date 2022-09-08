using LINYUN.Abp.Location.Amap.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Json;
using Volo.Abp.Threading;
using Volo.Abp.DependencyInjection;
using System.Linq;
using System.Collections.Generic;

namespace LINGYUN.Abp.Location.Amap
{
    public class AmapHttpRequestClient : ITransientDependency
    {
        protected AmapLocationOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected IJsonSerializer JsonSerializer { get; }

        protected IHttpClientFactory HttpClientFactory { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public AmapHttpRequestClient(
            IJsonSerializer jsonSerializer,
            IServiceProvider serviceProvider,
            IOptions<AmapLocationOptions> options,
            IHttpClientFactory httpClientFactory,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            Options = options.Value;
            JsonSerializer = jsonSerializer;
            ServiceProvider = serviceProvider;
            HttpClientFactory = httpClientFactory;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async virtual Task<GecodeLocation> PositiveAsync(AmapPositiveHttpRequestParamter requestParamter)
        {
            var client = HttpClientFactory.CreateClient(AmapHttpConsts.HttpClientName);
            var requestUrlBuilder = new StringBuilder(128);
            requestUrlBuilder.Append("http://restapi.amap.com/v3/geocode/geo");
            requestUrlBuilder.AppendFormat("?key={0}", Options.ApiKey);
            requestUrlBuilder.AppendFormat("&address={0}", requestParamter.Address);
            if (!requestParamter.City.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&city={0}", requestParamter.City);
            }
            if (!requestParamter.Output.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&output={0}", requestParamter.Output);
            }
            if (!requestParamter.Sig.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&sig={0}", requestParamter.Sig);
            }
            requestUrlBuilder.AppendFormat("&batch={0}", requestParamter.Batch);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrlBuilder.ToString());

            var response = await client.SendAsync(requestMessage, GetCancellationToken());

            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException($"Amap request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            var amapResponse = JsonSerializer.Deserialize<AmapPositiveHttpResponse>(resultContent);

            if (!amapResponse.IsSuccess())
            {
                var localizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
                var localizerError = amapResponse.GetErrorMessage().Localize(localizerFactory);
                if (Options.VisableErrorToClient)
                {
                    var localizer = ServiceProvider.GetRequiredService<IStringLocalizer<AmapLocationResource>>();
                    localizerError = localizer["ResolveLocationFailed", localizerError];
                    throw new UserFriendlyException(localizerError);
                }
                throw new AbpException($"Resolution address failed:{localizerError}!");
            }
            if(amapResponse.Count <= 0)
            {
                var localizer = ServiceProvider.GetRequiredService<IStringLocalizer<AmapLocationResource>>();
                var localizerError = localizer["ResolveLocationZero"];
                if (Options.VisableErrorToClient)
                {
                    throw new UserFriendlyException(localizerError);
                }
                throw new AbpException(localizerError);
            }

            var locations = amapResponse.Geocodes[0].Location.Split(",");
            var postiveLocation =  new GecodeLocation
            {
                Longitude = double.Parse(locations[0]),
                Latitude = double.Parse(locations[1]),
                Level = amapResponse.Geocodes[0].Level
            };
            postiveLocation.AddAdditional("Geocodes", amapResponse.Geocodes);

            return postiveLocation;
        }

        public async virtual Task<ReGeocodeLocation> InverseAsync(AmapInverseHttpRequestParamter requestParamter)
        {
            if(requestParamter.Locations.Length > 20)
            {
                var localizer = ServiceProvider.GetRequiredService<IStringLocalizer<AmapLocationResource>>();
                var localizerError = localizer["SupportsResolveAddress", 20];
                localizerError = localizer["ResolveLocationFailed", localizerError];
                if (Options.VisableErrorToClient)
                {
                    throw new UserFriendlyException(localizerError);
                }
                throw new AbpException($"Resolution address failed:{localizerError}!");
            }
            var client = HttpClientFactory.CreateClient(AmapHttpConsts.HttpClientName);
            var requestUrlBuilder = new StringBuilder(128);
            requestUrlBuilder.Append("http://restapi.amap.com/v3/geocode/regeo");
            requestUrlBuilder.AppendFormat("?key={0}", Options.ApiKey);
            requestUrlBuilder.AppendFormat("&batch={0}", requestParamter.Batch);
            requestUrlBuilder.AppendFormat("&output={0}", requestParamter.Output);
            requestUrlBuilder.AppendFormat("&radius={0}", requestParamter.Radius);
            requestUrlBuilder.AppendFormat("&extensions={0}", requestParamter.Extensions);
            requestUrlBuilder.AppendFormat("&homeorcorp={0}", requestParamter.HomeOrCorp);
            requestUrlBuilder.AppendFormat("&location=");
            for(int i = 0; i < requestParamter.Locations.Length; i++)
            {
                requestUrlBuilder.AppendFormat("{0},{1}", Math.Round(requestParamter.Locations[i].Longitude, 6), 
                    Math.Round(requestParamter.Locations[i].Latitude, 6));
                if (i < requestParamter.Locations.Length - 1)
                {
                    requestUrlBuilder.Append("|");
                }
            }
            if (!requestParamter.PoiType.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&poitype={0}", requestParamter.PoiType);
            }
            if (!requestParamter.PoiType.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&poitype={0}", requestParamter.PoiType);
            }
            if (!requestParamter.Sig.IsNullOrWhiteSpace())
            {
                requestUrlBuilder.AppendFormat("&sig={0}", requestParamter.Sig);
            }
            if (requestParamter.RoadLevel.HasValue)
            {
                requestUrlBuilder.AppendFormat("&roadlevel={0}", requestParamter.RoadLevel);
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrlBuilder.ToString());

            var response = await client.SendAsync(requestMessage, GetCancellationToken());

            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException($"Amap request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
            }

            var resultContent = await response.Content.ReadAsStringAsync();
            var amapResponse = JsonSerializer.Deserialize<AmapInverseLocationResponse>(resultContent);

            if (!amapResponse.IsSuccess())
            {
                var localizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
                var localizerError = amapResponse.GetErrorMessage().Localize(localizerFactory);
                if (Options.VisableErrorToClient)
                {
                    var localizer = ServiceProvider.GetRequiredService<IStringLocalizer<AmapLocationResource>>();
                    localizerError = localizer["ResolveLocationFailed", localizerError];
                    throw new UserFriendlyException(localizerError);
                }
                throw new AbpException($"Resolution address failed:{localizerError}!");
            }
            var inverseLocation = new ReGeocodeLocation
            {
                Street = amapResponse.Regeocode.AddressComponent.StreetNumber.Street,
                AdCode = amapResponse.Regeocode.AddressComponent.AdCode,
                Address = amapResponse.Regeocode.Address,
                City = amapResponse.Regeocode.AddressComponent.City.JoinAsString(","),
                Country = amapResponse.Regeocode.AddressComponent.Country,
                District = amapResponse.Regeocode.AddressComponent.District,
                Number = amapResponse.Regeocode.AddressComponent.StreetNumber.Number,
                Province = amapResponse.Regeocode.AddressComponent.Province,
                Town = amapResponse.Regeocode.AddressComponent.TownShip.JoinAsString(" ")
            };
            return inverseLocation;
        }

        protected virtual CancellationToken GetCancellationToken()
        {
            return CancellationTokenProvider.Token;
        }
    }
}
