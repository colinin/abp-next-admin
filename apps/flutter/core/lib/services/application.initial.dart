import 'package:core/models/index.dart';

abstract class ApplicationInitialService {
  Future<Application> initialApp();
}