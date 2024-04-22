import 'package:core/core.module.dart';
import 'package:core/modularity/index.dart';

class ComponentsModule extends Module {
  @override
  List<Module> get dependencies => [CoreModule()];
}