import { Rule } from 'ant-design-vue/lib/form';
import { OpenIdConfiguration } from '/@/api/identity-server/model/basicModel';
import { OpenIddictScopeDto } from '/@/api/openiddict/open-iddict-scope/model';

export interface ScopeState {
  activeTab: string;
  formRules?: Dictionary<string, Rule>,
  formModel: OpenIddictScopeDto;
  openIdConfiguration?: OpenIdConfiguration,
  entityChanged: boolean;
  isEdit: boolean;
}