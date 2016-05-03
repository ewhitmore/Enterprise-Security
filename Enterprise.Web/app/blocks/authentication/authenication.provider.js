//module App.Blocks {
//    "user strict";
//    export interface IAuthenicationConfig {
//        clientId: string;
//    }
//    export interface IAuthenicationProvider {
//        configure(clientId: string): void;
//    }
//    class AuthenicationProvider implements angular.IServiceProvider, IAuthenicationProvider {
//        config: IAuthenicationConfig;
//        configure(clientId: string): void {
//            this.config = {
//                clientId: clientId
//            }
//        }
//        $get(): IAuthenicationConfig {
//            return this.config;
//        }
//    }
//    angular
//        .module("app.blocks")
//        .provider("app.blocks.AuthenicationProvider", AuthenicationProvider);
//}
//# sourceMappingURL=authenication.provider.js.map