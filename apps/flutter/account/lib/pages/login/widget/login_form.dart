import 'package:core/utils/string.extensions.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../state.dart';

class LoginForm extends StatelessWidget {
  LoginForm({
    super.key,
    required this.state,
    required this.onSubmit,
    this.onPwdVisiable,
  });

  final LoginState state;
  final Future<void> Function() onSubmit;
  final VoidCallback? onPwdVisiable;

  final GlobalKey<FormState> formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget> [
        const SizedBox(height: 30),
        Padding(
          padding: const EdgeInsets.only(left: 25),
          child: Column(
            children: [
              Text(
                'Label:LoginTitle'.tr,
                textAlign: TextAlign.left,
                style: const TextStyle(
                  fontSize: 32.0,
                  fontWeight: FontWeight.w400,
                  letterSpacing: 0.0,
                )
              ),
              Text(
                'Label:LoginSubTitle'.tr,
                textAlign: TextAlign.left,
                style: const TextStyle(
                  fontSize: 16.0,
                  fontWeight: FontWeight.w400,
                  letterSpacing: 0.32,
                )
              ),
            ],
          )),
        const SizedBox(height: 30),
        Padding(
          padding: const EdgeInsets.all(15),
          child: Column(
            children: <Widget>[
              Form(
                key: formKey,
                child: Column(
                  children: <Widget>[
                    SizedBox(
                      height: 40,
                      child: TextFormField(
                        textAlignVertical: TextAlignVertical.bottom,
                        controller: state.username,
                        decoration: InputDecoration(
                          hintText: 'Label:UserNameRequired'.tr,
                          prefixIcon: const Icon(Icons.person),
                          filled: true,
                        ),
                        validator: (value) {
                          if (value.isNullOrWhiteSpace()) {
                            return 'Label:UserNameRequired'.tr;
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(height: 20),
                    SizedBox(
                      height: 40,
                      child: TextFormField(
                        textAlignVertical: TextAlignVertical.bottom,
                        controller: state.password,
                        decoration: InputDecoration(
                          hintText: 'Label:PasswordRequired'.tr,
                          prefixIcon: const Icon(Icons.lock),
                          suffixIcon: IconButton(
                            onPressed: onPwdVisiable,
                            icon: Icon(state.showPassword ? Icons.visibility_off : Icons.visibility,
                              color: state.showPassword ? Theme.of(context).indicatorColor : Theme.of(context).disabledColor,),
                          ),
                          filled: true,
                        ),
                        obscureText: !state.showPassword,
                        validator: (value) {
                          if (value.isNullOrWhiteSpace()) {
                            return 'Label:PasswordRequired'.tr;
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(height: 20),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        TextButton(
                          onPressed: () {

                          },
                          child: Text('Label:NoAccount'.tr)
                        ),
                        TextButton(
                          onPressed: () {

                          },
                          child: Text('Label:ForgotPassword'.tr)
                        ),
                      ],
                    ),
                    const SizedBox(height: 10),
                    Padding(
                      padding: const EdgeInsets.only(left: 8, right: 8, top: 6),
                      child: SizedBox(
                        height: 40,
                        child: Row(
                          children: [
                            Expanded(
                              child: _loginButton(state.loading),
                            )
                          ],
                        ),
                      )
                    ),
                  ],
                ),
              )
            ],
          ),
        ),
      ]
    );
  }

  Widget _loginButton(bool isLoading) {
    return IgnorePointer(
      ignoring: isLoading,
      child: FilledButton(
        onPressed: () {
          if (formKey.currentState?.validate() == true) {
            onSubmit().then((value) {
              formKey.currentState?.reset();
            });
          }
        },
        style: ButtonStyle(
          shape: MaterialStatePropertyAll(
            RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
          ),
        ),
        //shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        child: Text(
          'Label:Login'.tr,
          style: const TextStyle(
            letterSpacing: 4,
          ),
        )
      ),
    );
  }
}