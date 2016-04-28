var App;
(function (App) {
    var Blocks;
    (function (Blocks) {
        'use static';
        var AuthenticationService = (function () {
            function AuthenticationService($http, apiEndpoint) {
                this.$http = $http;
                this.apiEndpoint = apiEndpoint;
            }
            return AuthenticationService;
        }());
        factory.$inject = [
            '$http',
            'app.blocks.ApiEndpoint'
        ];
        function factory($http, apiEndpoint) {
            return new AuthenticationService($http, apiEndpoint);
        }
        angular
            .module('app.blocks')
            .factory('app.blocks.authenticationService', factory);
    })(Blocks = App.Blocks || (App.Blocks = {}));
})(App || (App = {}));
