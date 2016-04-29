module App.Blocks {
    'use static';

    interface IRefreshController {
        tokenRefreshed: boolean;
        tokenResponse: string;
        authDataDto: Blocks.IAuthDataDto;
        refreshToken():void;

    }

    class RefreshController implements IRefreshController {

        tokenRefreshed: boolean;
        tokenResponse: string;
        authDataDto: Blocks.IAuthDataDto;

        static $inject = ['$location', 'app.blocks.authenticationService'];
        constructor(
            private $location: angular.ILocationService,
            private authenticationService: Blocks.IAuthenicationService) {
            this.authDataDto = authenticationService.authDataDto;
        }

        refreshToken(): void {
            this.authenticationService.refreshToken()
                .then(response => {
                        this.tokenRefreshed = true;
                        this.tokenResponse = response;
                    },
                    error => {
                        console.log("Error:", error);
                        this.$location.path('/login');
                });
        }
    }

    // Hook my ts class into an angularjs module
    angular.module("app.blocks")
        .controller("app.blocks.refreshController", RefreshController);
}