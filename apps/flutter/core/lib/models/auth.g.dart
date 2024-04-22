// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'auth.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

PortalTokenRequest _$PortalTokenRequestFromJson(Map<String, dynamic> json) =>
    PortalTokenRequest(
      clientId: json['client_id'],
      clientSecret: json['client_secret'],
      userName: json['username'] as String,
      password: json['password'] as String,
      enterpriseId: json['enterpriseId'] as String?,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$PortalTokenRequestToJson(PortalTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'username': instance.userName,
      'password': instance.password,
      'enterpriseId': instance.enterpriseId,
    };

TwoFactorPhoneNumberTokenRequest _$TwoFactorPhoneNumberTokenRequestFromJson(
        Map<String, dynamic> json) =>
    TwoFactorPhoneNumberTokenRequest(
      clientId: json['client_id'],
      clientSecret: json['client_secret'],
      phoneNumber: json['phone_number'] as String,
      code: json['phone_verify_code'] as String,
      twoFactorProvider: json['twoFactorProvider'] as String,
      twoFactorCode: json['twoFactorCode'] as String,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$TwoFactorPhoneNumberTokenRequestToJson(
        TwoFactorPhoneNumberTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'phone_number': instance.phoneNumber,
      'phone_verify_code': instance.code,
      'twoFactorProvider': instance.twoFactorProvider,
      'twoFactorCode': instance.twoFactorCode,
    };

PhoneNumberTokenRequest _$PhoneNumberTokenRequestFromJson(
        Map<String, dynamic> json) =>
    PhoneNumberTokenRequest(
      clientId: json['client_id'],
      clientSecret: json['client_secret'],
      phoneNumber: json['phone_number'] as String,
      code: json['phone_verify_code'] as String,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$PhoneNumberTokenRequestToJson(
        PhoneNumberTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'phone_number': instance.phoneNumber,
      'phone_verify_code': instance.code,
    };

TwoFactorPasswordTokenRequest _$TwoFactorPasswordTokenRequestFromJson(
        Map<String, dynamic> json) =>
    TwoFactorPasswordTokenRequest(
      clientId: json['client_id'],
      clientSecret: json['client_secret'],
      userName: json['username'] as String,
      password: json['password'] as String,
      twoFactorProvider: json['twoFactorProvider'] as String,
      twoFactorCode: json['twoFactorCode'] as String,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$TwoFactorPasswordTokenRequestToJson(
        TwoFactorPasswordTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'username': instance.userName,
      'password': instance.password,
      'twoFactorProvider': instance.twoFactorProvider,
      'twoFactorCode': instance.twoFactorCode,
    };

PasswordTokenRequest _$PasswordTokenRequestFromJson(
        Map<String, dynamic> json) =>
    PasswordTokenRequest(
      clientId: json['client_id'],
      clientSecret: json['client_secret'],
      userName: json['username'] as String,
      password: json['password'] as String,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$PasswordTokenRequestToJson(
        PasswordTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'username': instance.userName,
      'password': instance.password,
    };

RefreshTokenRequest _$RefreshTokenRequestFromJson(Map<String, dynamic> json) =>
    RefreshTokenRequest(
      clientId: json['client_id'] as String,
      clientSecret: json['client_secret'] as String?,
      refreshToken: json['refresh_token'] as String,
    )..grantType = json['grant_type'] as String;

Map<String, dynamic> _$RefreshTokenRequestToJson(
        RefreshTokenRequest instance) =>
    <String, dynamic>{
      'grant_type': instance.grantType,
      'client_id': instance.clientId,
      'client_secret': instance.clientSecret,
      'refresh_token': instance.refreshToken,
    };

Token _$TokenFromJson(Map<String, dynamic> json) => Token(
      accessToken: json['access_token'] as String,
      expiresIn: json['expires_in'] as int?,
      expiration: json['expiration'] == null
        ? null
        : DateTime.tryParse(json['expiration'] as String),
      tokenType: json['token_type'] as String?,
      refreshToken: json['refresh_token'] as String?,
      scope: json['scope'] as String?,
    );

Map<String, dynamic> _$TokenToJson(Token instance) => <String, dynamic>{
      'access_token': instance.accessToken,
      'expires_in': instance.expiresIn,
      'expiration': instance.expiration?.toString(),
      'token_type': instance.tokenType,
      'refresh_token': instance.refreshToken,
      'scope': instance.scope,
    };

UserProfile _$UserProfileFromJson(Map<String, dynamic> json) => UserProfile(
      id: json['sub'] as String,
      userName: json['name'] as String,
      avatarUrl: json['avatarUrl'] as String?,
      name: json['given_name'] as String?,
      surName: json['family_name'] as String?,
      email: json['email'] as String?,
      emailVerified: json['email_verified'] as String?,
      phoneNumber: json['phoneNumber'] as String?,
      phoneNumberVerified: json['phone_number_verified'] as String?,
    );

Map<String, dynamic> _$UserProfileToJson(UserProfile instance) =>
    <String, dynamic>{
      'sub': instance.id,
      'name': instance.userName,
      'avatarUrl': instance.avatarUrl,
      'given_name': instance.name,
      'family_name': instance.surName,
      'email': instance.email,
      'email_verified': instance.emailVerified,
      'phoneNumber': instance.phoneNumber,
      'phone_number_verified': instance.phoneNumberVerified,
    };
