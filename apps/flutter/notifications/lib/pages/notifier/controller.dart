import 'package:get/get.dart';

import '../../models/index.dart';
import '../../services/index.dart';

class NotifierManageController extends GetxController {
  NotificationStateService get stateService => Get.find();

  final Rx<NotificationState> _state = Rx<NotificationState>(NotificationState(
    isEnabled: true,
    groups: []));
  NotificationState get state => _state.value;

  void _updateState() async {
    _state.value = stateService.state;
    stateService.getNotificationState$()
      .listen((state$) {
        _state.update((val) { 
          val = state$;
        });
      });
  }

  void onSubscribed(Notification notification, bool isSubscribe) async {
    await stateService.subscribe(notification, isSubscribe);
  }

  void onNotificationEnabled(bool isEnabled) {
    stateService.subscribeAll(isEnabled);
  }

  @override
  void onInit() {
    super.onInit();
    _updateState();
  }
}