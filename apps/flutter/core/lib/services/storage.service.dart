import 'package:core/dependency/index.dart';
import 'package:get_storage/get_storage.dart';
import 'service.base.dart';

class GetxStorageService extends StorageService {
  GetxStorageService(super._injector);
  static late GetStorage _storage;
  static Future<bool> init([String container = 'GetStorage']) async {
    try {
      var isInit = await GetStorage.init(container);
      if (isInit) {
        _storage = GetStorage(container);
      }
      return isInit;
    } catch(err) {
      return false;
    }
  }

  @override
  void clear() {
    _storage.erase();
  }

  @override
  String? getItem(String key) {
    if (_storage.hasData(key)) {
      return _storage.read<String>(key);
    }
    return null;
  }

  @override
  void removeItem(String key) {
    if (_storage.hasData(key)) {
      _storage.remove(key);
    }
  }

  @override
  void setItem(String key, String value) {
    _storage.write(key, value);
  }
}

class StorageService extends ServiceBase {
  StorageService(super._injector);
  static final Map<String, String> _storage = {};

  static StorageService get to => injector.get();

  static T? initStorage<T>(String key, T? Function(String) formater) {
    return to.getFormat(key, formater);
  }

  void clear() {
    _storage.clear();
  }

  String? getItem(String key) {
    return _storage[key];
  }

  void removeItem(String key) {
    _storage.remove(key);
  }

  void setItem(String key, String value) {
    _storage.update(key, (_) => value, ifAbsent: () => value);
  }
}

extension StorageServiceGetItem on StorageService {
  T? getFormat<T>(String key, T? Function(String) formater) {
    var item = getItem(key);
    if (item != null) {
      return formater(item);
    }
    return null;
  }
}