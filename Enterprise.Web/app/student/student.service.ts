module App.Student {
    "use strict";

    export interface IStudentService {
        getAll(): angular.IPromise<Student.IStudentDto[]>;
        save(teacherDto: Student.IStudentDto): angular.IPromise<Student.IStudentDto>;
    }

    class StudentService implements IStudentService {
        constructor(private $http: angular.IHttpService, private apiEndpoint: Blocks.IApiEndpointConfig) { }

        getAll(): angular.IPromise<Student.IStudentDto[]> {
            return this.$http
                .get(this.apiEndpoint.baseUrl + '/v1/student/GetAll')
                .then((response: angular.IHttpPromiseCallbackArg<Student.IStudentDto[]>): Student.IStudentDto[] => {
                    return response.data;
                });
        }

        save(studentDto: Student.IStudentDto): angular.IPromise<Student.IStudentDto> {
            return this.$http
                .post(this.apiEndpoint.baseUrl + '/v1/student/', studentDto)
                .then((response: angular.IHttpPromiseCallbackArg<Student.IStudentDto>): Student.IStudentDto => {
                    return response.data;
                });
        }
    }

    factory.$inject = [
        '$http',
        'app.blocks.ApiEndpoint'
    ];
    function factory($http: ng.IHttpService,
        apiEndpoint: Blocks.IApiEndpointConfig): IStudentService {
        return new StudentService($http, apiEndpoint);
    }

    angular
        .module('app.student')
        .factory('app.student.studentService',
        factory);
}