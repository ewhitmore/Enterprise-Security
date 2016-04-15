module App.Teacher {
    "use strict";

    export interface ITeacherDto {
        id: number;
        fullname: string;
        birthday: Date;
    }

    export class TeacherDto implements ITeacherDto {
        id: number;
        fullname: string;
        birthday: Date;
    }
}