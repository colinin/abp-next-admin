import 'package:date_format/date_format.dart';

extension DateTimeFormat on DateTime {
  String format(List<String> formats, {DateLocale locale = const EnglishDateLocale()}) {
    return formatDate(this, formats);
  }
}