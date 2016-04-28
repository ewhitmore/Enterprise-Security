module App.Layout {
    'use strict';

    interface IHomeScope {
        // Properties
        fullname: string;
        teachers: App.Teacher.ITeacherDto[];
        authDataDto: Blocks.IAuthDataDto;

        // Methods
        getall(): angular.IPromise<App.Teacher.ITeacherDto[]>;
        //login(username: string, password: string, refreshTokens: boolean): angular.IPromise<any>;
        submit(authDataDto: Blocks.IAuthDataDto) : void;
      
    }

    class HomeController implements IHomeScope {

        fullname: string;
        teachers: Teacher.ITeacherDto[];
        authDataDto = {} as Blocks.IAuthDataDto;

        static $inject = ['app.teacher.teacherService', 'app.blocks.authenticationService'];
        constructor(private teacherService: Teacher.ITeacherService, private authenticationService: Blocks.IAuthenicationService) {
            var vm = this;

            vm.fullname = "Eric Whitmore";
            vm.getall().then(results => {
                vm.teachers = results;
            });
        }

        getall(): angular.IPromise<Teacher.ITeacherDto[]> {
            return this.teacherService.getAll();
        }



        //login(username: string, password: string, refreshTokens: boolean): angular.IPromise<any> {
        //    var authDataDto = {
        //        userName: username,
        //        password: password,
        //        useRefreshTokens: refreshTokens
        //    } as Blocks.IAuthDataDto;

        //    return this.authenticationService.login(authDataDto);
        //}

        submit(authDataDto: Blocks.IAuthDataDto): void {
        console.log(authDataDto);
            this.authenticationService.login(authDataDto);
        }


    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.homeController", HomeController);
}