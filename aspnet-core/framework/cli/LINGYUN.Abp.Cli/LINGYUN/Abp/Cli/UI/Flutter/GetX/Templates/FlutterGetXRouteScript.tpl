import 'package:get/get.dart';

import 'view.dart';
import 'route.names.dart';

class {{ model.application }}Route {
  static List<GetPage> routes = [
    GetPage(
      name: {{ model.application }}Routes.index,
      page: () => const {{ model.application }}Page(),
      binding: {{ model.application }}Binding(),
    ),
  ];
}