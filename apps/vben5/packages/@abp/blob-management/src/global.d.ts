declare module 'vue-simple-uploader' {
  import type { DefineComponent } from 'vue';

  const uploader: {
    Uploader: DefineComponent<any>;
    UploaderBtn: DefineComponent<any>;
    UploaderDrop: DefineComponent<any>;
    UploaderList: DefineComponent<any>;
    UploaderUnsupport: DefineComponent<any>;
  };

  export default uploader;
}

declare module '@vue-office/docx' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/docx/lib/v3/index.js' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/excel' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/excel/lib/v3/index.js' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/pdf' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/pdf/lib/v3/index.js' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/pptx' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}

declare module '@vue-office/pptx/lib/v3/index.js' {
  import { DefineComponent } from 'vue';

  const component: DefineComponent<any>;
  export default component;
}
