import 'package:json_annotation/json_annotation.dart';

part 'models.g.dart';

@JsonSerializable()
class FindTenantResultDto {
  FindTenantResultDto({
    this.tenantId,
    this.name,
    required this.success,
    required this.isActive,
  });
  late bool success;
  String? tenantId;
  String? name;
  late bool isActive;

  factory FindTenantResultDto.fromJson(Map<String, dynamic> json) => _$FindTenantResultDtoFromJson(json);
  Map<String, dynamic> toJson() => _$FindTenantResultDtoToJson(this);
}

@JsonSerializable()
class CurrentTenantDto {
  CurrentTenantDto({
    this.id,
    this.name,
    this.isAvailable,
  });
  String? id;
  String? name;
  bool? isAvailable;

  factory CurrentTenantDto.fromJson(Map<String, dynamic> json) => _$CurrentTenantDtoFromJson(json);
  Map<String, dynamic> toJson() => _$CurrentTenantDtoToJson(this);
}

@JsonSerializable()
class MultiTenancyInfoDto {
  MultiTenancyInfoDto({
    required this.isEnabled,
  });
  late bool isEnabled;

  factory MultiTenancyInfoDto.fromJson(Map<String, dynamic> json) => _$MultiTenancyInfoDtoFromJson(json);
  Map<String, dynamic> toJson() => _$MultiTenancyInfoDtoToJson(this);
}