import 'auth.dart';

class PortalLoginException implements Exception {
  PortalLoginException(this.providers);
  List<PortalLoginProvider> providers;
}