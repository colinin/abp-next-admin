# LINGYUN.Abp.Location.Tencent

## Introduction

`LINGYUN.Abp.Location.Tencent` is a location service implementation module based on Tencent Maps API, providing functionalities such as geocoding, reverse geocoding, IP location, and more.

## Features

* Geocoding: Convert structured addresses into latitude and longitude coordinates
* Reverse Geocoding: Convert coordinates into structured addresses
* IP Location: Get location information based on IP addresses
* POI (Points of Interest) Information: Get information about nearby businesses, restaurants, and other points of interest
* Administrative Region Information: Get detailed administrative region hierarchy information
* Address Parsing: Intelligent address parsing supporting multiple formats

## Installation

```bash
dotnet add package LINGYUN.Abp.Location.Tencent
```

## Configuration

1. Add module dependency:

```csharp
[DependsOn(typeof(AbpTencentLocationModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<TencentLocationOptions>(options =>
        {
            // Set Tencent Maps API key
            options.Key = "your-tencent-map-key";
            // Optional: Set security key (for SK verification)
            options.SecretKey = "your-tencent-map-sk";
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
    "title": "Tiananmen",    // Place name
    "address": "Dongchangan Street, Dongcheng District, Beijing",  // Address
    "category": "Tourist Attraction",  // Category
    "adcode": "110101",      // Administrative region code
    "similarity": 0.8,       // Similarity (0-1)
    "reliability": 7,        // Reliability (1-10)
    "level": 11             // Address type
}
```

### Reverse Geocoding Response Data

```json
{
    "location": {
        "lat": 39.915119,    // Latitude value
        "lng": 116.403963    // Longitude value
    },
    "address": "Dongchangan Street, Dongcheng District, Beijing",  // Complete address
    "formatted_addresses": {
        "recommend": "Tiananmen, Dongcheng District",    // Recommended address
        "rough": "Dongcheng District, Beijing"           // Rough address
    },
    "address_component": {
        "nation": "China",                              // Country
        "province": "Beijing",                          // Province
        "city": "Beijing",                              // City
        "district": "Dongcheng District",               // District
        "street": "Dongchangan Street",                 // Street
        "street_number": "1"                            // Street number
    },
    "pois": [                                          // Nearby POIs
        {
            "title": "Tiananmen",                      // POI name
            "address": "Dongchangan Street, Dongcheng District, Beijing", // POI address
            "category": "Tourist Attraction",           // POI type
            "distance": 100,                           // Distance (meters)
            "_distance": 100.0,                        // Distance (meters, float)
            "tel": "",                                // Phone number
            "ad_info": {                              // Administrative region info
                "adcode": "110101",                   // Administrative region code
                "name": "Dongcheng District",         // Administrative region name
                "location": {                         // Administrative region center point
                    "lat": 39.915119,
                    "lng": 116.403963
                }
            }
        }
    ]
}
```

## More Information

* [中文文档](./README.md)
* [Tencent Location Service](https://lbs.qq.com/)
