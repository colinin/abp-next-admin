import 'package:core/models/index.dart';
import 'package:core/services/index.dart';
import 'package:core/services/service.base.dart';
import 'package:oauth_dio/oauth_dio.dart';
import 'package:rxdart/rxdart.dart';

class OAuthStorageService extends ServiceBase implements OAuthStorage {
  OAuthStorageService(super._injector);

  SessionService get _sessionService => resolve<SessionService>();
  RestService get _restService => resolve<RestService>();

  @override
  void onInit() {
    super.onInit();
    _sessionService.getToken$()
      .whereNotNull()
      .switchMap((token) => Stream.fromFuture(userProfile()))
      .listen((profile) async {
        _sessionService.setProfile(profile);
      });
  }

  @override
  Future<void> clear() {
    return Future.sync(() => _sessionService.setToken(null));
  }

  @override
  Future<OAuthToken?> fetch() {
    return Future.sync(() {
      var token = _sessionService.token;
      if (token == null) return null;
      return OAuthToken(
        accessToken: token.accessToken,
        refreshToken: token.refreshToken,
        expiration: token.expiration,
      );
    });
  }

  @override
  Future<OAuthToken> save(OAuthToken token) {
    return Future.sync(() {
      _sessionService.setToken(Token(
        accessToken: token.accessToken!,
        refreshToken: token.refreshToken,
        tokenType: 'Bearer',
        expiration: token.expiration));

      return token;
    });
  }

  Future<UserProfile?> userProfile() {
    return _restService.request(
      method: HttpMethod.GET,
      url: '/connect/userinfo',
      transformer: (res) => UserProfile.fromJson(res.data)
    );
  }
}