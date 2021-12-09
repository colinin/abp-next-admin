/**
 * Configure and register global directives
 */
import type { App } from 'vue';
import { setupPermissionDirective } from './permission';
import { setupFeatureDirective } from './feature';
import { setupLoadingDirective } from './loading';

export function setupGlobDirectives(app: App) {
  setupPermissionDirective(app);
  setupFeatureDirective(app);
  setupLoadingDirective(app);
}
