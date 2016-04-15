module App.Teacher {
    "use strict";

    export interface ITeacherService {
        getAll(): angular.IPromise<Teacher.ITeacherDto[]>;
    }

    class TeacherService implements ITeacherService {
        constructor(private $http: angular.IHttpService, private apiEndpoint: Blocks.IApiEndpointConfig) { }

        getAll(): angular.IPromise<Teacher.ITeacherDto[]> {
            return this.$http
                .get(this.apiEndpoint.baseUrl + '/v1/teacher/GetAll')
                .then((response: angular.IHttpPromiseCallbackArg<ITeacherDto[]>): ITeacherDto[] => {
                    return response.data;
                });
        }
    }

    factory.$inject = [
        '$http',
        'app.blocks.ApiEndpoint'
    ];
    function factory($http: ng.IHttpService,
        apiEndpoint: Blocks.IApiEndpointConfig): ITeacherService {
        return new TeacherService($http, apiEndpoint);
    }

    angular
        .module('app.teacher')
        .factory('app.teacher.teacherService',
        factory);
}