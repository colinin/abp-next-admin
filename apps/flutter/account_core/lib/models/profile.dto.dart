import 'package:core/models/abp.dto.dart';
import 'package:json_annotation/json_annotation.dart';

part 'profile.dto.g.dart';

@JsonSerializable()
class ProfileDto extends ExtensibleObject {
  ProfileDto({
    this.userName,
    this.email,
    this.name,
    this.surname,
    this.phoneNumber,
    this.concurrencyStamp,
    this.isExternal = false,
    this.hasPassword = false,
    super.extraProperties,
  });
  String? userName;
  String? email;
  String? name;
  String? surname;
  String? phoneNumber;
  bool isExternal;
  bool hasPassword;
  String? concurrencyStamp;

  factory ProfileDto.fromJson(Map<String, dynamic> json) => _$ProfileDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$ProfileDtoToJson(this);
}

@JsonSerializable()
class ChangePasswordInput {
  ChangePasswordInput({
    this.currentPassword,
    required this.newPassword,
  });
  String? currentPassword;
  String newPassword;

  factory ChangePasswordInput.fromJson(Map<String, dynamic> json) => _$ChangePasswordInputFromJson(json);
  Map<String, dynamic> toJson() => _$ChangePasswordInputToJson(this);
}

@JsonSerializable()
class RegisterDto extends ExtensibleObject {
  RegisterDto({
    required this.userName,
    required this.emailAddress,
    required this.password,
    this.appName = 'abp-flutter',
    super.extraProperties,
  });
  String userName;
  String emailAddress;
  String password;
  String appName;

  factory RegisterDto.fromJson(Map<String, dynamic> json) => _$RegisterDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$RegisterDtoToJson(this);
}

@JsonSerializable()
class UpdateProfileDto extends ExtensibleObject {
  UpdateProfileDto({
    this.userName,
    this.email,
    this.name,
    this.surname,
    this.phoneNumber,
    this.concurrencyStamp,
    super.extraProperties,
  });
  String? userName;
  String? email;
  String? name;
  String? surname;
  String? phoneNumber;
  String? concurrencyStamp;

  factory UpdateProfileDto.fromJson(Map<String, dynamic> json) => _$UpdateProfileDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$UpdateProfileDtoToJson(this);
}