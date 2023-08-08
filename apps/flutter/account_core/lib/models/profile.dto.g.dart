// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'profile.dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ProfileDto _$ProfileDtoFromJson(Map<String, dynamic> json) => ProfileDto(
      userName: json['userName'] as String?,
      email: json['email'] as String?,
      name: json['name'] as String?,
      surname: json['surname'] as String?,
      phoneNumber: json['phoneNumber'] as String?,
      concurrencyStamp: json['concurrencyStamp'] as String?,
      isExternal: json['isExternal'] as bool? ?? false,
      hasPassword: json['hasPassword'] as bool? ?? false,
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$ProfileDtoToJson(ProfileDto instance) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'userName': instance.userName,
      'email': instance.email,
      'name': instance.name,
      'surname': instance.surname,
      'phoneNumber': instance.phoneNumber,
      'isExternal': instance.isExternal,
      'hasPassword': instance.hasPassword,
      'concurrencyStamp': instance.concurrencyStamp,
    };

ChangePasswordInput _$ChangePasswordInputFromJson(Map<String, dynamic> json) =>
    ChangePasswordInput(
      currentPassword: json['currentPassword'] as String?,
      newPassword: json['newPassword'] as String,
    );

Map<String, dynamic> _$ChangePasswordInputToJson(
        ChangePasswordInput instance) =>
    <String, dynamic>{
      'currentPassword': instance.currentPassword,
      'newPassword': instance.newPassword,
    };

RegisterDto _$RegisterDtoFromJson(Map<String, dynamic> json) => RegisterDto(
      userName: json['userName'] as String,
      emailAddress: json['emailAddress'] as String,
      password: json['password'] as String,
      appName: json['appName'] as String? ?? 'abp-flutter',
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$RegisterDtoToJson(RegisterDto instance) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'userName': instance.userName,
      'emailAddress': instance.emailAddress,
      'password': instance.password,
      'appName': instance.appName,
    };

UpdateProfileDto _$UpdateProfileDtoFromJson(Map<String, dynamic> json) =>
    UpdateProfileDto(
      userName: json['userName'] as String?,
      email: json['email'] as String?,
      name: json['name'] as String?,
      surname: json['surname'] as String?,
      phoneNumber: json['phoneNumber'] as String?,
      concurrencyStamp: json['concurrencyStamp'] as String?,
      extraProperties: json['extraProperties'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$UpdateProfileDtoToJson(UpdateProfileDto instance) =>
    <String, dynamic>{
      'extraProperties': instance.extraProperties,
      'userName': instance.userName,
      'email': instance.email,
      'name': instance.name,
      'surname': instance.surname,
      'phoneNumber': instance.phoneNumber,
      'concurrencyStamp': instance.concurrencyStamp,
    };
