# LINGYUN.Abp.Location

## Introduction

`LINGYUN.Abp.Location` is a location service foundation module that provides geographic location-related functionality, including geocoding (forward/reverse), distance calculation, and more.

## Features

* Geocoding and Reverse Geocoding
* IP Geolocation Resolution
* Location distance calculation (based on Google algorithm, error <0.2m)
* Location offset calculation
* Support for POI (Points of Interest) and road information
* Extensible location resolution providers

## Installation

```bash
dotnet add package LINGYUN.Abp.Location
```

## Usage

1. Add module dependency:

```csharp
[DependsOn(typeof(AbpLocationModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Inject and use the location resolution service:

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
        return await _locationProvider.GeocodeAsync(address);
    }

    // Reverse Geocoding: Convert coordinates to address
    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        return await _locationProvider.ReGeocodeAsync(lat, lng);
    }

    // IP Geolocation Resolution
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        return await _locationProvider.IPGeocodeAsync(ipAddress);
    }
}
```

## Advanced Usage

### 1. Distance Calculation

```csharp
// Create location objects
var location1 = new Location { Latitude = 39.9042, Longitude = 116.4074 }; // Beijing
var location2 = new Location { Latitude = 31.2304, Longitude = 121.4737 }; // Shanghai

// Calculate distance between two points (in meters)
double distance = location1.CalcDistance(location2);

// Calculate location offset
var offset = location1.CalcOffset(1000, 45); // Offset 1000 meters to the northeast
```

### 2. Calculate Location Offset Range

```csharp
var location = new Location { Latitude = 39.9042, Longitude = 116.4074 };
// Calculate offset range for specified distance (meters)
var position = Location.CalcOffsetDistance(location, 1000); // 1km range
```

### 3. Custom Location Resolution Provider

```csharp
public class CustomLocationProvider : ILocationResolveProvider
{
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        // Implement IP geolocation resolution
    }

    public async Task<GecodeLocation> GeocodeAsync(string address, string city = null)
    {
        // Implement geocoding
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
    {
        // Implement reverse geocoding
    }
}
```

## Custom Location Resolution Provider Implementation

To implement a custom location resolution provider:

1. Implement the `ILocationResolveProvider` interface:

```csharp
public class CustomLocationProvider : ILocationResolveProvider
{
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        // Implement IP geolocation resolution
    }

    public async Task<GecodeLocation> GeocodeAsync(string address, string city = null)
    {
        // Implement geocoding
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
    {
        // Implement reverse geocoding
    }
}
```

2. Register your implementation in your module:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<ILocationResolveProvider, CustomLocationProvider>();
}
```

## Links

* [中文文档](./README.md)
* [Baidu Maps Location Service](./LINGYUN.Abp.Location.Baidu/README.EN.md)
* [Tencent Maps Location Service](./LINGYUN.Abp.Location.Tencent/README.EN.md)
