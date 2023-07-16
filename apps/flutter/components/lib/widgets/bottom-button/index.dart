import 'package:core/utils/theme.utils.dart';
import 'package:flutter/material.dart';

class BottomButton extends StatelessWidget {
  const BottomButton({
    super.key,
    this.padding,
    required this.title,
    this.titleStyle,
    required this.onPressed,
    this.enable = true,
  });
  
  final String title;
  final TextStyle? titleStyle;
  final VoidCallback onPressed;
  final EdgeInsetsGeometry? padding;
  final bool enable;

  final EdgeInsetsGeometry _defaultPadding = const EdgeInsets.only(left: 0, right: 0);
  
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: padding ?? _defaultPadding,
      child: Row(
        children: [
          _buildMainButtonPane(context),
        ],
      ),
    );
  }

  Widget _buildMainButtonPane(BuildContext context) {
    return Expanded(
      child: GestureDetector(
        onTap: () {
          if (enable == true) {
            onPressed();
          }
        },
        child: Container(
          height: 48,
          padding: const EdgeInsets.only(left: 8, right: 8, top: 6, bottom: 6),
          decoration: BoxDecoration(
            color: enable
                ? ThemeUtils.currentColor.primaryContainer
                : ThemeUtils.currentTheme.disabledColor,
            borderRadius: const BorderRadius.all(Radius.circular(12.0)),
          ),
          child: Center(
            child:  Text(
              title,
              style: TextStyle(
                fontSize: 16.0,
                fontWeight: FontWeight.w600,
                letterSpacing: 2,
                color: enable
                  ? Colors.white
                  : ThemeUtils.currentColor.inversePrimary.withOpacity(0.7),
              ).merge(titleStyle),
            ),
          ),
        ),
        // child: Text(
        //   title,
        //   style: titleStyle,
        // ),
      ),
    );
  }
}