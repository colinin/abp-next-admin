import 'package:core/utils/string.extensions.dart';
import 'package:flutter/material.dart';

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
  
  @override
  Widget build(BuildContext context) {
    return CircleAvatar(
      child: !url.isNullOrWhiteSpace()
        ? CircleAvatar(
            backgroundImage: NetworkImage(url!),
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