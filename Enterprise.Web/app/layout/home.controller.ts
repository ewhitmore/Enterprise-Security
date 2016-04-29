module App.Layout {
    'use strict';

    interface IHomeScope {
        // Properties
        teachers: App.Teacher.ITeacherDto[];

        // Methods
        getall(): angular.IPromise<App.Teacher.ITeacherDto[]>;

       
    }

    class HomeController implements IHomeScope {

        fullname: string;
        teachers: Teacher.ITeacherDto[];
        authDataDto = {} as Blocks.IAuthDataDto;

        static $inject = ['app.teacher.teacherService'];
        constructor(private teacherService: Teacher.ITeacherService) {
            var vm = this;

            vm.getall().then(results => {
                vm.teachers = results;
            });
        }

        getall(): angular.IPromise<Teacher.ITeacherDto[]> {
            return this.teacherService.getAll();
        }



    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.homeController", HomeController);
}