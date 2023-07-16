import 'package:json_annotation/json_annotation.dart';

part 'auth.g.dart';

@JsonSerializable()
class PortalLoginProvider {
  PortalLoginProvider({
    required this.id,
    required this.name,
    this.logo,
  });
  @JsonKey(name: 'Id')
  String id;
  @JsonKey(name: 'Name')
  String name;
  @JsonKey(name: 'Logo')
  String? logo;

  factory PortalLoginProvider.fromJson(Map<String, dynamic> json) => _$PortalLoginProviderFromJson(json);
  Map<String, dynamic> toJson() => _$PortalLoginProviderToJson(this);
}
