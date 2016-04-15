module App.Blocks {
    'user strict';

    export interface IApiEndpointConfig {
        baseUrl: string;
    }

    export interface IApiendpointProvider {
        configure(baseUrl: string): void;
    }

    class ApiendpointProvider implements angular.IServiceProvider, IApiendpointProvider {
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
        .module('app.blocks')
        .provider('app.blocks.ApiEndpoint', ApiendpointProvider);


}
