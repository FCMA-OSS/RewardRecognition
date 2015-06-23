(function () {
    'use strict';

    angular
        .module('app')
        .controller('myRewardsController', myRewardsController);

    myRewardsController.$inject = ['$location', '$scope', '$rootScope', '$modal', 'rewardService', 'authService'];

    function myRewardsController($location, $scope, $rootScope, $modal, rewardService, authService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'myRewardsController';

        activate();

        function activate() {
            getRewardTypes();
            getRewardStatuses();
            getMyRewards();
            getCurrentUser();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {
                        vm.currentUser = data.data.userName;
                        $rootScope.loading--;
                        //$rootScope.user = {};
                        //$rootScope.user.manager = {};
                        //$rootScope.user.directReports = {};
                        //$rootScope.user.directReports = data.data.directReports;
                        //$rootScope.user.emailAddress = data.data.emailAddress;
                        //$rootScope.user.isInRedeemGroup = data.data.isInRedeemGroup;
                        //$rootScope.user.isLeader = data.data.isLeader;
                        //$rootScope.user.jobTitle = data.data.jobTitle;
                        //$rootScope.user.manager.fullName = data.data.manager.fullName;
                        //$rootScope.user.manager.userName = data.data.manager.userName;
                        //$rootScope.user.officeLocation = data.data.officeLocation;
                        //$rootScope.user.userFullName = data.data.userFullName;
                        //$rootScope.user.userName = data.data.userName;
                    }
                });
        }

        function getRewardTypes() {
            $rootScope.loading++;
            rewardService.getRewardTypes()
               .then(function (data) {
                   vm.rewardTypes = [];
                   data.data.forEach(function (item) {
                       vm.rewardTypes.push(item);
                   });
                   $rootScope.loading--;
               }, function (error) {
                   var x = error;
                   $rootScope.loading--;
               });
        }

        function getRewardStatuses() {
            $rootScope.loading++;
            rewardService.getRewardStatuses()
               .then(function (data) {
                   vm.rewardStatuses = [];
                   data.data.forEach(function (item) {
                       vm.rewardStatuses.push(item);
                   });
                   $rootScope.loading--;
               }, function (error) {
                   var x = error;
                   $rootScope.loading--;
               });
        }

        function getMyRewards() {
            $rootScope.loading++;
            rewardService.getMyRewards()
                .then(function (data) {
                    vm.rewards = [];
                    data.forEach(function (item) {
                        //this needs to happen before the data reaches the user.
                        mapFields(item);
                        vm.rewards.push(item);
                    })
                    vm.rewards.reverse();
                    setUpPaging();
                    $rootScope.loading--;
                });
        }

        function mapFields(rewardItem) {
            if (vm.rewardTypes) {
                vm.rewardTypes.forEach(function (rewardType) {
                    if (rewardItem.rewardTypeID === rewardType.rewardTypeID) {
                        rewardItem.rewardTypeDescription = rewardType.description;
                    }
                });
            }
            else {
                getRewardTypes();
            }

            if (vm.rewardStatuses) {
                vm.rewardStatuses.forEach(function (statusCode) {
                    if (rewardItem.rewardStatusID === statusCode.rewardStatusID) {
                        rewardItem.statusDescription = statusCode.description;
                    }
                });
            }
            else {
                getRewardStatuses();
            }
        }

        function setUpPaging() {
            vm.filteredRewards = vm.rewards;

            vm.currentPage = 1;
            vm.pageSize = 10;

            vm.filteredItems = vm.rewards.length; //Initially for no filter
            vm.totalItems = vm.rewards.length;

            vm.setPage = function (pageNo) {
                vm.currentPage = pageNo;
            };

            vm.filter = function () {
                $timeout(function () {
                    vm.filteredItems = $rootScope.filtered.length;
                }, 10);
            };
        }

        vm.openDetail = function (reward) {
            var modalScope = $rootScope.$new();
            modalScope.reward = reward;
            $rootScope.detailModalInstance = $modal.open({
                templateUrl: 'detailModal.html',
                controller: detailModalController(modalScope, rewardService),
                backdrop: 'static',
                keyboard: false,
                scope: modalScope
            });
        };

        vm.openPrint = function (reward) {
            var modalScope = $rootScope.$new();
            modalScope.reward = reward;
            $rootScope.printModalInstance = $modal.open({
                templateUrl: 'printModal.html',
                controller: printController(modalScope, rewardService),
                backdrop: 'static',
                keyboard: false,
                scope: modalScope
            });
        };

        vm.changeSort = function (value) {
            if (vm.sort == value) {
                vm.reverse = !vm.reverse;
            }
            else {
                vm.sort = value;
                vm.reverse = false;
            }
        }

        vm.showPrint = function (status, recipient) {
            return ((status === "Approved") && (vm.currentUser.toUpperCase() !== recipient.toUpperCase()));
        };
    }

})();
