module App.Config {
    'use strict';

    angular.module('app')
        .config(locationProviderConfig);

    locationProviderConfig.$inject = ["$locationProvider"];
    function locationProviderConfig($locationProvider: angular.ILocationProvider): void {
        $locationProvider.html5Mode(true);
    }


}