using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
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

    public async virtual Task<IndexMappingInfo> GetMappingAsync<TDocument>(
        string indexPattern,
        CancellationToken cancellationToken = default)
    {
        return await GetMappingAsync(indexPattern, typeof(TDocument), cancellationToken);
    }

    public async virtual Task<IndexMappingInfo> GetMappingAsync(string indexPattern, CancellationToken cancellationToken = default)
    {
        return await GetMappingAsync(indexPattern, null, cancellationToken);
    }

    private async Task<IndexMappingInfo> GetMappingAsync(
        string indexPattern,
        Type? documentType,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = documentType == null
            ? $"es_mapping_{indexPattern}"
            : $"es_mapping_{indexPattern}_{documentType.FullName}";

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

            var indexName = indexPattern.EndsWith("*")
                ? indexPattern.Substring(0, indexPattern.Length - 1)
                : indexPattern;

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

            cacheItem = ParseMapping(indexMappings, indexPattern, documentType);

            _cache.Set(cacheKey, cacheItem, _cacheDuration);
        }

        return cacheItem;
    }

    private static IndexMappingInfo ParseMapping(
        TypeMapping mappings,
        string indexName,
        Type? documentType = null)
    {
        var mappingInfo = new IndexMappingInfo
        {
            IndexName = indexName,
            DocumentType = documentType
        };

        if (mappings?.Properties != null)
        {
            ParseProperties(
                mappings.Properties,
                mappingInfo,
                string.Empty,
                documentType,
                string.Empty);
        }

        return mappingInfo;
    }

    private static void ParseProperties(
        Properties? properties,
        IndexMappingInfo mappingInfo,
        string esParentPath,
        Type? parentClrType,
        string clrParentPath)
    {
        if (properties == null)
        {
            return;
        }

        foreach (var kvp in properties)
        {
            var propertyName = kvp.Key.ToString();
            var property = kvp.Value;

            // ES 完整路径
            var esFullPath = string.IsNullOrEmpty(esParentPath)
                ? propertyName
                : $"{esParentPath}.{propertyName}";

            // CLR 完整路径
            var clrFullPath = string.IsNullOrEmpty(clrParentPath)
                ? propertyName
                : $"{clrParentPath}.{propertyName}";

            // 解析 CLR 类型
            var clrType = ResolveClrType(parentClrType, propertyName);


            var fieldInfo = new FieldMappingInfo
            {
                Path = esFullPath,
                Name = propertyName,
                Type = GetPropertyType(property),
                ClrType = clrType,
                ClrPath = clrFullPath
            };

            switch (property)
            {
                // Keyword 类型
                case KeywordProperty keyword:
                    fieldInfo.IsKeyword = true;
                    mappingInfo.KeywordFields.Add(esFullPath);
                    break;

                // Text 类型 - 包含多字段支持
                case TextProperty text:
                    fieldInfo.IsText = true;
                    mappingInfo.TextFields.Add(esFullPath);

                    // 处理 Text 的 Fields（多字段）
                    if (text.Fields != null && text.Fields.Count() > 0)
                    {
                        fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();

                        foreach (var subFieldKvp in text.Fields)
                        {
                            var subFieldName = subFieldKvp.Key.ToString();
                            var subFieldProperty = subFieldKvp.Value;
                            var subFieldEsPath = $"{esFullPath}.{subFieldName}";
                            var subFieldClrPath = $"{clrFullPath}.{subFieldName}";

                            var subFieldInfo = new FieldMappingInfo
                            {
                                Path = subFieldEsPath,
                                Name = subFieldName,
                                Type = GetPropertyType(subFieldProperty),
                                ClrType = clrType,
                                ClrPath = subFieldClrPath,
                                IsMultiField = true
                            };

                            // 处理子字段的类型
                            if (subFieldProperty is KeywordProperty)
                            {
                                subFieldInfo.IsKeyword = true;
                                mappingInfo.KeywordFields.Add(subFieldEsPath);
                            }
                            else if (subFieldProperty is TextProperty)
                            {
                                subFieldInfo.IsText = true;
                                mappingInfo.TextFields.Add(subFieldEsPath);
                            }

                            fieldInfo.Properties[subFieldName] = subFieldInfo;
                            mappingInfo.Fields[subFieldEsPath] = subFieldInfo;
                            mappingInfo.ClrFields[subFieldClrPath] = subFieldInfo;
                        }
                    }
                    break;

                // 日期类型
                case DateProperty date:
                    fieldInfo.IsDate = true;
                    fieldInfo.Format = date.Format;
                    mappingInfo.DateFields.Add(esFullPath);
                    break;

                // 日期纳秒类型
                case DateNanosProperty dateNanos:
                    fieldInfo.IsDate = true;
                    fieldInfo.Format = dateNanos.Format;
                    mappingInfo.DateFields.Add(esFullPath);
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
                    mappingInfo.NumericFields.Add(esFullPath);
                    break;

                // 布尔类型
                case BooleanProperty:
                    fieldInfo.IsBoolean = true;
                    mappingInfo.BooleanFields.Add(esFullPath);
                    break;

                // Nested 类型
                case NestedProperty nested:
                    fieldInfo.IsNested = true;
                    fieldInfo.IsObject = true;
                    mappingInfo.NestedFieldPaths.Add(esFullPath);

                    var nestedInfo = new NestedMappingInfo
                    {
                        Path = esFullPath,
                        Name = propertyName,
                        Properties = new Dictionary<string, FieldMappingInfo>()
                    };

                    // 获取 nested 集合的元素类型
                    var elementType = GetElementType(clrType);

                    if (nested.Properties != null)
                    {
                        // 先递归解析内部字段
                        ParseProperties(
                            nested.Properties,
                            mappingInfo,
                            esFullPath,
                            elementType ?? clrType,
                            clrFullPath);

                        // 收集 nested 内部的字段信息
                        foreach (var innerKvp in nested.Properties)
                        {
                            var innerName = innerKvp.Key.ToString();
                            var innerEsFullPath = $"{esFullPath}.{innerName}";
                            var innerClrFullPath = $"{clrFullPath}.{innerName}";

                            if (mappingInfo.Fields.TryGetValue(innerEsFullPath, out var innerFieldInfo))
                            {
                                nestedInfo.Properties[innerName] = innerFieldInfo;
                            }
                            else
                            {
                                var innerClrType = ResolveClrType(elementType ?? clrType, innerName);
                                innerFieldInfo = new FieldMappingInfo
                                {
                                    Path = innerEsFullPath,
                                    Name = innerName,
                                    Type = GetPropertyType(innerKvp.Value),
                                    ClrType = innerClrType,
                                    ClrPath = innerClrFullPath
                                };
                                nestedInfo.Properties[innerName] = innerFieldInfo;
                                mappingInfo.Fields[innerEsFullPath] = innerFieldInfo;
                                mappingInfo.ClrFields[innerClrFullPath] = innerFieldInfo;
                            }
                        }
                    }

                    mappingInfo.NestedFields[esFullPath] = nestedInfo;
                    break;

                // Object 类型
                case ObjectProperty obj:
                    fieldInfo.IsObject = true;
                    fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();

                    if (obj.Properties != null)
                    {
                        ParseProperties(
                            obj.Properties,
                            mappingInfo,
                            esFullPath,
                            clrType,
                            clrFullPath);
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
                    mappingInfo.TextFields.Add(esFullPath);

                    // MatchOnlyText 也可能有 Fields
                    if (matchOnlyText.Fields != null && matchOnlyText.Fields.Count() > 0)
                    {
                        fieldInfo.Properties = new Dictionary<string, FieldMappingInfo>();
                        foreach (var subFieldKvp in matchOnlyText.Fields)
                        {
                            var subFieldName = subFieldKvp.Key.ToString();
                            var subFieldEsPath = $"{esFullPath}.{subFieldName}";
                            var subFieldClrPath = $"{clrFullPath}.{subFieldName}";
                            var subFieldInfo = new FieldMappingInfo
                            {
                                Path = subFieldEsPath,
                                Name = subFieldName,
                                Type = GetPropertyType(subFieldKvp.Value),
                                ClrType = clrType,
                                ClrPath = subFieldClrPath,
                                IsMultiField = true
                            };
                            if (subFieldKvp.Value is KeywordProperty)
                            {
                                subFieldInfo.IsKeyword = true;
                                mappingInfo.KeywordFields.Add(subFieldEsPath);
                            }
                            fieldInfo.Properties[subFieldName] = subFieldInfo;
                            mappingInfo.Fields[subFieldEsPath] = subFieldInfo;
                            mappingInfo.ClrFields[subFieldClrPath] = subFieldInfo;
                        }
                    }
                    break;

                case WildcardProperty:
                    fieldInfo.IsWildcard = true;
                    fieldInfo.Type = "wildcard";
                    mappingInfo.WildcardFields.Add(esFullPath);
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

            // 添加到映射集合
            mappingInfo.Fields[esFullPath] = fieldInfo;

            // 只有在有 CLR 类型信息时才添加到 CLR 字段集合
            if (clrType != null || parentClrType != null)
            {
                mappingInfo.ClrFields[clrFullPath] = fieldInfo;
            }
        }
    }

    private static Type? ResolveClrType(Type? parentType, string propertyName)
    {
        if (parentType == null)
        {
            return null;
        }

        var propertyInfo = parentType.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return propertyInfo?.PropertyType;
    }

    private static Type? GetElementType(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        // 处理数组
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        // 处理 IEnumerable<T>
        if (type.IsGenericType)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(IEnumerable<>) ||
                genericTypeDefinition == typeof(ICollection<>) ||
                genericTypeDefinition == typeof(IList<>) ||
                genericTypeDefinition == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }
        }

        // 处理实现了 IEnumerable<T> 的接口
        var enumerableInterface = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        return enumerableInterface?.GetGenericArguments()[0];
    }

    private static string GetJsonPropertyName(Type? type, string propertyName)
    {
        if (type == null)
        {
            return propertyName;
        }

        var propertyInfo = type.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (propertyInfo == null)
        {
            return propertyName;
        }

        // 检查 JsonPropertyName 特性
        var jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
        if (jsonPropertyNameAttribute != null && !string.IsNullOrEmpty(jsonPropertyNameAttribute.Name))
        {
            return jsonPropertyNameAttribute.Name;
        }

        return propertyName;
    }

    private static string GetPropertyType(IProperty property)
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
