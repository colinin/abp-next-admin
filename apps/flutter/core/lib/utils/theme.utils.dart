import 'package:flutter/material.dart';

class ThemeUtils {

  static ColorScheme currentColor = lightColorScheme;
  static ThemeData currentTheme = lightTheme;

  static ThemeData lightTheme = ThemeData.light();

  static ColorScheme lightColorScheme = const ColorScheme(
    brightness: Brightness.light,
    primary: Color(0xffe65100),
    onPrimary: Color(0xffffffff),
    primaryContainer: Color(0xffffcc80),
    onPrimaryContainer: Color(0xff14110b),
    secondary: Color(0xff2979ff),
    onSecondary: Color(0xffffffff),
    secondaryContainer: Color(0xffe4eaff),
    onSecondaryContainer: Color(0xff131314),
    tertiary: Color(0xff2962ff),
    onTertiary: Color(0xffffffff),
    tertiaryContainer: Color(0xffcbd6ff),
    onTertiaryContainer: Color(0xff111214),
    error: Color(0xffb00020),
    onError: Color(0xffffffff),
    errorContainer: Color(0xfffcd8df),
    onErrorContainer: Color(0xff141213),
    background: Color(0xfffefaf8),
    onBackground: Color(0xff090909),
    surface: Color(0xfffefaf8),
    onSurface: Color(0xff090909),
    surfaceVariant: Color(0xffede5e0),
    onSurfaceVariant: Color(0xff121111),
    outline: Color(0xff7c7c7c),
    outlineVariant: Color(0xffc8c8c8),
    shadow: Color(0xff000000),
    scrim: Color(0xff000000),
    inverseSurface: Color(0xff161210),
    onInverseSurface: Color(0xfff5f5f5),
    inversePrimary: Color(0xffffcf99),
    surfaceTint: Color(0xffe65100),
  );

  static ThemeData darkTheme = ThemeData.dark();

  static ColorScheme darkColorScheme = const ColorScheme(
    brightness: Brightness.dark,
    primary: Color(0xffffb300),
    onPrimary: Color(0xff141204),
    primaryContainer: Color(0xffc87200),
    onPrimaryContainer: Color(0xfffff1df),
    secondary: Color(0xff82b1ff),
    onSecondary: Color(0xff0e1114),
    secondaryContainer: Color(0xff3770cf),
    onSecondaryContainer: Color(0xffe8f1ff),
    tertiary: Color(0xff448aff),
    onTertiary: Color(0xfff4f9ff),
    tertiaryContainer: Color(0xff0b429c),
    onTertiaryContainer: Color(0xffe1eaf8),
    error: Color(0xffcf6679),
    onError: Color(0xff140c0d),
    errorContainer: Color(0xffb1384e),
    onErrorContainer: Color(0xfffbe8ec),
    background: Color(0xff1d1910),
    onBackground: Color(0xffedecec),
    surface: Color(0xff1d1910),
    onSurface: Color(0xffedecec),
    surfaceVariant: Color(0xff463f2c),
    onSurfaceVariant: Color(0xffe1e0dd),
    outline: Color(0xff7d7676),
    outlineVariant: Color(0xff2e2c2c),
    shadow: Color(0xff000000),
    scrim: Color(0xff000000),
    inverseSurface: Color(0xfffffbf2),
    onInverseSurface: Color(0xff141312),
    inversePrimary: Color(0xff775b0e),
    surfaceTint: Color(0xffffb300),
  );
}