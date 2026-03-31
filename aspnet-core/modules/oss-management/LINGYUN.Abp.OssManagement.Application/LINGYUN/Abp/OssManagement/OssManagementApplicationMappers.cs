using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.OssManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OssContainerToOssContainerDtoMapper : MapperBase<OssContainer, OssContainerDto>
{
    public override partial OssContainerDto Map(OssContainer source);
    public override partial void Map(OssContainer source, OssContainerDto destination);
}

[Mapper]
public partial class OssObjectToOssObjectDtoMapper : MapperBase<OssObject, OssObjectDto>
{
    [MapperIgnoreSource(nameof(OssObject.Content))]
    [MapperIgnoreSource(nameof(OssObject.FullName))]
    [MapProperty(nameof(OssObject.Prefix), nameof(OssObjectDto.Path))]
    public override partial OssObjectDto Map(OssObject source);

    [MapperIgnoreSource(nameof(OssObject.Content))]
    [MapperIgnoreSource(nameof(OssObject.FullName))]
    [MapProperty(nameof(OssObject.Prefix), nameof(OssObjectDto.Path))]
    public override partial void Map(OssObject source, OssObjectDto destination);
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
    private readonly OssObjectToOssObjectDtoMapper _ossObjectMapper;

    public GetOssObjectsResponseToOssObjectsResultDtoMapper(OssObjectToOssObjectDtoMapper ossObjectMapper)
    {
        _ossObjectMapper = ossObjectMapper;
    }

    [MapperIgnoreTarget(nameof(GetOssObjectsResponse.Objects))]
    public override partial OssObjectsResultDto Map(GetOssObjectsResponse source);

    [MapperIgnoreTarget(nameof(GetOssObjectsResponse.Objects))]
    public override partial void Map(GetOssObjectsResponse source, OssObjectsResultDto destination);

    public override void AfterMap(GetOssObjectsResponse source, OssObjectsResultDto destination)
    {
        if (source.Objects != null)
        {
            destination.Objects ??= new List<OssObjectDto>();
            destination.Objects.Clear();

            foreach (var ossObject in source.Objects)
            {
                var ossObjectDto = _ossObjectMapper.Map(ossObject);
                _ossObjectMapper.AfterMap(ossObject, ossObjectDto);
                destination.Objects.Add(ossObjectDto);
            }
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FileShareCacheItemToMyFileShareDtoMapper : MapperBase<FileShareCacheItem, MyFileShareDto>
{
    public override partial MyFileShareDto Map(FileShareCacheItem source);
    public override partial void Map(FileShareCacheItem source, MyFileShareDto destination);
}
