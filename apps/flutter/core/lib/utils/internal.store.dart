import 'package:rxdart_ext/rxdart_ext.dart';

class InternalStore<State> implements DisposableMixin {
  InternalStore({
    required State state,
  }) {
    _state = state;
    update$ = BehaviorSubject<State?>();
    state$ = BehaviorSubject<State>.seeded(state);
  }
  late BehaviorSubject<State?> update$;
  late BehaviorSubject<State> state$;
  late State _state;

  State get state => state$.value;

  Stream<T> sliceState<T>(
    T Function(State) selector,
    { bool Function(T, T)? compareFn }) {
    return state$.map((value) => selector(value))
      .distinctUniqueBy((map) => map, equals: compareFn);
  }

  Stream<T> sliceUpdate<T>(
    T Function(State) selector,
    {
      bool Function(T)? filterFn,
    }) {
    return update$.map((value) => selector(value as State))
      .where((state) {
        if (filterFn != null) {
          return filterFn(state);
        }
        return state != null;
      });
  }

  void set(State state) {
    _state = state;
    //state$.value = _state;
    state$.add(state);
    update$.add(state);
  }

  void patch(Function(State) update) {
    update(_state);
    state$.add(_state);
    update$.add(_state);
  }
  
  @override
  void dispose() {
    update$.close();
    state$.close();
  }
  
  @override
  Stream<void> get dispose$ => state$;
}
