(function () {
    'use strict';

    angular
        .module('app')
        .controller('shell', shell);

    angular.module('app').filter('startFrom', function () {
        return function (input, start) {
            if (input) {
                start = +start; //parse to int
                return input.slice(start);
            }
            return [];
        }
    });

    shell.$inject = ['$location','$scope', '$rootScope']; 

    function shell($location, $scope, $rootScope) {
        /* jshint validthis:true */
        var vm = this;
        $rootScope.loading = 0;
        vm.title = 'shell';

        var path = $location.path();
        vm.showLogo = function () {
            return $location.path().indexOf('create') < 0;
        };

        activate();

        function activate() { }
    }
})();
