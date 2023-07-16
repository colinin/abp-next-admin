import 'dart:convert';

import 'package:account/models/auth.dart';
import 'package:account/models/common.dart';
import 'package:core/models/auth.dart';
import 'package:core/models/oauth.dart';
import 'package:core/services/index.dart';
import 'package:core/tokens/http.token.dart';
import 'package:dio/dio.dart';

class OAuthService extends AuthService {
  OAuthService({
    required this.clientId,
    this.clientSecret,
  });
  RestService get _restService => find();

  final String clientId;
  final String? clientSecret;

  @override
  Future<Token> password(LoginParams params) {
    var request = PasswordTokenRequest(
      clientId: clientId,
      clientSecret: clientSecret,
      userName: params.username,
      password: params.password);
    return _restService.post('/connect/token',
      data: request.toFormUrlencodedData(),
      options: Options(
        extra: {
          HttpTokens.ignoreToken: true,
        },
        contentType: 'application/x-www-form-urlencoded',
      )).then((res) => Token.fromJson(res.data));
  }

  @override
  Future<Token> portal(PortalLoginParams params) {
    var request = PortalTokenRequest(
      clientId: clientId,
      clientSecret: clientSecret,
      userName: params.username,
      password: params.password,
      enterpriseId: params.enterpriseId);
    return _restService.post('/connect/token',
      data: request.toFormUrlencodedData(),
      options: Options(
        extra: {
          HttpTokens.ignoreToken: true,
          HttpTokens.ignoreError: true,
        },
        contentType: 'application/x-www-form-urlencoded',
      )).then((res) => Token.fromJson(res.data))
        .catchError((error) {
          var portalProviders = (jsonDecode(error.response.data["Enterprises"]) as List<dynamic>)
            .map((e) => PortalLoginProvider.fromJson(e)).toList();
          throw PortalLoginException(portalProviders);
        }, test:(error) {
          var err = error as dynamic;
          if (err?.response?.statusCode == 400 && err?.response?.data != null &&
            err?.response?.data["Enterprises"] != null) {
            return true;
          }
          return false;
        });
  }

  @override
  Future<Token> phoneNumber(SmsLoginParams params) {
    var request = PhoneNumberTokenRequest(
      clientId: clientId,
      clientSecret: clientSecret,
      phoneNumber: params.phonenumber,
      code: params.code);
    return _restService.post('/connect/token',
      data: request.toFormUrlencodedData(),
      options: Options(
        extra: {
          HttpTokens.ignoreToken: true,
        },
        contentType: 'application/x-www-form-urlencoded',
      )).then((res) => Token.fromJson(res.data));
  }

  @override
  Future<Token> refreshToken(RefreshTokenParams params) {
    var request = RefreshTokenRequest(
      clientId: clientId,
      clientSecret: clientSecret,
      refreshToken: params.refreshToken,
    );
    return _restService.post('/connect/token',
      data: request.toFormUrlencodedData(),
      options: Options(
        contentType: 'application/x-www-form-urlencoded',
      )).then((res) => Token.fromJson(res.data));
  }

  @override
  Future<UserProfile> getProfile() {
    return _restService.get('/connect/userinfo')
      .then((res) => UserProfile.fromJson(res.data));
  }
}