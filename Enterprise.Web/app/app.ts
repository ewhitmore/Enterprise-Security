module App.Config {
    'use strict';

    angular.module('app')
        .config(locationProviderConfig)
        .config(httpProviderConfig);

    locationProviderConfig.$inject = ["$locationProvider"];
    function locationProviderConfig($locationProvider: angular.ILocationProvider): void {
        $locationProvider.html5Mode(true);
    }

    httpProviderConfig.$inject = ["$httpProvider"];
    function httpProviderConfig($httpProvider : angular.IHttpProvider): void {
        $httpProvider.interceptors.push('app.services.authInterceptorService');
    }


}