(function () {
    'use strict';

    angular
        .module('app')
        .controller('redeemController', redeemController);

    redeemController.$inject = ['$scope','$rootScope', '$location', '$timeout', 'rewardService'];

    function redeemController($scope, $rootScope, $location, $timeout, rewardService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'redeemController';

        vm.found = true;
        vm.clicked = false;

        activate();

        function activate() {
            getCurrentUser();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {

                        //if ($rootScope.user) {
                        //} else {
                        //    $rootScope.user = {};
                        //    $rootScope.user.manager = {};
                        //    $rootScope.user = data.data;
                        //}

                        vm.isInRedeemGroup = data.data.isInRedeemGroup;
                        vm.currentUser = data.data.userName;
                    }
                    $rootScope.loading--;
                });
        }

        vm.redeemSearch = function (id) {
            vm.clicked = true;
            vm.found = false;
            $rootScope.loading++;
            return rewardService.getApprovedRewardById(id)
                .then(function (data) {
                    if (data !== null) {
                        vm.found = true;
                        vm.reward = data;
                        vm.reward.presenter = data.createdByFullName;
                        vm.reward.recipientFullName = data.recipientFullName;
                        vm.reward.rewardDescription = data.rewardType.description;
                        vm.id = "";
                    }
                    else {
                        vm.reward = {};
                        vm.wrongId = id;
                    }
                    $rootScope.loading--;
                }, function (error) {
                    vm.found = false;
                    vm.reward = {};
                    vm.wrongId = id;
                    var x = error;
                    $rootScope.loading--;
            });
        }

        $scope.alerts = [];

        vm.redeem = function () {
            $rootScope.loading++;
            return rewardService.UpdateRewardStatus(vm.reward.rewardID, "R")
                    .then(function (data) {
                        vm.found = true;
                        vm.clicked = false;
                        vm.reward = {};
                        vm.id = "";                        
                        $scope.alerts.push({ msg: 'Redemption succeeded.', type: 'success' });

                        $rootScope.loading--;
                        $timeout(function () {
                            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
                        }, 10000);

                    }, function (error) {
                        var x = error;
                        $scope.alerts.push({ msg: 'Redemption failed!', type: 'danger' });

                        $rootScope.loading--;
                        $timeout(function () {
                            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
                        }, 10000);
                    });

            
        }

        vm.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }
})();
