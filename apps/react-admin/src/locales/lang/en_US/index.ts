import common from "./common.json";
import sys from "./sys.json";
import ui from "./ui.json";
import abp from "./abp.json";
import component from "./component.json";
import workbench from "./workbench.json";
import authentication from "./authentication.json";

export default {
	...common,
	...sys,
	...ui,
	...abp,
	...component,
	...workbench,
	...authentication,
};
