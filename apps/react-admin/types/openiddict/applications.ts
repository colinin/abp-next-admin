import type {
	Dictionary,
	ExtensibleAuditedEntityDto,
	ExtensibleObject,
	PagedAndSortedResultRequestDto,
} from "#/abp-core";

interface OpenIddictApplicationFeaturesDto {
	requirePkce?: boolean;
}

interface OpenIddictApplicationRequirementsDto {
	features: OpenIddictApplicationFeaturesDto;
}

interface OpenIddictApplicationTokenLifetimesDto {
	accessToken?: number;
	authorizationCode?: number;
	deviceCode?: number;
	identityToken?: number;
	refreshToken?: number;
	userCode?: number;
}

interface OpenIddictApplicationSettingsDto {
	tokenLifetime: OpenIddictApplicationTokenLifetimesDto;
}

interface OpenIddictApplicationGetListInput extends PagedAndSortedResultRequestDto {
	filter?: string;
}

interface OpenIddictApplicationCreateOrUpdateDto extends ExtensibleObject {
	applicationType?: string;
	clientId: string;
	clientSecret?: string;
	clientType?: string;
	clientUri?: string;
	consentType?: string;
	displayName?: string;
	displayNames?: Dictionary<string, string>;
	endpoints?: string[];
	grantTypes?: string[];
	logoUri?: string;
	postLogoutRedirectUris?: string[];
	properties?: Dictionary<string, string>;
	redirectUris?: string[];
	requirements: OpenIddictApplicationRequirementsDto;
	responseTypes?: string[];
	scopes?: string[];
	settings?: OpenIddictApplicationSettingsDto;
}

type OpenIddictApplicationCreateDto = OpenIddictApplicationCreateOrUpdateDto;

type OpenIddictApplicationUpdateDto = OpenIddictApplicationCreateOrUpdateDto;

interface OpenIddictApplicationDto extends ExtensibleAuditedEntityDto<string> {
	applicationType?: string;
	clientId: string;
	clientSecret?: string;
	clientType?: string;
	clientUri?: string;
	consentType?: string;
	displayName?: string;
	displayNames?: Dictionary<string, string>;
	endpoints?: string[];
	grantTypes?: string[];
	logoUri?: string;
	postLogoutRedirectUris?: string[];
	properties?: Dictionary<string, string>;
	redirectUris?: string[];
	requirements: OpenIddictApplicationRequirementsDto;
	responseTypes?: string[];
	scopes?: string[];
	settings: OpenIddictApplicationSettingsDto;
}

export type {
	OpenIddictApplicationCreateDto,
	OpenIddictApplicationDto,
	OpenIddictApplicationGetListInput,
	OpenIddictApplicationUpdateDto,
};
