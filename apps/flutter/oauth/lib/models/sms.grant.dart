import 'package:dio/dio.dart';
import 'package:oauth_dio/oauth_dio.dart';

class SmsGrant extends OAuthGrantType {
  SmsGrant({
    required this.phoneNumber,
    required this.code,
    this.scope = const [],
  });
  final String phoneNumber;
  final String code;
  final List<String> scope;

  @override
  RequestOptions handle(RequestOptions request) {
    request.data = {
      "grant_type": "phone_verify",
      "scope": scope.join(' '),
      "phoneNumber": phoneNumber,
      "code": code,
    };
    return request;
  }
}