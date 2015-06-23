(function () {
    'use strict';

    angular
        .module('app')
        .controller('myTeamRewardsController', myTeamRewardsController)
        .filter('sumOfValue', function () {
            return function (data) {
                if (typeof data != "undefined") {
                    var totalAmount = 0;
                    data.forEach(function (item) {

                        if (!isNaN(item.rewardType.amount)) totalAmount += parseInt(item.rewardType.amount);
                    })
                    return totalAmount;
                }
        }
        })
        .filter('myDateRangeFilter', function () {
            return function (items, from, to) {
                var result = items;
                if (from && moment(from).isValid() && (!to || !moment(to).isValid)) {
                    result = [];
                    for (var i = 0; i < items.length; i++) {
                        if (!(moment(items[i].createdDate).isBefore(from))) {
                            result.push(items[i]);
                        }
                    }
                } else if (to && moment(to).isValid() && (!from || !moment(from).isValid)) {
                    result = [];
                    for (var i = 0; i < items.length; i++) {
                        if (!(moment(items[i].createdDate).isAfter(to))) {
                            result.push(items[i]);
                        }
                    }
                } else if (from && to && moment(from).isValid() && moment(to).isValid()) {                   
                    result = [];
                    for (var i = 0; i < items.length; i++) {
                        if (!(moment(items[i].createdDate).isBefore(from) || moment(items[i].createdDate).isAfter(to))){
                            result.push(items[i]);
                        }
                    }
                }
                return result;
            };

        });

  
    myTeamRewardsController.$inject = ['$location', '$scope', '$rootScope', '$modal', 'rewardService', 'authService'];

    function myTeamRewardsController($location, $scope, $rootScope, $modal, rewardService, authService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'myTeamRewardsController';

        activate();
        
        vm.exportData = function () {
            var createdByFullName = $scope.filterOptions.createdByFullName;
            if (createdByFullName) createdByFullName = createdByFullName;
            var sqlCreatedByFullName = " createdByFullName like '%" + createdByFullName + "%' "

            var recipientFullName = $scope.filterOptions.recipientFullName;
            if (recipientFullName) recipientFullName = recipientFullName;
            var sqlRecipientFullName = " recipientFullName like '%" + recipientFullName + "%' "

            var statusDescription = $scope.filterOptions.statusDescription;
            if (statusDescription) statusDescription = statusDescription.toUpperCase();
            var sqlStatusDescription = " ucase(statusDescription) like '%" + statusDescription + "%' "

            var rewardTypeDescription = $scope.filterOptions.rewardTypeDescription;
            if (rewardTypeDescription) rewardTypeDescription = rewardTypeDescription.toUpperCase();
            var sqlRewardTypeDescription = " ucase(rewardTypeDescription) like '%" + rewardTypeDescription + "%' "

            var fromDate = $scope.filterCreatedDateFrom;
            var toDate = $scope.filterCreatedDateTo;
            var sqlFromDate="";
            var sqlToDate="";

            if (fromDate) sqlFromDate = " And createdDate >= '" + moment(fromDate).format("YYYY-MM-DD") + "' ";

            if (toDate) sqlToDate = " And createdDate <= '" + moment(toDate).format("YYYY-MM-DD") + "' ";
            
            var sql = 'SELECT createdByFullName as [From],recipientFullName as [To],rewardTypeDescription as Reward,createdDate as Created,statusDescription as Status INTO XLSX("MyTeamRewards.xlsx",{headers:true}) FROM ? Where ' + sqlCreatedByFullName + ' And ' + sqlRecipientFullName + ' And ' + sqlRewardTypeDescription + ' And ' + sqlStatusDescription + sqlFromDate + sqlToDate
          
            alasql(sql, [vm.rewards]);
        };

        function activate() {
            getRewardTypes();
            getRewardStatuses();
            getMyTeamRewards();
            getCurrentUser();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {
                        vm.currentUser = data.data.userName;
                        vm.isLeader = data.data.isLeader;
                    }
                    $rootScope.loading--;
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

        function getMyTeamRewards() {
            $rootScope.loading++;
            rewardService.getMyTeamRewards()
                .then(function (data) {
                    vm.totalAmount = 0;
                    vm.rewards = [];
                    data.forEach(function (item) {
                        //this needs to happen before the data reaches the user.
                        mapFields(item);
                        vm.rewards.push(item);
                        if (!isNaN(item.rewardType.amount)) vm.totalAmount += parseInt(item.rewardType.amount);
                    })
                    setUpPaging();
                    vm.rewards.reverse();
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

            if (rewardItem.createdDate) {
                rewardItem.createdDate = rewardItem.createdDate.substring(0, 10);
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
            return ((status === "Approved"));// && ($rootScope.user.userName !== recipient));
        };
        
        $scope.open = function ($event, opened) {
            $event.preventDefault();
            $event.stopPropagation();
            //var myEvent = $event;
            //debugger;
            $scope[opened] = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        //$scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];       
        $scope.format = 'yyyy-MM-dd';
    }

  

})();
