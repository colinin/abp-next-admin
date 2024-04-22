// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'menu.dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MenuDto _$MenuDtoFromJson(Map<String, dynamic> json) => MenuDto(
      id: json['id'] as String,
      path: json['path'] as String,
      name: json['name'] as String,
      displayName: json['displayName'] as String,
      code: json['code'] as String,
      component: json['component'] as String,
      framework: json['framework'] as String,
      layoutId: json['layoutId'] as String,
      parentId: json['parentId'] as String?,
      isPublic: json['isPublic'] as bool? ?? false,
      startup: json['startup'] as bool? ?? false,
      description: json['description'] as String? ?? '',
      meta: json['meta'] as Map<String, dynamic>? ?? const {},
    )..redirect = json['redirect'] as String?;

Map<String, dynamic> _$MenuDtoToJson(MenuDto instance) => <String, dynamic>{
      'id': instance.id,
      'path': instance.path,
      'name': instance.name,
      'displayName': instance.displayName,
      'description': instance.description,
      'redirect': instance.redirect,
      'meta': instance.meta,
      'code': instance.code,
      'component': instance.component,
      'framework': instance.framework,
      'parentId': instance.parentId,
      'layoutId': instance.layoutId,
      'isPublic': instance.isPublic,
      'startup': instance.startup,
    };

UserFavoriteMenuDto _$UserFavoriteMenuDtoFromJson(Map<String, dynamic> json) =>
    UserFavoriteMenuDto(
      id: json['id'] as String,
      creationTime: DateTime.parse(json['creationTime'] as String),
      creatorId: json['creatorId'] as String?,
      lastModificationTime: json['lastModificationTime'] == null
          ? null
          : DateTime.parse(json['lastModificationTime'] as String),
      lastModifierId: json['lastModifierId'] as String?,
      menuId: json['menuId'] as String,
      userId: json['userId'] as String,
      framework: json['framework'] as String?,
      name: json['name'] as String,
      path: json['path'] as String,
      aliasName: json['aliasName'] as String?,
      icon: json['icon'] as String?,
      color: json['color'] as String?,
      displayName: json['displayName'] as String?,
    );

Map<String, dynamic> _$UserFavoriteMenuDtoToJson(
        UserFavoriteMenuDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'creationTime': instance.creationTime.toIso8601String(),
      'creatorId': instance.creatorId,
      'lastModificationTime': instance.lastModificationTime?.toIso8601String(),
      'lastModifierId': instance.lastModifierId,
      'menuId': instance.menuId,
      'userId': instance.userId,
      'framework': instance.framework,
      'name': instance.name,
      'path': instance.path,
      'aliasName': instance.aliasName,
      'icon': instance.icon,
      'color': instance.color,
      'displayName': instance.displayName,
    };

UserFavoriteMenuCreateDto _$UserFavoriteMenuCreateDtoFromJson(
        Map<String, dynamic> json) =>
    UserFavoriteMenuCreateDto(
      menuId: json['menuId'] as String,
      framework: json['framework'] as String?,
      color: json['color'] as String?,
      aliasName: json['aliasName'] as String?,
      icon: json['icon'] as String?,
    );

Map<String, dynamic> _$UserFavoriteMenuCreateDtoToJson(
        UserFavoriteMenuCreateDto instance) =>
    <String, dynamic>{
      'menuId': instance.menuId,
      'color': instance.color,
      'aliasName': instance.aliasName,
      'icon': instance.icon,
      'framework': instance.framework,
    };

UserFavoriteMenuUpdateDto _$UserFavoriteMenuUpdateDtoFromJson(
        Map<String, dynamic> json) =>
    UserFavoriteMenuUpdateDto(
      menuId: json['menuId'] as String,
      color: json['color'] as String?,
      aliasName: json['aliasName'] as String?,
      icon: json['icon'] as String?,
      concurrencyStamp: json['concurrencyStamp'] as String?,
    );

Map<String, dynamic> _$UserFavoriteMenuUpdateDtoToJson(
        UserFavoriteMenuUpdateDto instance) =>
    <String, dynamic>{
      'menuId': instance.menuId,
      'color': instance.color,
      'aliasName': instance.aliasName,
      'icon': instance.icon,
      'concurrencyStamp': instance.concurrencyStamp,
    };
