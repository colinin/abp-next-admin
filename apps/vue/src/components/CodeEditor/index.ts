import { withInstall } from '/@/utils';
import codeEditor from './src/CodeEditor.vue';
import codeEditorX from './src/codemirrorX/CodeMirrorX.vue';
import jsonPreview from './src/json-preview/JsonPreview.vue';

export const CodeEditor = withInstall(codeEditor);
export const CodeEditorX = withInstall(codeEditorX);
export const JsonPreview = withInstall(jsonPreview);

export * from './src/typing';
