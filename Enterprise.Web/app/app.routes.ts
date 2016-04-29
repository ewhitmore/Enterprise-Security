module App.Config {
    'use strict';

    angular.module('app').config(routeConfig);

    routeConfig.$inject = ["$routeProvider"];
    function routeConfig($routeProvider: angular.route.IRouteProvider): void {
        $routeProvider
            
            .when("/", { templateUrl: '/app/layout/home.tpl.html', controller: 'app.layout.homeController', controllerAs: 'vm' })
            .when("/login", { templateUrl: '/app/blocks/authentication/login.tpl.html', controller: 'app.blocks.loginController', controllerAs: 'vm' })
            .when("/refresh", { templateUrl: '/app/blocks/authentication/refresh.tpl.html', controller: 'app.blocks.refreshController', controllerAs: 'vm' })
            .otherwise("/");
        
    }
}