import 'dart:convert';

import 'package:core/dependency/index.dart';
import 'package:core/models/index.dart';
import 'package:dio/dio.dart';
import 'package:oauth/models/index.dart';
import 'package:oauth_dio/oauth_dio.dart' as authlib;
import 'package:core/services/auth.service.dart';
import 'package:core/services/environment.service.dart';
import 'package:core/services/rest.service.dart';

class OAuth extends authlib.OAuth {
  OAuth({
    required super.tokenUrl,
    required super.clientId,
    required super.clientSecret,
    super.dio,
    super.storage,
    super.extractor,
    super.validator,
  });

  @override
  Future<authlib.OAuthToken> requestToken(authlib.OAuthGrantType grantType) {
    final request = grantType.handle(
      RequestOptions(
        method: 'POST',
        path: '/',
        contentType: 'application/x-www-form-urlencoded',
        headers: {
          "Authorization":
              "Basic ${authlib.stringToBase64.encode('${super.clientId}:${super.clientSecret}')}"
        },
      ),
    );

    return super
        .dio
        .request(super.tokenUrl,
            data: request.data,
            options: Options(
              contentType: request.contentType,
              headers: request.headers,
              method: request.method,
              extra: request.extra,
            ))
        .then((res) => super.extractor(res));
  }
}

class OAuthService extends AuthService {
  OAuthService(Injector injector) : super(injector){
    _environmentService = injector.get<EnvironmentService>();
    var environment = _environmentService.getEnvironment();
    _auth = OAuth(
      tokenUrl: '${environment.auth.getAuthority()}connect/token',
      clientId: environment.auth.clientId,
      clientSecret: environment.auth.clientSecret ?? '',
      storage: injector.get<authlib.OAuthStorage>(),
      dio: injector.get<RestService>().dio
    );
  }

  late OAuth _auth;
  late EnvironmentService _environmentService;

  @override
  void onInit() {
    super.onInit();
    _environmentService
      .createOnUpdateStream
      .map((env) => env.auth)
      .listen((auth) {
        _auth.tokenUrl = '${auth.getAuthority()}connect/token';
        _auth.clientId = auth.clientId;
        _auth.clientSecret = auth.clientSecret ?? '';
      });
  }

  @override
  Future<Token> password(LoginParams params) {
    return _auth.requestTokenAndSave(authlib.PasswordGrant(
      username: params.username,
      password: params.password,
    )).then((res) {
      return Token(
        accessToken: res.accessToken!,
        refreshToken: res.refreshToken,
        expiration: res.expiration,
        tokenType: 'Bearer',
      );
    });
  }

  @override
  Future<Token> portal(PortalLoginParams params) {
    return _auth.requestTokenAndSave(PortalPasswordGrant(
      username: params.username,
      password: params.password,
      enterpriseId: params.enterpriseId,
    )).then((res) {
      return Token(
        accessToken: res.accessToken!,
        refreshToken: res.refreshToken,
        expiration: res.expiration,
        tokenType: 'Bearer',
      );
    }).catchError((error) {
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
    return _auth.requestTokenAndSave(SmsGrant(
      phoneNumber: params.phonenumber,
      code: params.code,
    )).then((res) {
      return Token(
        accessToken: res.accessToken!,
        refreshToken: res.refreshToken,
        expiration: res.expiration,
        tokenType: 'Bearer',
      );
    });
  }

  @override
  Future<Token> refreshToken(RefreshTokenParams params) {
    return _auth.requestTokenAndSave(authlib.RefreshTokenGrant(
      refreshToken: params.refreshToken,
    )).then((res) {
      return Token(
        accessToken: res.accessToken!,
        refreshToken: res.refreshToken,
        expiration: res.expiration,
        tokenType: 'Bearer',
      );
    });
  }
}