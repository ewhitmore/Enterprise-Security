module App.Blocks {
    'use static';

    angular.module('app')
        .config(config);

    config.$inject = ['app.blocks.ApiEndpointProvider'];
    function config(apiEndpointProvider: Blocks.IApiendpointProvider): void {
        apiEndpointProvider.configure('/api');
    }
}