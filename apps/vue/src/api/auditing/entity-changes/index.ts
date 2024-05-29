import { defHttp } from '/@/utils/http/axios';
import {
    EntityChangeDto,
    EntityChangeGetByPagedDto,
    EntityChangeWithUsernameDto,
    EntityChangeGetWithUsernameInput
} from './model';

export const GetAsyncById = (id: string) => {
    return defHttp.get<EntityChangeDto>({
        url: `/api/auditing/entity-changes/${id}`
    });
};

export const GetListAsyncByInput = (input: EntityChangeGetByPagedDto) => {
    return defHttp.get<PagedResultDto<EntityChangeDto>>({
        url: `/api/auditing/entity-changes`,
        params: input,
    });
};

export const GetWithUsernameAsyncById = (id: string) => {
    return defHttp.get<EntityChangeWithUsernameDto>({
        url: `/api/auditing/entity-changes/with-username/${id}`
    });
};

export const GetWithUsernameAsyncByInput = (input: EntityChangeGetWithUsernameInput) => {
    return defHttp.get<ListResultDto<EntityChangeWithUsernameDto>>({
        url: `/api/auditing/entity-changes/with-username`,
        params: input,
    });
};
