export enum Sex {
  Male,
  Female,
  Other,
}

export interface UserCard {
  userId: string;
  userName: string;
  avatarUrl: string;
  nickName: string;
  age: number;
  sex: Sex;
  sign: string;
  description: string;
  birthday?: Date;
}
