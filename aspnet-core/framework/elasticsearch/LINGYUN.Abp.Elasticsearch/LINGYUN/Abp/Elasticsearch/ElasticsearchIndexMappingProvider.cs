using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch;

public class ElasticsearchIndexMappingProvider : IIndexMappingProvider, ITransientDependency
{
    private readonly IMemoryCache _cache;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public ElasticsearchIndexMappingProvider(
        IElasticsearchClientFactory clientFactory, 
        IMemoryCache cache)
    {
        _clientFactory = clientFactory;
        _cache = cache;
    }

    public async Task<IndexMappingInfo> GetMappingAsync(string indexPattern, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"es_mapping_{indexPattern}";

        var cacheItem = _cache.Get<IndexMappingInfo>(cacheKey);
        if (cacheItem == null)
        {
            var client = _clientFactory.Create();
            var request = new GetMappingRequest(indexPattern)
            {
                IgnoreUnavailable = true,
                AllowNoIndices = true,
                ExpandWildcards = [ExpandWildcard.Open]
            };
            var response = await client.Indices.GetMappingAsync(request, cancellationToken);

            if (response.TryGetErrorMessage(out var errorMessage))
            {
                throw new AbpException($"Failed to get mapping for index {indexPattern}: {errorMessage}");
            }

            var indexName = indexPattern.EndsWith("*") ? indexPattern.Substring(0, indexPattern.Length - 1) : indexPattern;
            var indexMappings = response.GetMappingFor(indexName);
            if (indexMappings == null)
            {
                foreach (var indexMappingRecord in response.Mappings)
                {
                    if (indexMappingRecord.Key.StartsWith(indexName))
                    {
                        indexMappings = indexMappingRecord.Value.Mappings;
                        break;
                    }
                }
                if (indexMappings == null)
                {
                    throw new AbpException($"Index {indexPattern} not found in response");
                }
            }

            cacheItem = ParseMapping(indexMappings, indexPattern);

            _cache.Set(cacheKey, cacheItem, _cacheDuration);
        }

        return cacheItem;
    }

    private IndexMappingInfo ParseMapping(TypeMapping mappings, string indexName)
    {
        var mappingInfo = new IndexMappingInfo { IndexName = indexName };

        if (mappings?.Properties != null)
        {
            ParseProperties(mappings.Properties, mappingInfo, string.Empty);
        }

        return mappingInfo;
    }

