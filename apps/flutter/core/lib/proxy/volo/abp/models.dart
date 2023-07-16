import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

abstract class NameValue<T> {
  NameValue({
    required this.name,
    required this.value,
  });
  String name;
  T value;
}

@JsonSerializable()
class StringValue extends NameValue<String> {
  StringValue({
    required super.name,
    required super.value,
  });

  factory StringValue.fromJson(Map<String, dynamic> json) => _$StringValueFromJson(json);
  Map<String, dynamic> toJson() => _$StringValueToJson(this);
}