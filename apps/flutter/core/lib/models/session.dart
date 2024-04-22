import 'package:core/abstracts/copy.with.dart';
import 'package:core/models/auth.dart';
import 'package:core/proxy/volo/abp/asp-net-core/index.dart';
import 'package:json_annotation/json_annotation.dart';

part 'session.g.dart';

@JsonSerializable()
class Session extends CloneObject<Session> {
  Session({
    this.language,
    this.profile,
    this.tenant,
    this.token,
  });
  String? language;
  UserProfile? profile;
  CurrentTenantDto? tenant;
  Token? token;

  factory Session.fromJson(Map<String, dynamic> json) => _$SessionFromJson(json);
  
  Map<String, dynamic> toJson() => _$SessionToJson(this);
  
  @override
  Session clone() => Session(
    language: language,
    profile: profile,
    tenant: tenant,
    token: token,
  );
}