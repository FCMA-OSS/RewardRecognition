(function () {
    'use strict';

    var serviceId = 'app';
    var app = angular.module('app', [
        // Angular modules 
        //'ngAnimate',        // animations
        'ngRoute',          // routing
        'ngAnimate',
        'ngTouch',
        //'ui.grid.resizeColumns',
        //'ngSanitize',       // sanitizes html bindings (ex: sidebar.js)
        //'ngResource',
        //'LocalStorageModule',
        // Custom modules 
        //'common',           // common functions, logger, spinner
        //'common.bootstrap', // bootstrap dialog wrapper functions
        // 3rd Party Modules
        'ui.bootstrap'      // ui-bootstrap (ex: carousel, pagination, dialog)
    ]);

    // Handle routing errors and success events
    app.run(['$route', '$rootScope', '$q', 'authService', function ($route, $rootScope, $q, authService) {

        startRouting();

        function startRouting() {
            $rootScope.$on('$routeChangeStart', function (event, next, current) {

                $rootScope.appSettings = {};

                $rootScope.error = null;

                //if ($rootScope.user) {
                //    return true;
                //} else {
                    //$rootScope.user = {};
                    //$rootScope.user.manager = {};
                    //var deferred = $q.defer();

                    //checkRouting($q, $rootScope);

                    //return deferred.promise;
                //}

            });

            var checkRouting = function ($q, $rootScope) {
                var deferred = $q.defer();

                authService.getCurrentUser()
                .then(function (data) {
                    if (data.data) {
                        //$rootScope.user.isInRedeemGroup = data.data.IsInRedeemGroup;

                        //$rootScope.user.username = data.data.userName;
                        //$rootScope.user.userFullName = data.data.userFullName;
                        //$rootScope.user.manager.userName = data.data.manager.userName;
                    }
                });

                return deferred.promise;
            };
        }
    }]);

})();