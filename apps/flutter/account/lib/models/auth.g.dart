// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'auth.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PortalLoginProvider _$PortalLoginProviderFromJson(Map<String, dynamic> json) =>
    PortalLoginProvider(
      id: json['Id'] as String,
      name: json['Name'] as String,
      logo: json['Logo'] as String?,
    );

Map<String, dynamic> _$PortalLoginProviderToJson(
        PortalLoginProvider instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'Name': instance.name,
      'Logo': instance.logo,
    };
