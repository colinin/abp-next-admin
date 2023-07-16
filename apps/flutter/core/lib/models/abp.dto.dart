class LocalizableStringInfo {
  LocalizableStringInfo({
    required this.resourceName,
    required this.name,
    this.values,
  });
  String resourceName;
  String name;
  Map<String, dynamic>? values;

  factory LocalizableStringInfo.fromJson(Map<String, dynamic> json) => 
    LocalizableStringInfo(
      resourceName: json['resourceName'] as String,
      name: json['name'] as String,
      values: json['values'] as Map<String, dynamic>?,
    );
  Map<String, dynamic> toJson() => <String, dynamic>{
      'resourceName': resourceName,
      'name': name,
      'values': values,
    };
}


class ExtraPropertyDictionary implements Map<String, dynamic> {
  ExtraPropertyDictionary({
    Map<String, dynamic>? map
  }) {
    _innerMap = map ?? {};
  }
  late Map<String, dynamic> _innerMap = {};
  @override
  operator [](Object? key) {
    return _innerMap[key];
  }

  @override
  void operator []=(String key, value) {
    _innerMap[key] = value;
  }

  @override
  void addAll(Map<String, dynamic> other) {
    _innerMap.addAll(other);
  }

  @override
  void addEntries(Iterable<MapEntry<String, dynamic>> newEntries) {
    _innerMap.addEntries(newEntries);
  }

  @override
  Map<RK, RV> cast<RK, RV>() {
    return _innerMap.cast<RK, RV>();
  }

  @override
  void clear() {
    _innerMap.clear();
  }

  @override
  bool containsKey(Object? key) {
    return _innerMap.containsKey(key);
  }

  @override
  bool containsValue(Object? value) {
    return _innerMap.containsValue(value);
  }

  @override
  Iterable<MapEntry<String, dynamic>> get entries => _innerMap.entries;

  @override
  void forEach(void Function(String key, dynamic value) action) {
    _innerMap.forEach(action);
  }

  @override
  bool get isEmpty => _innerMap.isEmpty;

  @override
  bool get isNotEmpty => _innerMap.isNotEmpty;

  @override
  Iterable<String> get keys => _innerMap.keys;

  @override
  int get length => _innerMap.length;

  @override
  Map<K2, V2> map<K2, V2>(MapEntry<K2, V2> Function(String key, dynamic value) convert) {
    return _innerMap.map<K2, V2>(convert);
  }

  @override
  putIfAbsent(String key, Function() ifAbsent) {
    return _innerMap.putIfAbsent(key, ifAbsent);
  }

  @override
  remove(Object? key) {
    return _innerMap.remove(key);
  }

  @override
  void removeWhere(bool Function(String key, dynamic value) test) {
    _innerMap.removeWhere(test);
  }

  @override
  update(String key, Function(dynamic value) update, {Function()? ifAbsent}) {
    return _innerMap.update(key, update, ifAbsent: ifAbsent);
  }

  @override
  void updateAll(Function(String key, dynamic value) update) {
    _innerMap.updateAll(update);
  }

  @override
  Iterable get values => _innerMap.values;
}

class ExtensibleObject {
  ExtensibleObject({
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}

class EntityDto<TPrimaryKey> {
  EntityDto({
    required this.id,
  });
  TPrimaryKey id;
}

class CreationAuditedEntityDto<TPrimaryKey> extends EntityDto<TPrimaryKey> {
  CreationAuditedEntityDto({
    required super.id,
    required this.creationTime,
    this.creatorId,
  });
  DateTime creationTime;
  String? creatorId;
}

class CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends CreationAuditedEntityDto<TPrimaryKey> {
  CreationAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required super.creationTime,
    super.creatorId,
  });
  TUserDto creator;
}

class AuditedEntityDto<TPrimaryKey> extends CreationAuditedEntityDto<TPrimaryKey> {
  AuditedEntityDto({
    required super.id,
    required super.creationTime,
    super.creatorId,
    this.lastModificationTime,
    this.lastModifierId,
  });
  DateTime? lastModificationTime;
  String? lastModifierId;
}

class AuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends AuditedEntityDto<TPrimaryKey> {
  AuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;
}

class FullAuditedEntityDto<TPrimaryKey> extends AuditedEntityDto<TPrimaryKey> {
  FullAuditedEntityDto({
    required super.id,
    required this.isDeleted,
    this.deleterId,
    this.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  bool isDeleted;
  String? deleterId;
  DateTime? deletionTime;
}

class FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends FullAuditedEntityDto<TPrimaryKey> {
  FullAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required this.deleter,
    required super.isDeleted,
    super.deleterId,
    super.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;
  TUserDto deleter;
}

class ExtensibleEntityDto<TKey> extends ExtensibleObject {
  ExtensibleEntityDto({
    required this.id,
  });
  TKey id;
}

class ExtensibleCreationAuditedEntityDto<TPrimaryKey> extends ExtensibleEntityDto<TPrimaryKey> {
  ExtensibleCreationAuditedEntityDto({
    required super.id,
    required this.creationTime,
    this.creatorId,
  });
  DateTime creationTime;
  String? creatorId;
}

class ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  ExtensibleCreationAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required super.creationTime,
    super.creatorId,
  });
  TUserDto creator;
}

class ExtensibleAuditedEntityDto<TPrimaryKey> extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  ExtensibleAuditedEntityDto({
    required super.id,
    required super.creationTime,
    super.creatorId,
    this.lastModificationTime,
    this.lastModifierId,
  });
  DateTime? lastModificationTime;
  String? lastModifierId;
}

class ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  ExtensibleAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;
}

class ExtensibleFullAuditedEntityDto<TPrimaryKey> extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  ExtensibleFullAuditedEntityDto({
    required super.id,
    required this.isDeleted,
    this.deleterId,
    this.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  bool isDeleted;
  String? deleterId;
  DateTime? deletionTime;
}

class ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  ExtensibleFullAuditedEntityWithUserDto({
    required super.id,
    required this.creator,
    required this.lastModifier,
    required this.deleter,
    required super.isDeleted,
    super.deleterId,
    super.deletionTime,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
  });
  TUserDto creator;
  TUserDto lastModifier;
  TUserDto deleter;
}

class ListResultDto<T> {
  ListResultDto({
    required this.items,
  });
  List<T> items;
}

class LimitedResultRequestDto {
  LimitedResultRequestDto({
    this.maxResultCount,
  });
  int? maxResultCount = 10;
}

class ExtensibleLimitedResultRequestDto extends LimitedResultRequestDto {
  ExtensibleLimitedResultRequestDto({
    super.maxResultCount,
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}

class ExtensibleListResultDto<T> extends ListResultDto<T> {
  ExtensibleListResultDto({
    required super.items,
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}

class PagedResultDto<T> extends ListResultDto<T> {
  PagedResultDto({
    required this.totalCount,
    required super.items,
  });
  int totalCount;
}

class ExtensiblePagedResultDto<T> extends PagedResultDto<T> {
  ExtensiblePagedResultDto({
    required super.totalCount,
    required super.items,
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}

class SortedResultRequest {
  SortedResultRequest({
    this.sorting,
  });
  String? sorting;
}

class PagedResultRequestDto extends LimitedResultRequestDto {
  PagedResultRequestDto({
    this.skipCount,
    super.maxResultCount,
  });
  int? skipCount = 0;
}

class PagedAndSortedResultRequestDto extends PagedResultRequestDto {
  PagedAndSortedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    this.sorting,
  });
  String? sorting;
}

class ExtensiblePagedAndSortedResultRequestDto extends PagedAndSortedResultRequestDto {
  ExtensiblePagedAndSortedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    super.sorting,
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}

class ExtensiblePagedResultRequestDto extends PagedResultRequestDto {
  ExtensiblePagedResultRequestDto({
    super.skipCount,
    super.maxResultCount,
    this.extraProperties,
  });
  ExtraPropertyDictionary? extraProperties;
}
