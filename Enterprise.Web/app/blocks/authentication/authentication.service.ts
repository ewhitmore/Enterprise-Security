module App.Blocks {
    'use static';

    export interface IAuthenicationService {
        authDataDto: IAuthDataDto;
        login(authDataDto: IAuthDataDto): angular.IPromise<any>;
        logout(): void;
        fillAuthData(): void;
        refreshToken(): angular.IPromise<any>;
    }

    export class AuthenticationService {

        authDataDto: IAuthDataDto;

        constructor(
            private $http: angular.IHttpService,
            private apiEndpoint: Blocks.IApiEndpointConfig,
            private localStorageService: angular.local.storage.ILocalStorageService) {

            this.authDataDto = {
                userName: '',
                password: '',
                isAuth: false,
                useRefreshTokens: false
            } as IAuthDataDto;
        }

        /** Authenticate Client */
        login(authDto: IAuthDataDto): angular.IPromise<any> {
            this.authDataDto = authDto;

            var data = "grant_type=password&username=" + this.authDataDto.userName + "&password=" + this.authDataDto.password;

            if (this.authDataDto.useRefreshTokens) {
                data = data + "&client_id=AngularWebClient";
            }

            return this.$http
                .post('/token', data, "{ headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }")
                .then((response: angular.IHttpPromiseCallbackArg<any>): any => {
                    // Remove Password from memory
                    this.authDataDto.password = "";

                    // Success
                    if (response.status === 200) {
                        if (this.authDataDto.useRefreshTokens) {
                            this.localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: response.data.refresh_token, useRefreshTokens: true });
                        }
                        else {
                            this.localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: "", useRefreshTokens: false });
                        }
                        this.fillAuthData();
                        return response.data;

                        // Failure
                    } else {
                        this.logout();
                        return response.data;
                    }
                });
        }

        /** Remove Security Tokens from Client */
        logout(): void {
            this.localStorageService.remove('authorizationData');

            this.authDataDto.isAuth = false;
            this.authDataDto.userName = "";
            this.authDataDto.password = "";
            this.authDataDto.useRefreshTokens = false;
        }

        /** Populate AuthDataDto from local storage */
        fillAuthData(): void {
            var authData = this.localStorageService.get('authorizationData');

            if (authData) {
                this.authDataDto.isAuth = true;
                this.authDataDto.userName = authData['userName'];
                this.authDataDto.useRefreshTokens = authData['useRefreshTokens'];
            }
        }

        /** Get new Refresh Token */
        refreshToken(): angular.IPromise<any> {

            var authData = this.localStorageService.get('authorizationData');
            if (authData['useRefreshTokens']) {
                var data = "grant_type=refresh_token&refresh_token=" + authData['refreshToken'] + "&client_id=AngularWebClient";
                this.localStorageService.remove('authorizationData');

                return this.$http
                    .post('/token', data, "{ headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }")
                    .then((response: angular.IHttpPromiseCallbackArg<any>): any => {
                        // Remove Password from memory
                        this.authDataDto.password = "";

                        // Success
                        if (response.status === 200) {
                            this.localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: response.data.refresh_token, useRefreshTokens: true });
                            return response.data;

                            // Failure
                        } else {
                            this.logout();
                            return response.data;
                        }
                    });
            }
        }
    }



    factory.$inject = [
        '$http',
        'app.blocks.ApiEndpoint',
        'localStorageService'
    ];
    function factory($http: angular.IHttpService, apiEndpoint: Blocks.IApiEndpointConfig, localStorageService: angular.local.storage.ILocalStorageService): IAuthenicationService {
        return new AuthenticationService($http, apiEndpoint, localStorageService);
    }

    angular
        .module('app.blocks')
        .factory('app.blocks.authenticationService',
        factory);

}