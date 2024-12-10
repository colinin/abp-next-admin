# LINGYUN.Abp.Location

## Introduction

`LINGYUN.Abp.Location` is a location service foundation module that provides geographic location-related functionality, including geocoding (forward/reverse), distance calculation, and more.

## Features

* Geocoding and Reverse Geocoding
* Location distance calculation (based on Google algorithm, error <0.2m)
* Location offset calculation
* Support for POI (Points of Interest) and road information
* Extensible location resolution providers

## Installation

```bash
dotnet add package LINGYUN.Abp.Location
```

## Usage

1. Add `[DependsOn(typeof(AbpLocationModule))]` to your module class.

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

    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        return await _locationProvider.GeocodeAsync(address);
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        return await _locationProvider.ReGeocodeAsync(lat, lng);
    }
}
```

## Advanced Usage

### 1. Distance Calculation

```csharp
// Calculate distance between two locations
var location1 = new Location { Latitude = 39.9042, Longitude = 116.4074 }; // Beijing
var location2 = new Location { Latitude = 31.2304, Longitude = 121.4737 }; // Shanghai
double distance = Location.CalcDistance(location1, location2); // Returns distance in meters
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
    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        // Implement geocoding logic
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        // Implement reverse geocoding logic
    }
}
```

## Links

* [中文文档](./README.md)
