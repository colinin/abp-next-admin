import 'package:rxdart/rxdart.dart';

class ErrorReporterService {
  final Subject<Exception> _reporter$ = BehaviorSubject<Exception>();
  final BehaviorSubject<List<Exception>> _errors$ = BehaviorSubject<List<Exception>>.seeded([]);

  Stream<Exception> getReporter$() => _reporter$;
  Stream<List<Exception>> getErrors$() => _errors$;

  void reportError(Exception exception){
    _reporter$.add(exception);
    _errors$.add([..._errors$.value, exception]);
  }
}