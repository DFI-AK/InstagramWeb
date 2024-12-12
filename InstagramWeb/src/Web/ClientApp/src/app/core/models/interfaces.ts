import { BaseUserDto } from "src/app/web-api-client";

export interface ChatDto {
    user: BaseUserDto
    messages: ReadonlyArray<Message>;
}

export interface Message {
    messageId: string;
    textMessage: string;
    messageStatus: number;
    sentAt: Date;
    sender: Receiver;
    receiver: Receiver;
    isMine:boolean
}

export interface Receiver {
    userId: string;
    fullName: FullName;
    emailAddress: string;
    contactNumber: null;
    joinAt: Date;
    followers: Follower[];
    followings: Following[];
    followerCount: number;
    followingCount: number;
}

export interface Follower {
    followerId: string;
}

export interface Following {
    followedId: string;
}

export enum FullName {
    AyushKumar = "AYUSH.KUMAR ",
    Sqlgig = "SQLGIG ",
}
