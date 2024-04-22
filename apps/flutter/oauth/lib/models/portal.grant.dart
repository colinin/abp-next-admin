
import 'package:core/index.dart';
import 'package:dio/dio.dart';
import 'package:oauth_dio/oauth_dio.dart';

class PortalPasswordGrant extends PasswordGrant {
  PortalPasswordGrant({
    super.username,
    super.password,
    super.scope,
    this.enterpriseId,
  });
  final String? enterpriseId;

  /// Prepare Request
  @override
  RequestOptions handle(RequestOptions request) {
    request.data = {
      "grant_type": "portal",
      "username": username,
      "password": password,
      "scope": scope.join(' '),
      "enterpriseId": enterpriseId,
    };
    if (enterpriseId.isNullOrWhiteSpace()) {
      request.extra = {
        HttpTokens.ignoreError: 'true',
      };
    }
    return request;
  }
}