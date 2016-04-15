module App.Layout {
    'use strict';

    interface INavigationScope {

    }

    class NavigationController implements INavigationScope {

    }

    // Hook my ts class into an angularjs module
    angular.module("app.layout")
        .controller("app.layout.navigationController", NavigationController);
}