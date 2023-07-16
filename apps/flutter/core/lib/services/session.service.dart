import 'dart:convert';

import 'package:core/models/auth.dart';
import 'package:core/models/session.dart';
import 'package:core/proxy/index.dart';
import 'package:core/utils/internal.store.dart';
import 'package:core/utils/string.extensions.dart';
import 'package:get/get.dart';
import 'package:rxdart/rxdart.dart';

import 'auth.service.dart';
import 'storage.service.dart';
import 'config.state.service.dart';
import 'service.base.dart';

class SessionService extends ServiceBase {
  static const String sessionKey = '_abp_session_';
  final InternalStore<Session> _store = InternalStore<Session>(state: _initState());

  static SessionService get to => Get.find();

  AuthService get _authService => find();
  StorageService get _storageService => find();
  ConfigStateService get _configStateService => find();

  bool get isAuthenticated {
    if (_store.state.profile == null || _store.state.token == null) {
      return false;
    }
    
    return !_store.state.profile!.id.isNullOrWhiteSpace() &&
      !_store.state.token!.accessToken.isNullOrWhiteSpace();
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
    token$.doOnData((_) => _configStateService.refreshAppState())
      .whereNotNull()
      .switchMap((_) => Stream.fromFuture(_authService.getProfile()))
      .listen((profile) {
        var abpConfig = _configStateService.getAll();
        _store.patch((state) {
          state.profile = profile;
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

  void refreshProfile() {
    _store.patch((state) {
      state.profile = profile;
    });
  }

  void refreshToken(Token? token) {
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