    private void ParseProperties(Properties? properties, IndexMappingInfo mappingInfo, string parentPath)
    {
        if (properties == null) return;

        foreach (var kvp in properties)
        {
            var propertyName = kvp.Key.ToString();
            var property = kvp.Value;
            var fullPath = string.IsNullOrEmpty(parentPath)
                ? propertyName
                : $"{parentPath}.{propertyName}";

            var fieldInfo = new FieldMappingInfo
            {
                Path = fullPath,
                Name = propertyName,
                Type = GetPropertyType(property)
            };

            switch (property)
            {
                // Keyword 类型
                case KeywordProperty keyword:
                    fieldInfo.IsKeyword = true;
                    mappingInfo.KeywordFields.Add(fullPath);
                    break;

                // Text 类型 - 包含多字段支持
                case TextProperty text:
                    fieldInfo.IsText = true;
                    mappingInfo.TextFields.Add(fullPath);

                    // 处理 Text 的 Fields（多字段）
                    if (text.Fields != null && text.Fields.Count() > 0)
                    {
                        fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();

                        foreach (var subFieldKvp in text.Fields)
                        {
                            var subFieldName = subFieldKvp.Key.ToString();
                            var subFieldProperty = subFieldKvp.Value;
                            var subFieldPath = $"{fullPath}.{subFieldName}";

                            var subFieldInfo = new FieldMappingInfo
                            {
                                Path = subFieldPath,
                                Name = subFieldName,
                                Type = GetPropertyType(subFieldProperty)
                            };

                            // 处理子字段的类型
                            if (subFieldProperty is KeywordProperty)
                            {
                                subFieldInfo.IsKeyword = true;
                                mappingInfo.KeywordFields.Add(subFieldPath);
                            }
                            else if (subFieldProperty is TextProperty)
                            {
                                subFieldInfo.IsText = true;
                                mappingInfo.TextFields.Add(subFieldPath);
                            }

                            fieldInfo.Properties[subFieldName] = subFieldInfo;
                            mappingInfo.Fields[subFieldPath] = subFieldInfo;
                        }
                    }
                    break;

                // 日期类型
                case DateProperty date:
                    fieldInfo.IsDate = true;
                    fieldInfo.Format = date.Format;
                    mappingInfo.DateFields.Add(fullPath);
                    break;

                // 日期纳秒类型
                case DateNanosProperty dateNanos:
                    fieldInfo.IsDate = true;
                    fieldInfo.Format = dateNanos.Format;
                    mappingInfo.DateFields.Add(fullPath);
                    break;

                // 数值类型
                case ByteNumberProperty:
                case DoubleNumberProperty:
                case FloatNumberProperty:
                case HalfFloatNumberProperty:
                case IntegerNumberProperty:
                case LongNumberProperty:
                case ScaledFloatNumberProperty:
                case ShortNumberProperty:
                case UnsignedLongNumberProperty:
                    fieldInfo.IsNumeric = true;
                    mappingInfo.NumericFields.Add(fullPath);
                    break;

                // 布尔类型
                case BooleanProperty:
                    fieldInfo.IsBoolean = true;
                    mappingInfo.BooleanFields.Add(fullPath);
                    break;

                // Nested 类型
                case NestedProperty nested:
                    fieldInfo.IsNested = true;
                    fieldInfo.IsObject = true;
                    mappingInfo.NestedFieldPaths.Add(fullPath);

                    var nestedInfo = new NestedMappingInfo
                    {
                        Path = fullPath,
                        Name = propertyName,
                        Properties = new Dictionary<string, FieldMappingInfo>()
                    };

                    if (nested.Properties != null)
                    {
                        // 先递归解析内部字段
                        ParseProperties(nested.Properties, mappingInfo, fullPath);

                        // 收集 nested 内部的字段信息
                        foreach (var innerKvp in nested.Properties)
                        {
                            var innerName = innerKvp.Key.ToString();
                            var innerFullPath = $"{fullPath}.{innerName}";

                            if (mappingInfo.Fields.TryGetValue(innerFullPath, out var innerFieldInfo))
                            {
                                nestedInfo.Properties[innerName] = innerFieldInfo;
                            }
                            else
                            {
                                innerFieldInfo = new FieldMappingInfo
                                {
                                    Path = innerFullPath,
                                    Name = innerName,
                                    Type = GetPropertyType(innerKvp.Value)
                                };
                                nestedInfo.Properties[innerName] = innerFieldInfo;
                                mappingInfo.Fields[innerFullPath] = innerFieldInfo;
                            }
                        }
                    }

                    mappingInfo.NestedFields[fullPath] = nestedInfo;
                    break;

                // Object 类型
                case ObjectProperty obj:
                    fieldInfo.IsObject = true;
                    fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();

                    if (obj.Properties != null)
                    {
                        ParseProperties(obj.Properties, mappingInfo, fullPath);
                    }
                    break;

                // 范围类型
                case DateRangeProperty:
                case DoubleRangeProperty:
                case FloatRangeProperty:
                case IntegerRangeProperty:
                case LongRangeProperty:
                case IpRangeProperty:
                    fieldInfo.IsRange = true;
                    break;

                // 其他类型
                case FlattenedProperty:
                    fieldInfo.Type = "flattened";
                    break;

                case GeoPointProperty:
                    fieldInfo.Type = "geo_point";
                    break;

                case GeoShapeProperty:
                    fieldInfo.Type = "geo_shape";
                    break;

                case IpProperty:
                    fieldInfo.Type = "ip";
                    break;

                case VersionProperty:
                    fieldInfo.Type = "version";
                    break;

                case MatchOnlyTextProperty matchOnlyText:
                    fieldInfo.IsText = true;
                    fieldInfo.Type = "match_only_text";
                    mappingInfo.TextFields.Add(fullPath);

                    // MatchOnlyText 也可能有 Fields
                    if (matchOnlyText.Fields != null && matchOnlyText.Fields.Count() > 0)
                    {
                        fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();
                        foreach (var subFieldKvp in matchOnlyText.Fields)
                        {
                            var subFieldName = subFieldKvp.Key.ToString();
                            var subFieldPath = $"{fullPath}.{subFieldName}";
                            var subFieldInfo = new FieldMappingInfo
                            {
                                Path = subFieldPath,
                                Name = subFieldName,
                                Type = GetPropertyType(subFieldKvp.Value)
                            };
                            if (subFieldKvp.Value is KeywordProperty)
                            {
                                subFieldInfo.IsKeyword = true;
                                mappingInfo.KeywordFields.Add(subFieldPath);
                            }
                            fieldInfo.Properties[subFieldName] = subFieldInfo;
                            mappingInfo.Fields[subFieldPath] = subFieldInfo;
                        }
                    }
                    break;

                case WildcardProperty:
                    fieldInfo.IsWildcard = true;
                    fieldInfo.Type = "wildcard";
                    mappingInfo.WildcardFields.Add(fullPath);
                    break;

                case CompletionProperty:
                    fieldInfo.Type = "completion";
                    break;

                case JoinProperty:
                    fieldInfo.Type = "join";
                    break;

                case PercolatorProperty:
                    fieldInfo.Type = "percolator";
                    break;

                case RankFeatureProperty:
                    fieldInfo.Type = "rank_feature";
                    break;

                case RankFeaturesProperty:
                    fieldInfo.Type = "rank_features";
                    break;

                case DenseVectorProperty:
                    fieldInfo.Type = "dense_vector";
                    break;

                case SparseVectorProperty:
                    fieldInfo.Type = "sparse_vector";
                    break;

                default:
                    fieldInfo.Type = property.GetType().Name.Replace("Property", "").ToLowerInvariant();
                    break;
            }

            mappingInfo.Fields[fullPath] = fieldInfo;
        }
    }

