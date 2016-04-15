module App {
    "use strict";

    angular
        .module('app', [

            // Base Modules
             'app.core'
            ,'app.layout'
            ,'app.services'
            ,'app.widgets'
            ,'app.blocks'

            // Feature Modules
            , 'app.teacher' 
            

  
        ]);
}