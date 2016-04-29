module App.Layout {
    'use strict';

    interface INavigationScope {
        // Properties
        authDataDto: Blocks.IAuthDataDto;

        // Methods
        logout() : void;
    }

    class NavigationController implements INavigationScope {
        authDataDto: Blocks.IAuthDataDto;

        static $inject = ['$scope','$location','app.blocks.authenticationService'];
        constructor(
            private $scope: angular.IScope,
            private $location: angular.ILocationService,
            private authenticationService: Blocks.IAuthenicationService) {

            authenticationService.fillAuthData();
            this.authDataDto = authenticationService.authDataDto;

            this.authWatch($scope, authenticationService);
        }

        logout(): void {
            this.authenticationService.logout();
            this.$location.path('/login');
        }

        // Watch for authenication changes to update menu
        private authWatch(scope, authenticationService) {
            scope.$watch(
                () => authenticationService.authDataDto.isAuth,
                (oldValue: boolean, newValue: boolean) => {
                    if (oldValue !== newValue) {
                        this.authDataDto = authenticationService.authDataDto;
                    }
                });
        }
    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.navigationController", NavigationController);
}