module App.Blocks {
    "user strict";

    export interface IApiEndpointConfig {
        baseUrl: string;
    }

    export interface IApiEndpointProvider {
        configure(baseUrl: string): void;
    }

    class ApiEndpointProvider implements angular.IServiceProvider, IApiEndpointProvider {
        config: IApiEndpointConfig;

        configure(baseUrl: string): void {
            this.config = {
                baseUrl: baseUrl
            }
        }

        $get(): IApiEndpointConfig {
            return this.config;
        }

    }

    angular
        .module("app.blocks")
        .provider("app.blocks.ApiEndpoint", ApiEndpointProvider);
}
