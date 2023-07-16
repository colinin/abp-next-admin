import 'package:get/get.dart';

extension StringNullableUitl on String? {
  bool isNullOrWhiteSpace() {
    if (GetUtils.isNullOrBlank(this) == true) {
      return true;
    }
    return false;
  }
}

extension StringUitl on String {
  bool isNullOrWhiteSpace() {
    if (GetUtils.isNullOrBlank(this) == true) {
      return true;
    }
    return false;
  }
}