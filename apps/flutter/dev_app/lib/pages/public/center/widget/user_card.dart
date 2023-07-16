import 'package:components/index.dart';
import 'package:flutter/material.dart';

class UserCard extends StatelessWidget {
  const UserCard({
    super.key,
    required this.userName,
    required this.phoneNumber,
    this.avatarUrl,
    this.takeToken,
    this.onTap,
  });

  final String userName;
  final String phoneNumber;
  final String? avatarUrl;
  final String? takeToken;
  final VoidCallback? onTap;
  
  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onTap,
      borderRadius: const BorderRadius.all(Radius.circular(12)),
      child: Card(
        child: Row(
          children: [
            Container(
              margin: const EdgeInsets.only(left: 10),
              width: 50,
              child: Avatar(
                url: avatarUrl,
                hintText: userName,
                takeToken: takeToken,
              ),
            ),
            Container(
              margin: const EdgeInsets.only(left: 20),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(userName,
                    style: Theme.of(context).textTheme.titleMedium,
                  ),
                  Opacity(
                    opacity: 0.6,
                    child: Text(phoneNumber,
                      style: Theme.of(context).textTheme.bodySmall,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}