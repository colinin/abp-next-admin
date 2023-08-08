class TranslationState {
  TranslationState({
    this.language,
    this.translations = const {},
  });
  String? language;
  Map<String, Map<String, String>> translations;
}