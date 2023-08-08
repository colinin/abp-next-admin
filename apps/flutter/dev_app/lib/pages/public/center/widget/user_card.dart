import 'package:components/index.dart';
import 'package:flutter/material.dart';

import '../state.dart';

class UserCard extends StatelessWidget {
  const UserCard({
    super.key,
    required this.state,
    this.onTap,
  });

  final CenterState state;
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
                url: state.profile?.avatarUrl,
                hintText: state.userName,
                takeToken: state.token?.accessToken,
              ),
            ),
            Container(
              margin: const EdgeInsets.only(left: 20),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(state.userName,
                    style: Theme.of(context).textTheme.titleMedium,
                  ),
                  Opacity(
                    opacity: 0.6,
                    child: Text(state.phoneNumber,
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