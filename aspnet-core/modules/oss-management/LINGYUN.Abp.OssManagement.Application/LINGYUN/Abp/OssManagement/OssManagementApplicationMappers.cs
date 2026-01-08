using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.OssManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OssContainerToOssContainerDtoMapper : MapperBase<OssContainer, OssContainerDto>
{
    public override partial OssContainerDto Map(OssContainer source);
    public override partial void Map(OssContainer source, OssContainerDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OssObjectToOssObjectDtoMapper : MapperBase<OssObject, OssObjectDto>
{
    [MapperIgnoreTarget(nameof(OssObjectDto.Path))]
    [MapperIgnoreSource(nameof(OssObject.Prefix))]
    public override partial OssObjectDto Map(OssObject source);

    [MapperIgnoreTarget(nameof(OssObjectDto.Path))]
    [MapperIgnoreSource(nameof(OssObject.Prefix))]
    public override partial void Map(OssObject source, OssObjectDto destination);

    public override void AfterMap(OssObject source, OssObjectDto destination)
    {
        destination.Path = source.Prefix;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class GetOssContainersResponseToOssContainersResultDtoMapper : MapperBase<GetOssContainersResponse, OssContainersResultDto>
{
    public override partial OssContainersResultDto Map(GetOssContainersResponse source);
    public override partial void Map(GetOssContainersResponse source, OssContainersResultDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class GetOssObjectsResponseToOssObjectsResultDtoMapper : MapperBase<GetOssObjectsResponse, OssObjectsResultDto>
{
    public override partial OssObjectsResultDto Map(GetOssObjectsResponse source);
    public override partial void Map(GetOssObjectsResponse source, OssObjectsResultDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FileShareCacheItemToMyFileShareDtoMapper : MapperBase<FileShareCacheItem, MyFileShareDto>
{
    public override partial MyFileShareDto Map(FileShareCacheItem source);
    public override partial void Map(FileShareCacheItem source, MyFileShareDto destination);
}
