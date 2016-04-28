module App.Blocks {
    "use static";

    angular.module("app")
        .config(config);

    config.$inject = ["app.blocks.ApiEndpointProvider"];
    function config(apiEndpointProvider: Blocks.IApiEndpointProvider): void {
        // Configure the API server URL here
        apiEndpointProvider.configure("/api");
    }
}