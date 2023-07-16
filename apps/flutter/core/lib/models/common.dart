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
