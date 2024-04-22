import 'package:account/pages/route.account.dart';
import 'package:core/modularity/module.dart';
import 'package:get/get.dart';

class AccountModule extends Module {
  @override
  void configureServices() {
    inject<AccountModule>(this);
  }

  @override
  List<GetPage> get routes => [
    ...AccountRoute.routes,
  ];
}
