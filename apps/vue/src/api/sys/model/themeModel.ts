import type { BeforeMiniState } from '/#/store';
import type { ProjectConfig } from '/#/config';

export interface ThemeSetting {
  darkMode?: string;
  projectConfig: ProjectConfig;
  beforeMiniInfo: BeforeMiniState;
}
