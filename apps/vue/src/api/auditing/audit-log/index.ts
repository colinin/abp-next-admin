import { defHttp } from '/@/utils/http/axios';
import {
  AuditLogDto,
  AuditLogGetByPagedDto,
} from './model';

export const deleteById = (id: string) => {
    return defHttp.delete<void>({
        url: `/api/auditing/audit-log/${id}`
    });
};

export const get = (id: string) => {
    return defHttp.get<AuditLogDto>({
        url: `/api/auditing/audit-log/${id}`
    });
};

export const getList = (input: AuditLogGetByPagedDto) => {
    return defHttp.get<PagedResultDto<AuditLogDto>>({
        url: `/api/auditing/audit-log`,
        params: input,
    });
};
