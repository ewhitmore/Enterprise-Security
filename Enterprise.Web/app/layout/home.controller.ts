module App.Layout {
    'use strict';

    import Teacher = App.Teacher.ITeacherDto;

    interface IHomeScope {
        // Properties
        fullname: string;
        teachers: Teacher[];

        // Methods
        getall():angular.IPromise<Teacher[]>;
      
    }

    class HomeController implements IHomeScope {

        fullname: string;
        teachers: Teacher[];

        static $inject = ['app.teacher.teacherService'];
        constructor(private teacherService: App.Teacher.ITeacherService) {
            var vm = this;

            vm.fullname = "Eric Whitmore";
            vm.getall().then(results => {
                vm.teachers = results;
            });
        }

        getall(): angular.IPromise<App.Teacher.ITeacherDto[]> {
            return this.teacherService.getAll();
        }
    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.homeController", HomeController);
}