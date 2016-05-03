module App.Blocks {
    'use static';

    export interface IAuthInterceptorService {

        request(config: any): angular.IPromise<any>;
        responseError(rejection: any): angular.IPromise<any>;
    }



    // TODO: refacter this into a more "typescripty" format
    class AuthInterceptorService {
        
        constructor(private $q: angular.IQService, private $injector: angular.auto.IInjectorService, private $location: angular.ILocationService, private localStorageService: angular.local.storage.ILocalStorageService) {
            
        }

        request = (config) => {
            config.headers = config.headers || {};

            var authData = this.localStorageService.get('authorizationData') as any;
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        responseError = (rejection) => {
            if (rejection.status === 401) {
                var authService = this.$injector.get('app.blocks.authenticationService') as App.Blocks.AuthenticationService;
                var authData = this.localStorageService.get('authorizationData') as any;

                if (authData) {
                    if (authData.useRefreshTokens) {
                        this.$location.path('/refresh');
                        return this.$q.reject(rejection);
                    }
                }
                authService.logout();
                this.$location.path('/login');
            }
            return this.$q.reject(rejection);
        }
       
    }

    factory.$inject = ['$q', '$injector', '$location', 'localStorageService'];
    function factory($q: angular.IQService, $injector: angular.auto.IInjectorService, $location: angular.ILocationService, localStorageService: angular.local.storage.ILocalStorageService): IAuthInterceptorService {
        return new AuthInterceptorService($q, $injector, $location, localStorageService);
    }

    angular
        .module('app.services')
        .factory('app.services.authInterceptorService',
        factory);
}