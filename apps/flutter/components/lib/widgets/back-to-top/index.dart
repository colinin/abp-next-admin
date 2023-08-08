import 'package:flutter/material.dart';

class BackToTop extends StatefulWidget {
  const BackToTop({
    super.key,
    required this.controller,
    this.bottom,
  });

  final double? bottom;
  final ScrollController controller;

  @override
  State<StatefulWidget> createState() => _BackToTopState();

}

class _BackToTopState extends State<BackToTop> {
  bool shown = false;

  @override
  void initState() {
    super.initState();
    widget.controller.addListener(isScroll);
  }

  void isScroll() {
    final bool toShow = widget.controller.offset > MediaQuery.of(context).size.height / 2;
    if (toShow ^ shown) {
      setState(() {
        shown = toShow;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Positioned(
        bottom: MediaQuery.of(context).padding.bottom + (widget.bottom ?? 40),
        right: 20,
        child: Offstage(
          offstage: !shown,
          child: GestureDetector(
            onTap: () {
              widget.controller.animateTo(0,
                duration: const Duration(milliseconds: 200),
                curve: Curves.easeIn);
            },
            child: Container(
                height: 48,
                width: 48,
                alignment: const Alignment(0, 0),
                decoration: BoxDecoration(
                    color: Theme.of(context).highlightColor,
                    borderRadius: const BorderRadius.all(Radius.circular(20)),
                    boxShadow: [
                      BoxShadow(
                          color: const Color(0xFF000000).withOpacity(0.1),
                          blurRadius: 4,
                          spreadRadius: 0),
                    ]),
                child: Column(
                  children: <Widget>[
                    Container(
                      margin: const EdgeInsets.only(top: 4),
                      child: Icon(
                        Icons.vertical_align_top,
                        size: 20,
                        color: Theme.of(context).hintColor,
                      ),
                    ),
                    Container(
                      margin: const EdgeInsets.only(top: 0),
                      child: const Text(
                        'Top',
                        //style: TextStyle(fontSize: 10, color: Color(0xFFA1A6AA)),
                      ),
                    )
                  ],
                )
            ),
          ),
        )
    );
  }
}