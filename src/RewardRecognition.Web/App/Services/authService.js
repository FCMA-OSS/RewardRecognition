(function () {
    'use strict';

    angular.module('app')
    .factory('authService', function ($http, $q) {
        var current = undefined;

        var _getCurrentUser = function () {
            if (current) {
                return current;
            }
            var promise = $http.get("api/Reward/GetCurrentUser")
            .success(function (data) {
                current = data;
                return data;
            });
            return promise;
    };
        
    return {
        getCurrentUser: _getCurrentUser
    };
});
})();