import 'package:core/dependency/index.dart';
import 'package:get/get.dart';
import 'package:core/models/request.dart';
import 'package:core/services/subscription.service.dart';
import 'package:notifications/proxy/index.dart';

import '../../models/index.dart';
import '../../services/index.dart';

class NotifierManageController extends GetxController {
  NotificationService get _notificationService => injector.get();
  NotificationStateService get _stateService => injector.get();
  SubscriptionService get _subscriptionService => injector.get();

  final Rx<NotificationState> _state = Rx<NotificationState>(NotificationState(
    isEnabled: true,
    groups: []));
  NotificationState get state => _state.value;

  void _updateState() {
    _state.value = _stateService.state;
    _stateService.getNotificationState$()
      .listen((state$) {
        _state.update((val) { 
          val = state$;
        });
      });
  }

  void onSubscribed(Notification notification, bool isSubscribe) {
    _stateService.changeLoadingState(notification, true);
    var requestOptions = RequestOptions();
    Stream<void> subscribe$;
    if (isSubscribe) {
      subscribe$ = _notificationService
        .subscribe(notification.name, requestOptions: requestOptions)
        .asStream();
    } else {
      subscribe$ = _notificationService
        .unSubscribe(notification.name, requestOptions: requestOptions)
        .asStream();
    }
    _subscriptionService.subscribeOnce(subscribe$,
      done: () {
        _stateService.changeSubscribedState(notification, isSubscribe);
        _stateService.changeLoadingState(notification, false);
      },
      cancel: () {
        if (super.isClosed) {
          // 页面退出时需要取消网络请求
          requestOptions.cancel();
          // 页面退出时需要改变加载状态
          _stateService.changeLoadingState(notification, false);
        }
      },
    );
  }

  void onNotificationEnabled(bool isEnabled) {
    _stateService.subscribeAll(isEnabled);
  }

  @override
  void onInit() {
    super.onInit();
    _updateState();
  }

  @override
  void onClose() {
    _subscriptionService.closeAll();
    super.onClose();
  }
}