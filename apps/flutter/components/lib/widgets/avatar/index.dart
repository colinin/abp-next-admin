import 'package:core/utils/string.extensions.dart';
import 'package:flutter/material.dart';
import 'package:components/config/avatar.config.dart';

class Avatar extends StatelessWidget {
  const Avatar({
    super.key,
    this.url,
    this.schema,
    this.hintText,
    this.takeToken,
  });

  final String? url;
  final String? hintText;
  final String? schema;
  final String? takeToken;

  String get avatarUrl {
    var formatUrl = url ?? '';
    if (!formatUrl.isNullOrWhiteSpace()) {
      var urlSchema = schema ?? AvatarConfig.baseUrl;
      if (!formatUrl.startsWith(urlSchema)) {
        formatUrl = "$urlSchema$formatUrl";
      }
    }
    if (!takeToken.isNullOrWhiteSpace()) {
      formatUrl += "?access_token=$takeToken";
    }
    return formatUrl;
  }
  
  @override
  Widget build(BuildContext context) {
    return CircleAvatar(
      child: !url.isNullOrWhiteSpace()
        ? CircleAvatar(
            backgroundImage: NetworkImage(avatarUrl),
          )
        : Text(!hintText.isNullOrWhiteSpace() && hintText!.length > 3 ? hintText!.substring(0, 3) : hintText ?? 'A',
            style: const TextStyle(
              //fontSize: 20,
              color: Colors.white,
            ),
          ),
    );
  }
}