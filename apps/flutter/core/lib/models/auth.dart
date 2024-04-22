import 'package:json_annotation/json_annotation.dart';

part 'auth.g.dart';

/// 企业平台认证请求承载体
@JsonSerializable()
class PortalTokenRequest extends TokenRequest {
  PortalTokenRequest({
    required clientId,
    clientSecret,
    required this.userName,
    required this.password,
    this.enterpriseId,
  }) : super(
    grantType: 'portal',
    clientId: clientId,
    clientSecret: clientSecret,
  );
  /// 用户名
  @JsonKey(name: 'username')
  String userName;
  /// 密码
  @JsonKey(name: 'password')
  String password;
  /// 双因素认证提供者
  @JsonKey(name: 'enterpriseId')
  String? enterpriseId;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&enterpriseId=${enterpriseId ?? ''}&username=$userName&password=$password';
  }

  factory PortalTokenRequest.fromJson(Map<String, dynamic> json) => _$PortalTokenRequestFromJson(json);
  
  Map<String, dynamic> toJson() => _$PortalTokenRequestToJson(this);
}

/// 双因素身份认证手机认证请求承载体
@JsonSerializable()
class TwoFactorPhoneNumberTokenRequest extends PhoneNumberTokenRequest {
  TwoFactorPhoneNumberTokenRequest({
    required super.clientId,
    super.clientSecret,
    required super.phoneNumber,
    required super.code,
    required this.twoFactorProvider,
    required this.twoFactorCode,
  });
  /// 双因素认证提供者
  @JsonKey(name: 'twoFactorProvider')
  String twoFactorProvider;
  /// 双因素认证验证码
  @JsonKey(name: 'twoFactorCode')
  String twoFactorCode;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&phone_number=$phoneNumber&phone_verify_code=$code&twoFactorProvider=twoFactorProvider&twoFactorCode=$twoFactorCode';
  }

  factory TwoFactorPhoneNumberTokenRequest.fromJson(Map<String, dynamic> json) => _$TwoFactorPhoneNumberTokenRequestFromJson(json);
  
  @override
  Map<String, dynamic> toJson() => _$TwoFactorPhoneNumberTokenRequestToJson(this);
}

/// 身份认证手机验证请求承载体
@JsonSerializable()
class PhoneNumberTokenRequest extends TokenRequest {
  PhoneNumberTokenRequest({
    required clientId,
    clientSecret,
    required this.phoneNumber,
    required this.code,
  }) : super(grantType: 'phone_verify', clientId: clientId, clientSecret: clientSecret);
  /// 手机号
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'phone_number')
  late String phoneNumber;
  /// 验证码
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'phone_verify_code')
  late String code;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&phone_number=$phoneNumber&phone_verify_code=$code';
  }

  factory PhoneNumberTokenRequest.fromJson(Map<String, dynamic> json) => _$PhoneNumberTokenRequestFromJson(json);
  Map<String, dynamic> toJson() => _$PhoneNumberTokenRequestToJson(this);
}

/// 双因素身份认证密码认证请求承载体
@JsonSerializable()
class TwoFactorPasswordTokenRequest extends PasswordTokenRequest {
  TwoFactorPasswordTokenRequest({
    required super.clientId,
    super.clientSecret,
    required super.userName,
    required super.password,
    required this.twoFactorProvider,
    required this.twoFactorCode,
  });
  /// 双因素认证提供者
  @JsonKey(name: 'twoFactorProvider')
  String twoFactorProvider;
  /// 双因素认证验证码
  @JsonKey(name: 'twoFactorCode')
  String twoFactorCode;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&twoFactorProvider=$twoFactorProvider&twoFactorCode=$twoFactorCode';
  }

  factory TwoFactorPasswordTokenRequest.fromJson(Map<String, dynamic> json) => _$TwoFactorPasswordTokenRequestFromJson(json);
  
  @override
  Map<String, dynamic> toJson() => _$TwoFactorPasswordTokenRequestToJson(this);
}

/// 身份认证密码验证请求承载体
@JsonSerializable()
class PasswordTokenRequest extends TokenRequest {
  PasswordTokenRequest({
    required clientId,
    clientSecret,
    required this.userName,
    required this.password,
  }) : super(
    grantType: 'password',
    clientId: clientId,
    clientSecret: clientSecret,
  );
  /// 用户名
  @JsonKey(name: 'username')
  String userName;
  /// 密码
  @JsonKey(name: 'password')
  String password;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&username=$userName&password=$password';
  }

  factory PasswordTokenRequest.fromJson(Map<String, dynamic> json) => _$PasswordTokenRequestFromJson(json);
  Map<String, dynamic> toJson() => _$PasswordTokenRequestToJson(this);
}

