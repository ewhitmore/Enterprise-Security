///<reference path="../../Scripts/typings/jasmine/jasmine.d.ts"/>
///<reference path="teacherDto.ts"/>

module App.Teacher {
    "use strict";

    describe("TeacherDto", () => {

        var teacher: ITeacherDto;
        var birthday = new Date("2016-01-15");
        beforeEach(() => {
            teacher = new TeacherDto();
            teacher.id = 1;
            teacher.fullname = "John Doe";
            teacher.birthday = birthday;
        });

        it("should have id of 1", () => {
            expect(teacher.id).toBe(1);
        });

        it("should have fullname of John Doe", () => {
            expect(teacher.fullname).toBe("John Doe");
        });

        it("should have a birthday of 2016-01-15", () => {
            expect(teacher.birthday).toBe(birthday);
        });

    });
}