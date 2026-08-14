import { oxlintConfig } from '@vben/oxlint-config';

import { defineConfig } from 'oxlint';

export default defineConfig({
  ...oxlintConfig,
  rules: {
    'no-unused-expressions': 'warn',
    'no-unused-vars': 'warn',
    'typescript/no-dynamic-delete': 'warn',
    'typescript/no-non-null-assertion': 'warn',
    'unicorn/no-useless-spread': 'warn',
    'vitest/hoisted-apis-on-top': 'warn',
    'vitest/require-mock-type-parameters': 'warn',
  },
});
