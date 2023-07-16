import 'package:json_annotation/json_annotation.dart';

part 'abp.error.g.dart';

@JsonSerializable()
class RemoteServiceErrorInfo {
  RemoteServiceErrorInfo({
    required this.code,
    required this.message,
    this.details,
    this.data,
    this.validationErrors,
  });
  String code;
  String message;
  String? details;
  Map<String, String>? data;
  List<RemoteServiceValidationErrorInfo>? validationErrors;

  factory RemoteServiceErrorInfo.fromJson(Map<String, dynamic> json) => _$RemoteServiceErrorInfoFromJson(json);
  Map<String, dynamic> toJson() => _$RemoteServiceErrorInfoToJson(this);
}

@JsonSerializable()
class RemoteServiceValidationErrorInfo {
  RemoteServiceValidationErrorInfo({
    required this.message,
    this.members,
  });
  String message;
  List<String>? members = [];

  factory RemoteServiceValidationErrorInfo.fromJson(Map<String, dynamic> json) => _$RemoteServiceValidationErrorInfoFromJson(json);
  Map<String, dynamic> toJson() => _$RemoteServiceValidationErrorInfoToJson(this);
}