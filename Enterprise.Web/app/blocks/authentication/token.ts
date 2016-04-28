module App.Blocks {
    'use strict';

    export interface IToken {
        expires: string;
        issued: string;
        accessToken: string;
        clientId: string;
        expiresIn: number;
        tokenType: string;
        userName: string;
    }

    export class Token implements IToken {
        expires: string;
        issued: string;
        accessToken: string;
        clientId: string;
        expiresIn: number;
        tokenType: string;
        userName: string;
    }
}