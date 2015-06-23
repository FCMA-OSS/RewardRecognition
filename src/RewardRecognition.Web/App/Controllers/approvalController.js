(function () {
    'use strict';

    angular
        .module('app')
        .controller('approvalController', approvalController);

    approvalController.$inject = ['$scope', '$rootScope', '$modal', 'rewardService'];

    function approvalController($scope, $rootScope, $modal, rewardService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'approvalController';

        activate();

        

        function activate() {
            getRewardTypes();
            getRewardReasons();
            getCurrentUser();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {
                        vm.isLeader = data.data.isLeader;
                        vm.currentUser = data.data.userName;
                        vm.presenterFullName = data.data.userFullName;
                        vm.presenterSupervisor = data.data.manager.userName;
                        //wait for promise
                        getPendingRewards();
                        $rootScope.loading--;
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

        function getRewardReasons() {
            $rootScope.loading++;
            rewardService.getRewardReasons()
               .then(function (data) {
                   vm.rewardReasons = [];
                   data.data.forEach(function (item) {
                       vm.rewardReasons.push(item);
                   });
                   $rootScope.loading--;
               }, function (error) {
                   var x = error;
                   $rootScope.loading--;
                });
        }

        function setUpPaging() {
            vm.filteredRequests = vm.rewards;

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

        function getPendingRewards() {
            $rootScope.loading++;
            rewardService.getPendingRewards(vm.currentUser) //vm.presenter//"ERISING"
                .then(function (data) {
                    if (data) {
                        vm.rewards = [];
                        data.forEach(function (item) {
                            mapFields(item);
                            vm.rewards.push(item);
                        });
                        setUpPaging();
                        //vm.gridOptions.data = vm.rewards;
                        $rootScope.loading--;
                    }
                }, function (error) {
                    var ex = error;
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

            if (vm.rewardReasons) {
                vm.rewardReasons.forEach(function (reasonCode) {
                    if (rewardItem.rewardReasonID === reasonCode.rewardReasonID) {
                        rewardItem.reasonDescription = reasonCode.description;
                    }
                });
        }
            else {
                getRewardReasons();
            }
        }

        vm.changeSort = function (value) {
            if (vm.sort == value) {
                vm.reverse = !vm.reverse;
            }
            else {
                vm.sort = value;
                vm.reverse = false;
            }
        }

        $rootScope.alerts = [];

        vm.clickApprove = function (id) {
            ConfirmAction(id, "A");
            $scope.alerts = $rootScope.alerts;
        };

        vm.clickDenied = function (id) {
            ConfirmAction(id, "D");
            $scope.alerts = $rootScope.alerts;
        };


        function ConfirmAction(rewardId, statusCode) {
            var confirmModal = $modal.open({
                templateUrl: 'confirmModalContent.html',
                controller: 'confirmModalCtrl',
                resolve: {
                    rewardInfo: function () {
                        return { rewardId: rewardId, action: statusCode, getPendingRewards: function () { getPendingRewards(); } }
                    }
                },
                backdrop: 'static',
                keyboard: false
            });
        }

        vm.closeAlert = function (index) {
            $rootScope.alerts.splice(index, 1);
            $scope.alerts = $rootScope.alerts;
        };


    }

    angular
       .module('app')
       .controller('detailModalCtrl', detailModalCtrl);

    detailModalCtrl.$inject = ['$scope', '$rootScope', '$timeout', '$modalInstance', '$location', 'rewardInfo', 'rewardService'];

    function detailModalCtrl($scope, $rootScope, $timeout, $modalInstance, $location, rewardInfo, rewardService) {
        $scope.title = 'Reward Details';

        $scope.reward = [];
        rewardService.getRewardById(rewardInfo.rewardId)
               .then(function (data) {
                   $scope.reward = data;
               }, function (error) {
                   var x = error;
               });

        $scope.ok = function () {
            $modalInstance.close();
        };
    }


    angular
       .module('app')
       .controller('confirmModalCtrl', confirmModalCtrl);

    confirmModalCtrl.$inject = ['$scope', '$rootScope', '$timeout', '$modalInstance', '$location', 'rewardInfo', 'rewardService'];

    function confirmModalCtrl($scope, $rootScope, $timeout, $modalInstance, $location, rewardInfo, rewardService) {
        if (rewardInfo.action === "A") {
            $scope.title = 'Are you sure you want to APPROVE this reward?';
            $scope.buttonText = 'Approve';
        }
        if (rewardInfo.action === "D") {
            $scope.title = 'Are you sure you want to DENY this reward?';
            $scope.buttonText = 'Deny';
        }

        $scope.reward = [];
        rewardService.getRewardById(rewardInfo.rewardId)
               .then(function (data) {
                   $scope.reward = data;
               }, function (error) {
                   var x = error;
               });

        $scope.ok = function () {
            rewardService.UpdateRewardStatus(rewardInfo.rewardId, rewardInfo.action)
                .then(function (data) {
                    rewardInfo.getPendingRewards();
                    if (rewardInfo.action === "A") {
                        $rootScope.alerts.push({ msg: 'Approval complete.', type: 'success' });
                    }
                    if (rewardInfo.action === "D") {
                        $rootScope.alerts.push({ msg: 'Denial complete.', type: 'success' });
                    }

                    $timeout(function () {
                        $rootScope.alerts.splice($rootScope.alerts.indexOf(alert), 1);
                    }, 10000);

                }, function (error) {
                    var x = error;
                    $rootScope.alerts.push({ msg: 'Decision FAILED!', type: 'danger' });
                    $timeout(function () {
                        $rootScope.alerts.splice($rootScope.alerts.indexOf(alert), 1);
                    }, 10000);
                });
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.close();
        };
    }

})();
