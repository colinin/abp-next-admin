import 'package:core/dependency/injector.dart';
import 'package:core/models/auth.dart';
import 'package:core/models/oauth.dart';

import '../utils/logging.dart';

import 'service.base.dart';

abstract class AuthService extends ServiceBase {
  AuthService(super._injector);
  static AuthService get to => Injector.instance.get<AuthService>();

  Future<Token> password(LoginParams params) {
    logger.debug('not implemented');
    return Future.error('not implemented');
  }

  Future<Token> portal(PortalLoginParams params) {
    logger.debug('not implemented');
    return Future.error('not implemented');
  }

  Future<Token> phoneNumber(SmsLoginParams params) {
    logger.debug('not implemented');
    return Future.error('not implemented');
  }

  Future<Token> refreshToken(RefreshTokenParams params) {
    logger.debug('not implemented');
    return Future.error('not implemented');
  }

  // Future<UserProfile> getProfile() {
  //   logger.debug('not implemented');
  //   return Future.error('not implemented');
  // }
}