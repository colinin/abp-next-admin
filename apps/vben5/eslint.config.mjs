import { defineConfig } from '@vben/eslint-config';

export default defineConfig({
  "vue/html-closing-bracket-newline": [
    "error",
    {
      "singleline": "never",
      "multiline": "always",
      "selfClosingTag": {
        "singleline": "never",
        "multiline": "never"
      }
    }
  ]
});
