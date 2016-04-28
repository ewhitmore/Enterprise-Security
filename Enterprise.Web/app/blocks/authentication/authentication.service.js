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
            AuthenticationService.prototype.login = function (authDataDto) {
                //var data = "grant_type=password&username=" + authDataDto.userName + "&password=" + authDataDto.password;
                var data = "grant_type=password&username=ewhitmore&password=ewhitmore";
                if (authDataDto.useRefreshTokens) {
                    data = data + "&client_id=AngularWebClient";
                }
                return this.$http
                    .post('/token', data, "{ headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }")
                    .then(function (response) {
                    //var token = new Token();
                    //token.accessToken = response.data.accessToken;
                    //token.clientId = response.data.clientId;
                    //token.expires = response.data.expires;
                    //return token;
                    return response.data;
                });
            };
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
