class Wrapper<T> {
  Wrapper({
    required this.code,
    required this.message,
    required this.result,
    this.details,
  });
  String code;
  String message;
  String? details;
  T? result;

  factory Wrapper.fromJson(dynamic json) {
    return Wrapper<T>(
      code: json['code'] as String,
      message: json['message'] as String,
      result: json['result'] as T?,
      details: json['details'] as String?,
    );
  }
}