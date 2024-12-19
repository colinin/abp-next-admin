# LINGYUN.Abp.Location.Baidu

## Introduction

`LINGYUN.Abp.Location.Baidu` is a location service implementation module based on Baidu Maps API, providing functionalities such as geocoding, reverse geocoding, IP location, and more.

## Features

* Geocoding: Convert structured addresses into latitude and longitude coordinates
* Reverse Geocoding: Convert coordinates into structured addresses
* IP Location: Get location information based on IP addresses
* POI (Points of Interest) Information: Get information about nearby businesses, restaurants, and other points of interest
* Road Information: Get information about nearby roads
* Administrative Region Information: Get detailed administrative region hierarchy information

## Installation

```bash
dotnet add package LINGYUN.Abp.Location.Baidu
```

## Configuration

1. Add module dependency:

```csharp
[DependsOn(typeof(AbpBaiduLocationModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<BaiduLocationOptions>(options =>
        {
            // Set Baidu Maps API key
            options.AccessKey = "your-baidu-map-ak";
            // Optional: Set security key (for sn verification)
            options.SecurityKey = "your-baidu-map-sk";
            // Optional: Set coordinate system type (default is bd09ll)
            options.CoordType = "bd09ll";
            // Optional: Set output format (default is json)
            options.Output = "json";
        });
    }
}
```

## Usage

1. Inject and use the location resolution service:

```csharp
public class YourLocationService
{
    private readonly ILocationResolveProvider _locationProvider;

    public YourLocationService(ILocationResolveProvider locationProvider)
    {
        _locationProvider = locationProvider;
    }

    // Geocoding: Convert address to coordinates
    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        // city parameter is optional, used to specify the city of the address
        return await _locationProvider.GeocodeAsync(address, "Beijing");
    }

    // Reverse Geocoding: Convert coordinates to address
    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        // radius parameter is optional, specifies search radius (in meters)
        return await _locationProvider.ReGeocodeAsync(lat, lng, 1000);
    }

    // IP Geolocation
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        return await _locationProvider.IPGeocodeAsync(ipAddress);
    }
}
```

## Response Data Description

### Geocoding Response Data

```json
{
    "location": {
        "lat": 39.915119,    // Latitude value
        "lng": 116.403963    // Longitude value
    },
    "precise": 1,            // Additional location info, precise match or not (1 for precise, 0 for not precise)
    "confidence": 80,        // Confidence level
    "comprehension": 100,    // Address understanding level
    "level": "门址"          // Address type
}
```

### Reverse Geocoding Response Data

```json
{
    "location": {
        "lat": 39.915119,    // Latitude value
        "lng": 116.403963    // Longitude value
    },
    "formatted_address": "Dongchangan Street, Dongcheng District, Beijing",  // Structured address
    "business": "Tiananmen",                   // Business area information
    "addressComponent": {
        "country": "China",                    // Country
        "province": "Beijing",                 // Province
        "city": "Beijing",                     // City
        "district": "Dongcheng District",      // District
        "street": "Dongchangan Street",        // Street
        "street_number": "1"                   // Street number
    },
    "pois": [                                 // Nearby POIs
        {
            "name": "Tiananmen",              // POI name
            "type": "Tourist Attraction",      // POI type
            "distance": "100"                  // Distance (meters)
        }
    ],
    "roads": [                                // Nearby roads
        {
            "name": "Dongchangan Street",      // Road name
            "distance": "50"                   // Distance (meters)
        }
    ]
}
```

## More Information

* [中文文档](./README.md)
* [Baidu Maps Open Platform](https://lbsyun.baidu.com/)
