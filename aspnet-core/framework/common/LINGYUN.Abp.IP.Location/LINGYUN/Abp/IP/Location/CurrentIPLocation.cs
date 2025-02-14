using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IP.Location;
public class CurrentIPLocation : ICurrentIPLocation, ITransientDependency
{
    public string? Country => _currentIPLocationAccessor.Current?.Country;

    public string? Province => _currentIPLocationAccessor.Current?.Province;

    public string? City => _currentIPLocationAccessor.Current?.City;

    public string? Remarks => _currentIPLocationAccessor.Current?.Remarks;


    private readonly ICurrentIPLocationAccessor _currentIPLocationAccessor;

    public CurrentIPLocation(ICurrentIPLocationAccessor currentIPLocationAccessor)
    {
        _currentIPLocationAccessor = currentIPLocationAccessor;
    }

    public IDisposable Change(IPLocation? location = null)
    {
        return SetCurrent(location);
    }

    private IDisposable SetCurrent(IPLocation? location = null)
    {
        var parentScope = _currentIPLocationAccessor.Current;
        _currentIPLocationAccessor.Current = location;

        return new DisposeAction<ValueTuple<ICurrentIPLocationAccessor, IPLocation?>>(static (state) =>
        {
            var (currentIPLocationAccessor, parentScope) = state;
            currentIPLocationAccessor.Current = parentScope;
        }, (_currentIPLocationAccessor, parentScope));
    }
}
