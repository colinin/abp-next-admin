import { Rule } from 'ant-design-vue/lib/form';
import { OpenIdConfiguration } from '/@/api/identity-server/discovery/model';
import { OpenIddictApplicationDto } from '/@/api/openiddict/open-iddict-application/model';

export interface EndPointComponent {
  component: string;
  uris?: string[];
}

export interface ApplicationState {
  activeTab: string;
  formRules?: Dictionary<string, Rule>;
  application: OpenIddictApplicationDto;
  endPoint: EndPointComponent;
  openIdConfiguration?: OpenIdConfiguration;
  entityChanged: boolean;
  isEdit: boolean;
}
