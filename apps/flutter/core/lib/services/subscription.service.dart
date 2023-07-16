import 'dart:async';
import 'package:rxdart/rxdart.dart';
import 'package:rxdart_ext/rxdart_ext.dart';

class SubscriptionService {
  final CompositeSubscription _subscription = CompositeSubscription();

  bool get isClosed => _subscription.isDisposed;

  void addOne<T>(StreamSubscription<T> subscription) {
    subscription.addTo(_subscription);
  }

  StreamSubscription<T> subscribe<T>(
    Stream<T> source$,
    [void Function(T value)? next,
    void Function(dynamic error)? error]
  ) {
    var subscription = source$.listen((value) {
      if (next != null) {
        next(value);
      }
    }, onError: (err) {
      if (error != null) {
        error(err);
      }
    });
    subscription.addTo(_subscription);

    return subscription;
  }

  Future<void> closeAll() async {
    await _subscription.cancel();
  }

  Future<void> closeOne<T>(StreamSubscription<T>? subscription) async {
    await removeOne(subscription);
    if (subscription != null) {
      await subscription.cancel();
    }
  }
  
  Future<void> removeOne<T>(StreamSubscription<T>? subscription) async {
    if (subscription == null) return;
    await _subscription.remove(subscription);
  }

  Future<void> reset() async {
    await _subscription.clear();
  }
}