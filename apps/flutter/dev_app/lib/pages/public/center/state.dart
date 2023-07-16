import 'package:core/models/auth.dart';

class CenterState {
  CenterState({
    required this.isAuthenticated,
    this.token,
    this.profile,
  });
  Token? token;
  UserProfile? profile;
  bool isAuthenticated;
}