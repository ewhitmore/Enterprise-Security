module App.Config {
    'use strict';

    angular.module('app').config(routeConfig);

    routeConfig.$inject = ["$routeProvider"];
    function routeConfig($routeProvider: angular.route.IRouteProvider): void {
        $routeProvider
            
            .when("/", { templateUrl: '/app/layout/home.tpl.html', controller:'app.layout.homeController', controllerAs: 'vm' })
            .otherwise("/");
        
    }
}