    private string GetPropertyType(IProperty property)
    {
        return property switch
        {
            KeywordProperty => "keyword",
            TextProperty => "text",
            DateProperty => "date",
            DateNanosProperty => "date_nanos",
            ByteNumberProperty => "byte",
            DoubleNumberProperty => "double",
            FloatNumberProperty => "float",
            HalfFloatNumberProperty => "half_float",
            IntegerNumberProperty => "integer",
            LongNumberProperty => "long",
            ScaledFloatNumberProperty => "scaled_float",
            ShortNumberProperty => "short",
            UnsignedLongNumberProperty => "unsigned_long",
            BooleanProperty => "boolean",
            NestedProperty => "nested",
            ObjectProperty => "object",
            FlattenedProperty => "flattened",
            GeoPointProperty => "geo_point",
            GeoShapeProperty => "geo_shape",
            IpProperty => "ip",
            VersionProperty => "version",
            MatchOnlyTextProperty => "match_only_text",
            WildcardProperty => "wildcard",
            CompletionProperty => "completion",
            JoinProperty => "join",
            PercolatorProperty => "percolator",
            RankFeatureProperty => "rank_feature",
            RankFeaturesProperty => "rank_features",
            DenseVectorProperty => "dense_vector",
            SparseVectorProperty => "sparse_vector",
            DateRangeProperty => "date_range",
            DoubleRangeProperty => "double_range",
            FloatRangeProperty => "float_range",
            IntegerRangeProperty => "integer_range",
            LongRangeProperty => "long_range",
            IpRangeProperty => "ip_range",
            _ => property.GetType().Name.Replace("Property", "").ToLowerInvariant()
        };
    }
}
