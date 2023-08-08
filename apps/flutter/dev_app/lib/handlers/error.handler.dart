import 'dart:async';

import 'package:account/pages/route.name.dart';
import 'package:core/dependency/index.dart';
import 'package:core/models/abp.error.dart';
import 'package:core/services/index.dart';
import 'package:core/tokens/index.dart';
import 'package:dev_app/utils/loading.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:rxdart/rxdart.dart';

class ErrorHandler {
  ErrorHandler(Injector injector):
    _sessionService = injector.get<SessionService>(),
    _errorReporter = injector.get<ErrorReporterService>();

  final SessionService _sessionService;
  final ErrorReporterService _errorReporter;

  StreamSubscription<Exception> listenToRestError() {
    return _errorReporter.getReporter$()
      .whereType<DioException>()
      .where(_filterRestErrors)
      .listen(_handleError);
  }

  bool _filterRestErrors(DioException exception) {
    return !exception.requestOptions.extra.containsKey(HttpTokens.ignoreError);
  }

  void _handleError(DioException exception) {
    if (exception.response == null) return;
    var errorTitle = 'DefaultErrorMessage'.tr;
    var errorMessage = 'DefaultErrorMessageDetail'.tr;
    if (exception.response!.headers['abp-tenant-resolve-error']?.isEmpty == false) {
      _sessionService.setToken(null);
      return;
    }

    if (exception.response!.headers['_abperrorformat']?.isEmpty == false) {
      _showErrorWithRequestBody(exception);
    } else {
      switch (exception.response!.statusCode) {
        case 400:
        if (exception.response?.data?['error'] != null) {
          errorTitle = exception.response!.data['error'];
        }
        if (exception.response?.data?['error_description'] != null) {
          errorMessage = exception.response!.data['error_description'];
        }
        case 401:
          errorTitle = 'DefaultErrorMessage401'.tr;
          errorMessage = 'DefaultErrorMessage401Detail'.tr;
          _showError(errorTitle, errorMessage, () => Get.toNamed(AccountRoutes.login));
          return;
        case 403:
          errorTitle = 'DefaultErrorMessage403'.tr;
          errorMessage = 'DefaultErrorMessage403Detail'.tr;
          break;
        case 404:
          errorTitle = 'DefaultErrorMessage404'.tr;
          errorMessage = 'DefaultErrorMessage404Detail'.tr;
          break;
        case 500:
          errorTitle = '500Message'.tr;
          errorMessage = 'DefaultErrorMessage'.tr;
          break;
        default:
      }
      _showError(errorTitle, errorMessage);
    }
  }

  void _showErrorWithRequestBody(DioException err) {
    var errorJson = err.response!.data['error'];
    var remoteServiceErrorInfo = RemoteServiceErrorInfo.fromJson(errorJson);
    var errorTitle = remoteServiceErrorInfo.message;
    var errorMessage = remoteServiceErrorInfo.details ?? errorTitle;
    if (remoteServiceErrorInfo.validationErrors?.isEmpty == false) {
      errorMessage += remoteServiceErrorInfo.validationErrors!.map((v) => v.message).join('\n');
    }
    _showError(errorTitle, errorMessage);
  }

  void _showError(String title, String message, [VoidCallback? callback]) {
    //Loading.toast(message);
    if (Get.isOverlaysOpen == false) {
      Loading.toast(message);
      if (callback != null) {
        callback();
      }
      return;
    }
    Get.defaultDialog(
      title: title,
      content: Text(message),
      onConfirm: () {
        if (callback != null) {
          callback();
        }
      },
    );
  }
}