import 'package:core/dependency/injector.dart';
import 'package:core/models/abp.dto.dart';
import 'package:core/proxy/volo/abp/localization/models.dart';
import 'package:core/services/rest.service.dart';

class LanguageService {
  LanguageService(Injector injector):
    _restService = injector.get<RestService>();
  final RestService _restService;

  Future<ListResultDto<LanguageInfo>> getList() {
    return _restService.request<ListResultDto<LanguageInfo>>(
      method: HttpMethod.GET,
      url: '/api/abp/localization/languages',
      transformer: (res) {
        return ListResultDto<LanguageInfo>(
          items: (res.data['items'] as List<dynamic>).map((e) => LanguageInfo.fromJson(e)).toList(),
        );
      }
    );
  }
}