/// 刷新令牌请求承载体
@JsonSerializable()
class RefreshTokenRequest extends TokenRequest {
  RefreshTokenRequest({
    required String clientId,
    String? clientSecret,
    required this.refreshToken,
  }) : super(
    grantType: 'refresh_token',
    clientId: clientId,
    clientSecret: clientSecret,
  );
  /// 刷新令牌
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'refresh_token')
  late String refreshToken;

  @override
  String toFormUrlencodedData() {
    return '${super.toFormUrlencodedData()}&refresh_token=$refreshToken';
  }

  factory RefreshTokenRequest.fromJson(Map<String, dynamic> json) => _$RefreshTokenRequestFromJson(json);
  Map<String, dynamic> toJson() => _$RefreshTokenRequestToJson(this);
}

/// 身份认证请求承载体
abstract class TokenRequest {
  TokenRequest({
    required this.grantType,
    required this.clientId,
    this.clientSecret,
  });
  /// 认证类型
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'grant_type')
  late String grantType;
  /// 客户端标识
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'client_id')
  late String clientId;
  /// 客户端密钥
  // ignore: non_constant_identifier_names
  @JsonKey(name: 'client_secret')
  String? clientSecret;

  String toFormUrlencodedData() {
    return 'grant_type=$grantType&client_id=$clientId&client_secret=$clientSecret';
  }
}

/// 双因素认证请求承载
abstract class TwoFactorRequest {
  TwoFactorRequest({
    required this.twoFactorProvider,
    required this.twoFactorCode,
  });
  /// 双因素认证提供者
  @JsonKey(name: 'twoFactorProvider')
  String twoFactorProvider;
  /// 双因素认证验证码
  @JsonKey(name: 'twoFactorCode')
  String twoFactorCode;
}

/// 身份认证令牌承载体
@JsonSerializable()
class Token {
  Token({
    required this.accessToken,
    this.expiresIn,
    this.expiration,
    this.tokenType,
    this.refreshToken,
    this.scope,
  });
  /// 令牌
  @JsonKey(name: 'access_token')
  String accessToken;
  /// 过期时间
  @JsonKey(name: 'expires_in')
  int? expiresIn;
  /// 过期时间
  @JsonKey(name: 'expiration')
  DateTime? expiration;
  /// 令牌类型
  @JsonKey(name: 'token_type')
  String? tokenType;
  /// 刷新令牌
  @JsonKey(name: 'refresh_token')
  String? refreshToken;
  /// 范围
  @JsonKey(name: 'scope')
  String? scope;

  factory Token.fromJson(Map<String, dynamic> json) => _$TokenFromJson(json);
  Map<String, dynamic> toJson() => _$TokenToJson(this);
}

@JsonSerializable()
class UserProfile {
  UserProfile({
    required this.id,
    required this.userName,
    this.avatarUrl,
    this.name,
    this.surName,
    this.email,
    this.emailVerified,
    this.phoneNumber,
    this.phoneNumberVerified,
  });
  @JsonKey(name: 'sub')
  String id;
  @JsonKey(name: 'name')
  String userName;
  @JsonKey(name: 'avatarUrl')
  String? avatarUrl;
  @JsonKey(name: 'given_name')
  String? name;
  @JsonKey(name: 'family_name')
  String? surName;
  @JsonKey(name: 'email')
  String? email;
  @JsonKey(name: 'email_verified')
  String? emailVerified;
  @JsonKey(name: 'phoneNumber')
  String? phoneNumber;
  @JsonKey(name: 'phone_number_verified')
  String? phoneNumberVerified;

  bool get isEmailVerified => emailVerified?.toLowerCase() == 'true';
  bool get isPhoneNumberVerified => phoneNumberVerified?.toLowerCase() == 'true';

  factory UserProfile.fromJson(Map<String, dynamic> json) => _$UserProfileFromJson(json);
  Map<String, dynamic> toJson() => _$UserProfileToJson(this);
}

class PortalLoginProvider {
  PortalLoginProvider({
    required this.id,
    required this.name,
    this.logo,
  });
  String id;
  String name;
  String? logo;

  factory PortalLoginProvider.fromJson(Map<String, dynamic> json) => 
    PortalLoginProvider(
      id: json['Id'] as String,
      name: json['Name'] as String,
      logo: json['Logo'] as String?,
    );
    
  Map<String, dynamic> toJson() => 
    <String, dynamic>{
      'Id': id,
      'Name': name,
      'Logo': logo,
    };
}

class PortalLoginException implements Exception {
  PortalLoginException(this.providers);
  List<PortalLoginProvider> providers;
}
