(function () {
    'use strict';

    angular
        .module('app')
        .controller('reportController', reportController);


    reportController.$inject = ['$location', 'rewardService'];

    function reportController($location, rewardService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'reportController';

        activate();

        function activate() {
            getCurrentUser();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {
                        vm.isLeader = data.data.isLeader;
                    }
                    $rootScope.loading--;
                });
        }

    }
})();
