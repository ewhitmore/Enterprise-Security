module App.Blocks {
    'use static';

    export interface IAuthenicationService {
        login(authDataDto: AuthDataDto):angular.IPromise<any>;
    }
    
    class AuthenticationService {
        constructor(private $http: angular.IHttpService, private apiEndpoint: Blocks.IApiEndpointConfig) { }

        login(authDataDto: AuthDataDto): angular.IPromise<any> {
            
            //var data = "grant_type=password&username=" + authDataDto.userName + "&password=" + authDataDto.password;

            var data = "grant_type=password&username=ewhitmore&password=ewhitmore";

            if (authDataDto.useRefreshTokens) {
                data = data + "&client_id=AngularWebClient";
            }

            return this.$http
                .post('/token', data,"{ headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }")
                .then((response: angular.IHttpPromiseCallbackArg<any>): any => {

                    //var token = new Token();
                    //token.accessToken = response.data.accessToken;
                    //token.clientId = response.data.clientId;
                    //token.expires = response.data.expires;

                    //return token;

                    return response.data;
                });


      
        }
    }

    factory.$inject = [
        '$http',
        'app.blocks.ApiEndpoint'
    ];
    function factory($http: ng.IHttpService,
        apiEndpoint: Blocks.IApiEndpointConfig): IAuthenicationService {
        return new AuthenticationService($http, apiEndpoint);
    }

    angular
        .module('app.blocks')
        .factory('app.blocks.authenticationService',
        factory);

}