module App.Blocks {
    'use static';

    export interface IAuthInterceptorService {

    }


    // TODO: refacter this into a more "typescripty" format
    class AuthInterceptorService {
        constructor(
            private $q: angular.IQService,
            private $injector: angular.auto.IInjectorService,
            private $location: angular.ILocationService,
            private localStorageService: angular.local.storage.ILocalStorageService) {

            var authInterceptorServiceFactory = {} as any;

            var _request = config => {

                config.headers = config.headers || {};

                var authData = localStorageService.get('authorizationData') as any;
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.token;
                }

                return config;
            }

            var _responseError = rejection => {
                if (rejection.status === 401) {
                    var authService = $injector.get('app.blocks.authenticationService') as App.Blocks.AuthenticationService;
                    var authData = localStorageService.get('authorizationData') as any;

                    if (authData) {
                        if (authData.useRefreshTokens) {
                            $location.path('/refresh');
                            return $q.reject(rejection);
                        }
                    }
                    authService.logout();
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = _request;
            authInterceptorServiceFactory.responseError = _responseError;

            return authInterceptorServiceFactory;
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