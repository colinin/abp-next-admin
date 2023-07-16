import 'package:flutter/material.dart';
import '../empty/index.dart';

class ActionButton extends StatelessWidget {
  const ActionButton({
    super.key,
    required this.title,
    this.prefixIcon,
    this.prefixIconColor,
    this.splashColor,
    this.onTap,
    this.suffix,
    this.suffixIcon,
    this.suffixIconColor,
    this.hideBorder = false,
  });

  final String title;
  final VoidCallback? onTap;
  final Color? splashColor;
  final IconData? prefixIcon;
  final Color? prefixIconColor;
  final Widget? suffix;
  final IconData? suffixIcon;
  final Color? suffixIconColor;
  final bool? hideBorder;
  
  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onTap,
      splashColor: splashColor,
      child: Container(
        decoration: BoxDecoration(
          border: hideBorder != true ? const Border(
            bottom: BorderSide(
              width: 0.2,
              style: BorderStyle.solid,
            ),
            // top: BorderSide(
            //   width: 0.2,
            //   style: BorderStyle.solid,
            // ),
          ) : null,
        ),
        child: Stack(
          children: [
            prefixIcon != null ? 
              Positioned(
                left: 10,
                top: 12,
                width: 20,
                height: 25,
                child: Center(
                  child: Icon(prefixIcon,
                    size: 25,
                    color: prefixIconColor
                  ),
                ),
              ) : const Empty(
                height: 50,
              ),
            Positioned(
              left: prefixIcon != null ? 50 : 16,
              height: 50,
              child: Center(
                child: Text(
                  title,
                  style: const TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.w400,
                  ),
                ),
              ),
            ),
            suffix == null ? const Empty() :
              Positioned(
                height: 50,
                right: 36,
                child: Center(
                  child: suffix!,
                ),
            ),
            Positioned(
              height: 50,
              right: 10,
              child: Center(
                child: Icon(
                  suffixIcon ?? Icons.arrow_forward_ios,
                  size: 16,
                  color: suffixIconColor,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}