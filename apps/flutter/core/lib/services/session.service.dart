import 'dart:convert';

import 'package:core/models/auth.dart';
import 'package:core/models/session.dart';
import 'package:core/proxy/index.dart';
import 'package:core/utils/internal.store.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:get/get.dart';
import 'package:rxdart/rxdart.dart';

import '../dependency/injector.dart';
import 'environment.service.dart';
import 'storage.service.dart';
import 'config.state.service.dart';
import 'service.base.dart';

class SessionService extends ServiceBase {
  SessionService(super._injector);

  static const String sessionKey = '_abp_session_';
  final InternalStore<Session> _store = InternalStore<Session>(state: _initState());

  static SessionService get to => Injector.instance.get();

  StorageService get _storageService => resolve<StorageService>();
  ConfigStateService get _configStateService => resolve<ConfigStateService>();
  EnvironmentService get _environmentService => resolve<EnvironmentService>();

  bool get isAuthenticated {
    if (_store.state.profile == null || _store.state.token == null) {
      return false;
    }
    
    return !_store.state.profile!.id.isNullOrWhiteSpace() &&
      !_store.state.token!.accessToken.isNullOrWhiteSpace();
  }

  Session get state {
    return _store.state;
  }

  CurrentTenantDto? get tenant{
    return _store.state.tenant;
  }

  UserProfile? get profile {
    return _store.state.profile;
  }

  Token? get token {
    return _store.state.token;
  }

  String? get currentLanguage {
    return _store.state.language;
  }
  
  @override
  void onInit() {
    super.onInit();
    _initUpdateStream();
  }

  static Session _initState() {
    var session = StorageService.initStorage(sessionKey, (value) => Session.fromJson(jsonDecode(value)));
    return session ?? Session();
  }

  void _initUpdateStream() {
    var token$ = _store.sliceState((state) => state.token);
    token$
      .switchMap((_) => _configStateService.refreshAppState())
      .listen((abpConfig) {
        _store.patch((state) {
          state.tenant = abpConfig.currentTenant;
        });
      });

    var session$ = _store.sliceUpdate((state) => state);
    session$.listen((session) {
      _storageService.setItem(sessionKey, jsonEncode(session.toJson()));
    });
  }

  Stream<UserProfile?> getProfile$() {
    return _store.sliceState((state) => state.profile);
  }

  Stream<Token?> getToken$() {
    return _store.sliceState((state) => state.token);
  }

  Stream<String?> getLanguage$() {
    return _store.sliceState((state) => state.language);
  }

  Stream<String?> onLanguageChange$() {
    return _store.sliceUpdate((state) => state.language);
  }

  void setLanguage(String language) {
    if (language == _store.state.language) return;
    _store.patch((state) => state.language = language);
  }

  void setProfile(UserProfile? profile) {
    if (profile != null && !profile.avatarUrl.isNullOrWhiteSpace()) {
      if (!GetUtils.isURL(profile.avatarUrl!)) {
        var environment = _environmentService.getEnvironment();
        var avatarServerPath = environment.remoteServices.getApiUrl('avatar');
        if (!profile.avatarUrl!.startsWith(avatarServerPath)) {
          profile.avatarUrl = '$avatarServerPath${profile.avatarUrl}${'?access_token=${token?.accessToken}'}';
        }
      }
    }
    
    _store.patch((state) {
      state.profile = profile;
    });
  }

  void setToken(Token? token) {
    _store.patch((state) {
      state.token = token;
    });
  }

  void resetSession() {
    _store.patch((state) {
      state.token = null;
      state.profile = null;
    });
  }
}