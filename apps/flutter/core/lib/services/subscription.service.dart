import 'dart:async';
import 'package:core/services/service.base.dart';
import 'package:rxdart_ext/rxdart_ext.dart';

class SubscriptionService extends ServiceBase {
  SubscriptionService(super._injector);
  final CompositeSubscription _subscription = CompositeSubscription();

  @override
  void onClose() {
    _subscription.clear();
    super.onClose();
  }

  void addOne<T>(StreamSubscription<T> subscription) {
    subscription.addTo(_subscription);
  }

  void subscribeOnce<T>(
    Stream<T> source$,
    {
      void Function(T value)? next,
      void Function(dynamic error)? error,
      void Function()? done,
      void Function()? cancel,
    }
  ) {
    StreamSubscription<T>? subscription;
    subscription = subscribe(source$,
      next: next,
      error: error,
      cancel: cancel,
      done: () {
        if (done != null) done();
        closeOne(subscription);
      }
    );
  }

  StreamSubscription<T> subscribe<T>(
    Stream<T> source$,
    {
      void Function(T value)? next,
      void Function(dynamic error)? error,
      void Function()? done,
      void Function()? cancel,
    }
  ) {
    var subscription = source$.doOn(
      data: next,
      error:(err, stackTrace) {
        if (error != null) {
          error(err);
        }
      },
      done: done,
      cancel: cancel,
    ).listen(null);
  
    subscription.addTo(_subscription);

    return subscription;
  }

  Future<void> closeAll() async {
    await _subscription.clear();
  }

  Future<void> closeOne<T>(StreamSubscription<T>? subscription, { bool shouldCancel = true }) async {
    await removeOne(subscription, shouldCancel: shouldCancel);
  }
  
  Future<void> removeOne<T>(StreamSubscription<T>? subscription, { bool shouldCancel = true }) async {
    if (subscription == null) return;
    await _subscription.remove(subscription, shouldCancel: shouldCancel);
  }

  Future<void> reset() async {
    await _subscription.clear();
  }
}