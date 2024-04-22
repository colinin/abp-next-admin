import 'package:core/index.dart';
import 'package:json_annotation/json_annotation.dart';

part 'menu.dto.g.dart';

abstract class RouteDto extends EntityDto<String> {
  RouteDto({
    required super.id,
    required this.path,
    required this.name,
    required this.displayName,
    this.description = '',
    this.meta = const {},
  });
  String path;
  String name;
  String displayName;
  String? description;
  String? redirect;
  Map<String, dynamic>? meta;
}

@JsonSerializable()
class MenuDto extends RouteDto {
  MenuDto({
    required super.id, 
    required super.path, 
    required super.name, 
    required super.displayName,
    required this.code,
    required this.component,
    required this.framework,
    required this.layoutId,
    this.parentId,
    this.isPublic = false,
    this.startup = false,
    super.description = '',
    super.meta = const {},
  });
  String code;
  String component;
  String framework;
  String? parentId;
  String layoutId;
  bool isPublic;
  bool startup;

  factory MenuDto.fromJson(Map<String, dynamic> json) => _$MenuDtoFromJson(json);

  @override
  Map<String, dynamic> toJson({ Object? Function(String value)? toJsonTPrimaryKey }) => _$MenuDtoToJson(this);
}

@JsonSerializable()
class UserFavoriteMenuDto extends AuditedEntityDto<String> {
  UserFavoriteMenuDto({
    required super.id,
    required super.creationTime,
    super.creatorId,
    super.lastModificationTime,
    super.lastModifierId,
    required this.menuId,
    required this.userId,
    this.framework,
    required this.name,
    required this.path,
    this.aliasName,
    this.icon,
    this.color,
    this.displayName,
  });
  String menuId;
  String userId;
  String? framework;
  String name;
  String path;
  String? aliasName;
  String? icon;
  String? color;
  String? displayName;

  factory UserFavoriteMenuDto.fromJson(Map<String, dynamic> json) => _$UserFavoriteMenuDtoFromJson(json);

  @override
  Map<String, dynamic> toJson({ Object? Function(String value)? toJsonTPrimaryKey }) => _$UserFavoriteMenuDtoToJson(this);
}

abstract class UserFavoriteMenuCreateOrUpdateDto {
  UserFavoriteMenuCreateOrUpdateDto({
    required this.menuId,
    this.color,
    this.aliasName,
    this.icon,
  });
  String menuId;
  String? color;
  String? aliasName;
  String? icon;  
}

@JsonSerializable()
class UserFavoriteMenuCreateDto extends UserFavoriteMenuCreateOrUpdateDto {
  UserFavoriteMenuCreateDto({
    required super.menuId,
    this.framework,
    super.color,
    super.aliasName,
    super.icon,
  });
  String? framework;

  factory UserFavoriteMenuCreateDto.fromJson(Map<String, dynamic> json) => _$UserFavoriteMenuCreateDtoFromJson(json);

  Map<String, dynamic> toJson() => _$UserFavoriteMenuCreateDtoToJson(this);
}

@JsonSerializable()
class UserFavoriteMenuUpdateDto extends UserFavoriteMenuCreateOrUpdateDto implements IHasConcurrencyStamp {
  UserFavoriteMenuUpdateDto({
    required super.menuId,
    super.color,
    super.aliasName,
    super.icon,
    this.concurrencyStamp,
  });
  @override
  String? concurrencyStamp;

  factory UserFavoriteMenuUpdateDto.fromJson(Map<String, dynamic> json) => _$UserFavoriteMenuUpdateDtoFromJson(json);

  Map<String, dynamic> toJson() => _$UserFavoriteMenuUpdateDtoToJson(this);
}