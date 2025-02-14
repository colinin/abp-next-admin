import type {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

/** 消息状态 */
export enum MessageStatus {
  /** 发送失败 */
  Failed = 10,
  /** 未发送 */
  Pending = -1,
  /** 已发送 */
  Sent = 0,
}
/** 邮件优先级 */
export enum MailPriority {
  /** 高 */
  High = 2,
  /** 低 */
  Low = 1,
  /** 普通 */
  Normal = 0,
}

interface MessageDto extends AuditedEntityDto<string> {
  /** 消息内容 */
  content: string;
  /** 消息发布者 */
  provider?: string;
  /** 错误原因 */
  reason?: string;
  /** 接收方 */
  receiver: string;
  /** 发送次数 */
  sendCount: number;
  /** 发送人 */
  sender?: string;
  /** 发送时间 */
  sendTime?: Date;
  /** 状态 */
  status: MessageStatus;
  /** 发送人Id */
  userId?: string;
}
/** 邮件附件 */
interface EmailMessageAttachmentDto {
  /** 附件存储名称 */
  blobName: string;
  /** 附件名称 */
  name: string;
  /** 附件大小 */
  size: number;
}
/** 邮件标头 */
interface EmailMessageHeaderDto {
  /** 键名 */
  key: string;
  /** 键值 */
  value: string;
}
/** 邮件消息 */
interface EmailMessageDto extends MessageDto {
  attachments: EmailMessageAttachmentDto[];
  cc?: string;
  from?: string;
  headers: EmailMessageHeaderDto[];
  isBodyHtml: boolean;
  normalize: boolean;
  priority?: MailPriority;
  subject?: string;
}

interface EmailMessageGetListInput extends PagedAndSortedResultRequestDto {
  beginSendTime?: Date;
  content?: string;
  emailAddress?: string;
  endSendTime?: Date;
  from?: string;
  priority?: MailPriority;
  subject?: string;
}

type SmsMessageDto = MessageDto;

interface SmsMessageGetListInput extends PagedAndSortedResultRequestDto {
  beginSendTime?: Date;
  content?: string;
  endSendTime?: Date;
  phoneNumber?: string;
}

export type {
  EmailMessageDto,
  EmailMessageGetListInput,
  SmsMessageDto,
  SmsMessageGetListInput,
};
