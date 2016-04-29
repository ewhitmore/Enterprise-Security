module App.Blocks {
    'use static';

    interface ILoginController {
        authDataDto: Blocks.IAuthDataDto;
        login(authDataDto: Blocks.IAuthDataDto): void;
        message: string;
    }

    class LoginController implements ILoginController {
        authDataDto = {} as Blocks.IAuthDataDto;
        message: string;

        static $inject = ['$location', 'app.blocks.authenticationService'];
        constructor(private $location: angular.ILocationService, private authenticationService: Blocks.IAuthenicationService) { }

        // Login and Bearer Token
        login(authDataDto: Blocks.IAuthDataDto): void {
            this.authenticationService.login(authDataDto)
                .then(() => {
                    this.$location.path('/home');
                }, err => {
                    this.message = err.data.error_description;
                });
        }
    }


    // Hook my ts class into an angularjs module
    angular.module("app.blocks")
        .controller("app.blocks.loginController", LoginController);
}