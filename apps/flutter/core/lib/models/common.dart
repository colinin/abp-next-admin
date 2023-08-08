import 'environment.dart';

class Application {
  Application({
    required this.environment,
  });
  Environment environment;

  factory Application.fromJson(Map<String, dynamic> json) => Application(
    environment: Environment.fromJson(json),
  );
  
  Map<String, dynamic> toJson() => 
    <String, dynamic>{
      'environment': environment,
    };
}

class Localization {
  Localization(
    this.culture,
    this.resources,
  );
  String culture;
  List<LocalizationResource> resources;
}

class LocalizationResource {
  LocalizationResource(
    this.resourceName,
    this.texts,
  );
  String resourceName;
  Map<String, String> texts;
}

class SignalrMessage {
  SignalrMessage(
    this.method,
    this.data,
  );
  String method;
  List<Object?> data;
}
