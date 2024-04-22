class LoginParams {
  LoginParams({
    required this.username,
    required this.password,
  });
  String username;
  String password;
}

class SmsLoginParams {
  SmsLoginParams({
    required this.phonenumber,
    required this.code,
  });
  String phonenumber;
  String code;
}

class PortalLoginParams extends LoginParams {
  PortalLoginParams({
    this.enterpriseId,
    required super.username,
    required super.password,
  });
  String? enterpriseId;
}

class RefreshTokenParams {
  RefreshTokenParams(this.refreshToken);
  String refreshToken;
}
