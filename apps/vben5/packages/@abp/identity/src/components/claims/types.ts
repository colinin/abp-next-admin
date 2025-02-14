import type { ListResultDto } from '@abp/core';

import type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimDto,
  IdentityClaimUpdateDto,
} from '../../types/claims';

interface ClaimEditModalProps {
  /** 新增声明api */
  createApi: (input: IdentityClaimCreateDto) => Promise<void>;
  /** 新增声明权限代码 */
  createPolicy: string;
  /** 删除声明api */
  deleteApi: (input: IdentityClaimDeleteDto) => Promise<void>;
  /** 删除声明权限代码 */
  deletePolicy: string;
  /** 更新声明api */
  updateApi: (input: IdentityClaimUpdateDto) => Promise<void>;
  /** 更新声明权限代码 */
  updatePolicy: string;
}

interface ClaimModalProps extends ClaimEditModalProps {
  /** 加载声明列表api */
  getApi: () => Promise<ListResultDto<IdentityClaimDto>>;
}

interface ClaimModalEvents {
  (event: 'change', data: IdentityClaimDto): void;
  (event: 'onDelete'): void;
}

export type { ClaimEditModalProps, ClaimModalEvents, ClaimModalProps };
