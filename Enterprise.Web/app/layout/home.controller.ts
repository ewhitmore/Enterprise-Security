module App.Layout {
    'use strict';

    interface IHomeScope {
        // Properties
        students: Student.IStudentDto[];

        // Methods
        getall(): void;
        save(teacher: Teacher.ITeacherDto): void;
       
    }

    class HomeController implements IHomeScope {

        fullname: string;
        students: Student.IStudentDto[];
        authDataDto = {} as Blocks.IAuthDataDto;

        static $inject = ['app.teacher.teacherService', 'app.student.studentService'];
        constructor(private teacherService: Teacher.ITeacherService, private studentService: Student.IStudentService) {
            this.getall();
        }

        getall(): void {
            this.studentService.getAll().then(students => {
                this.students = students;
            });
        }

        save(student: Student.IStudentDto): void {
            this.studentService.save(student).then(() => {
                this.getall();
            });
        }
    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.homeController", HomeController);
}