import 'injector.dart';

typedef InjectorBuilderFactory<T> = T Function(Injector injector);

typedef AsyncInjectorBuilderFactory<T> = Future<T> Function(Injector injector);
