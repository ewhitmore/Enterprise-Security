module App.Blocks {
    'use strict';

    export interface IAuthDataDto {
        isAuth: boolean;
        userName: string;
        password:string;
        useRefreshTokens: boolean;
        token :string;
    }

    export class AuthDataDto implements IAuthDataDto {
        isAuth: boolean;
        userName: string;
        password:string;
        useRefreshTokens: boolean;
        token: string;
    }
}