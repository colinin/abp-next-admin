import 'package:core/models/abp.dto.dart';
import 'package:core/proxy/volo/abp/localization/models.dart';
import 'package:core/services/rest.service.dart';
import 'package:get/get.dart';

class LanguageService {
  RestService get _restService => Get.find();

  Future<ListResultDto<LanguageInfo>> getList() {
    return _restService.get('/api/abp/localization/languages')
    .then((res) {
      return ListResultDto<LanguageInfo>(
        items: (res.data['items'] as List<dynamic>).map((e) => LanguageInfo.fromJson(e)).toList(),
      );
    });
  }
}