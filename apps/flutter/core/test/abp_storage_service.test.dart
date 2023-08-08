import 'package:core/core.module.dart';
import 'package:core/dependency/index.dart';
import 'package:core/services/storage.service.dart';
import 'package:flutter_test/flutter_test.dart';

void main() {
  test('set storage', () async {
    var module = CoreModule();
    await module.initAsync();
    var storage = injector.get<StorageService>();

    storage.setItem('key', 'value');
    expect(storage.getItem('key'), 'value');

    storage.removeItem('key');
    expect(storage.getItem('key'), null);

    storage.clear();
    expect(storage.getItem('key'), null);
  });
}
