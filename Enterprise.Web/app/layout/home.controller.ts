module App.Layout {
    'use strict';

    import Teacher = App.Teacher.ITeacherDto;
    import AuthDataDto = App.Blocks.AuthDataDto;

    interface IHomeScope {
        // Properties
        fullname: string;
        teachers: Teacher[];

        // Methods
        getall(): angular.IPromise<Teacher[]>;
        login(): angular.IPromise<any>;
        check() : void;
      
    }

    class HomeController implements IHomeScope {

        fullname: string;
        teachers: Teacher[];

        static $inject = ['app.teacher.teacherService', 'app.blocks.authenticationService'];
        constructor(private teacherService: App.Teacher.ITeacherService, private authenticationService: App.Blocks.IAuthenicationService) {
            var vm = this;

            vm.fullname = "Eric Whitmore";
            vm.getall().then(results => {
                vm.teachers = results;
            });

           
        }

        getall(): angular.IPromise<App.Teacher.ITeacherDto[]> {
            return this.teacherService.getAll();
        }

        login(): angular.IPromise<any> {
            var data = new AuthDataDto();

            return this.authenticationService.login(data);
        }

        check(): void {
            this.login().then(data => {
                console.log(data);
            });
        }


    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.homeController", HomeController);
}