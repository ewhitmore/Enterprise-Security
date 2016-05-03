module App.Student {
    "use strict";

    export interface IStudentDto {
        id: number;
        fullname: string;
        birthday: Date;
    }

    export class StudentDto implements IStudentDto {
        id: number;
        fullname: string;
        birthday: Date;
    }
}