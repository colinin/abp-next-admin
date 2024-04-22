import 'package:flutter/widgets.dart';

class Empty extends StatelessWidget {
  const Empty({
    super.key,
    this.width,
    this.height,
    this.margin,
    this.padding,
    this.child,
    this.alignment,
    this.decoration,
    this.foregroundDecoration,
    this.transformAlignment,
  });

  final Widget? child;
  final double? width;
  final double? height;
  final EdgeInsetsGeometry? margin;
  final EdgeInsetsGeometry? padding;
  final AlignmentGeometry? alignment;
  final AlignmentGeometry? transformAlignment;
  final Decoration? decoration;
  final Decoration? foregroundDecoration;
  
  @override
  Widget build(BuildContext context) {
    return Container(
      width: width,
      height: height,
      margin: margin,
      padding: padding,
      alignment: alignment,
      decoration: decoration,
      foregroundDecoration: foregroundDecoration,
      transformAlignment: transformAlignment,
      child: child,
    );
  }

  static Empty get none {
    return const Empty();
  }
}