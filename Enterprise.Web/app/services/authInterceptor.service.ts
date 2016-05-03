module App.Blocks {
    'use static';

    export interface IAuthInterceptorService {

        request(config: any): angular.IPromise<any>;
        response(response: any): angular.IPromise<any>;
        responseError(rejection: any): angular.IPromise<any>;

    }

    class AuthInterceptorService {
        constructor(
            private $q: angular.IQService,
            private $injector: angular.auto.IInjectorService,
            private $location: angular.ILocationService,
            private localStorageService: angular.local.storage.ILocalStorageService) {
        }

        request = (config) => {
            config.headers = config.headers || {};

            var authData = this.localStorageService.get('authorizationData') as any;
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        requestError = (rejection) => {
            return rejection;
        }

        response = (response) => {
            return response;
        }

        responseError = (rejection) => {

            // Recieved a 401 (Unauthorized)
            if (rejection.status === 401) {

                var authService = this.$injector.get('app.blocks.authenticationService') as Blocks.AuthenticationService;
                var authData = this.localStorageService.get('authorizationData') as any;

                // If using refresh tokens get a new one and try again or send to login screen
                if (authData && authData.useRefreshTokens) {

                    return authService.refreshToken().then(() => {
                        var $http = this.$injector.get('$http') as angular.IHttpService;
                        return $http(rejection.config);
                    });
                } else {
                    authService.logout();
                    this.$location.path('/login');
                    return this.$q.reject(rejection);
                }
            }
            return rejection;
        }
    }

    factory.$inject = ['$q', '$injector', '$location', 'localStorageService'];
    function factory($q: angular.IQService, $injector: angular.auto.IInjectorService, $location: angular.ILocationService,  localStorageService: angular.local.storage.ILocalStorageService): IAuthInterceptorService {
        return new AuthInterceptorService($q, $injector, $location, localStorageService);
    }

    angular
        .module('app.services')
        .factory('app.services.authInterceptorService',
        factory);
}