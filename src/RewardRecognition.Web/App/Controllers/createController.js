(function () {
    'use strict';

    angular
        .module('app')
        .controller('createController', createController);

    createController.$inject = ['$scope', '$timeout', '$interval', '$modal', '$q', '$location', '$http', '$rootScope', 'rewardService'];

    function createController($scope, $timeout, $interval, $modal, $q, $location, $http, $rootScope, rewardService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Recognize a Teammate!';
        vm.presenter = '';
        vm.presenterSupervisor = '';
        vm.recipient = '';
        vm.amount = '';
        vm.reason = '';
        vm.Comments = '';
        vm.rewardTypes = [];
        vm.rewardReasons = [];
        vm.otherAmount = undefined;
        vm.selected = undefined;
        vm.users = [];
        vm.selectedUsers = [];
        vm.isLeader = false;
        vm.showApprovalNotice = false;

        vm.showApprovalNoticeCalc = function () {
            vm.showApprovalNotice = false;
            if (vm.rewardType) {
                vm.showApprovalNotice = (vm.rewardType.needApproval && !vm.isLeader);
            }

            return vm.showApprovalNotice;
        }

        vm.onSelect = function ($item, $model, $label) {
            if (vm.selectedUsers.indexOf($item) < 0) {
                if ($item.userName !== vm.presenter && $item.userName !== 'bjohns4') {
                    vm.selectedUsers.push($item);
                    var index = vm.users.indexOf($item);
                    vm.users.splice(index, 1);
                }
            }
            vm.selected = '';
        };

        vm.removeSelectedUser = function (user) {
            // this removes the user from the selected users
            // and adds it back to the AD list
            if (vm.users.indexOf(user) < 0) {
                vm.users.push(user);
                var index = vm.selectedUsers.indexOf(user);
                vm.selectedUsers.splice(index, 1);
            }
        }

        vm.inAD = function (evt) {
            if (evt.keyCode == 13 && vm.selected && vm.users.indexOf(vm.selected) < 0) {
                vm.selected = '';
            }
        }

        vm.invalid = function (forceInvalid) {
            if (forceInvalid) {
                return forceInvalid;
            }
            var valid = (vm.selectedUsers.length > 0 &&
                         vm.rewardReason != null &&
                         vm.rewardType != null &&
                         vm.Comments.length > 0);

            return !valid;
        }

        activate();

        function activate() {
            getUsers();
            getRewardTypes();
            getRewardReasons();
            getCurrentUser();
            getRecentRewards();
        }

        function getCurrentUser() {
            $rootScope.loading++;
            rewardService.getCurrentUser()
                .then(function (data) {
                    if (data) {
                        vm.selected = '';
                        vm.presenter = data.data.userName;
                        vm.presenterFullName = data.data.userFullName;
                        vm.presenterSupervisor = data.data.manager.userName;
                        vm.isInRedeemGroup = data.data.isInRedeemGroup;
                        vm.isLeader = data.data.isLeader;
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

        function getRecentRewards() {
            $rootScope.loading++;
            rewardService.getRecentRewards()
            .then(function (data) {
                vm.recentRewards = [];
                vm.currentRewards = [];
                var promises = [];
                if (data) {
                    data.forEach(function (item) {
                        $rootScope.loading++;
                        var promise = rewardService.getUserImage(item.recipient, 'l');
                        promise.then(function (url) {
                            item.image = url;
                            vm.recentRewards.push(item);
                            $rootScope.loading--;
                        });
                        promises.push(promise);
                    });
                }
                $rootScope.loading--;
                $q.all(promises).then(setUpLiveTiles);
            });
        }

        function setUpLiveTiles() {
            // make sure we have enough rewards to display
            // fill in with placeholder biostar logos.
            var placeholder = { image: 'Content/biostar-large.jpg' }
            while (vm.recentRewards.length < 23) {
                vm.recentRewards.push(placeholder);
            }

            vm.recentRewards.splice(0, 0, placeholder);
            vm.recentRewards.splice(18, 0, placeholder);

            for (var i = 0; i < 20; i++) {
                var reward = vm.recentRewards.splice(0, 1)[0];
                vm.currentRewards.push(reward);
            }

            $scope.items = ['front', 'back'];
            $scope.num = Math.floor(((Math.random() * 100) + 1) % 2);
            $scope.flip = false;
            $scope.flip = [false, false, false, false, false, false, false, false, false, false];

            $scope.flipTile = function () {
                var previousIndex = vm.index;
                while (previousIndex == vm.index) {
                    vm.index = Math.floor(Math.random() * ($scope.flip.length));
                }
                var rewardIndex = 2 * vm.index;
                if (!$scope.flip[vm.index]) { //if flipped then change front image, else change back image
                    rewardIndex++;
                }

                var thisReward = vm.currentRewards[rewardIndex];
                vm.currentRewards[rewardIndex] = vm.recentRewards.splice(0, 1)[0];
                vm.recentRewards.push(thisReward);

                $timeout(function () {
                    $scope.flip[vm.index] = !$scope.flip[vm.index];
                }, 1000);
            }
            $interval($scope.flipTile, 3000);
        }

        vm.openDetail = function (reward) {
            if (reward.rewardID) {
                var modalScope = $rootScope.$new();
                modalScope.reward = reward;
                $rootScope.detailModalInstance = $modal.open({
                    templateUrl: 'detailModal.html',
                    controller: detailModalController(modalScope, rewardService),
                    backdrop: 'static',
                    keyboard: false,
                    scope: modalScope
                });
            }
        };

        function getUsers() {
            $rootScope.loading++;
            rewardService.getUsers()
                .then(function (data) {
                    if (data) {
                        vm.users = data.data;
                        vm.data = data;
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

        function generateRewards() {
            var rewards = [];
            var rewardStatus = {
                RewardStatusID: 2, Code: 'A', Description: 'Approved', IsActive: 1
            };
            if ((vm.rewardType.needApproval || vm.isInRedeemGroup) && !vm.isLeader) {
                rewardStatus.RewardStatusID = 1;
                rewardStatus.Code = 'P';
                rewardStatus.Description = 'Pending Approval';
            }
            vm.selectedUsers.forEach(function (user) {
                var supervisor = vm.presenterSupervisor.toUpperCase();
                if ((user.userName.toUpperCase() == supervisor) && (user.manager.userName)) supervisor = user.manager.userName.toUpperCase();
                rewards.push(
                {
                    RewardTypeID: vm.rewardType.rewardTypeID,
                    RewardReasonID: vm.rewardReason.rewardReasonID,
                    OtherReason: vm.Comments,
                    Recipient: user.userName.toUpperCase(),
                    RecipientFullName: user.userFullName,
                    Supervisor: supervisor,
                    RewardStatusID: rewardStatus.RewardStatusID,
                    CreatedDate: new Date,
                    CreatedBy: vm.presenter.toUpperCase(),
                    CreatedByFullName: vm.presenterFullName,
                    LastChangedDate: new Date,
                    ChangedBy: vm.presenter.toUpperCase(),
                    RedeemedDate: undefined,
                    RedeemedBy: undefined,
                    PresentationDate: new Date,
                    RewardReason: undefined, //vm.rewardReason,
                    RewardStatus: undefined, //rewardStatus,
                    RewardType: undefined //rewardType
                }
                );
            });
            return rewards;
        }

        vm.openReview = function () {
            vm.invalid(true);
            var rewards = generateRewards();
            var previewModal = $modal.open({
                templateUrl: 'previewModalContent.html',
                controller: 'previewModalController',
                resolve: {
                    rewardInfo: function () {
                        return {
                            selectedUsers: vm.selectedUsers, rewardType: vm.rewardType, reason: vm.rewardReason, rewards: rewards
                        }
                    }
                },
                backdrop: 'static',
                keyboard: false
            });
        };

        vm.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

    }

    angular
        .module('app')
        .controller('previewModalController', previewModalController);

    previewModalController.$inject = ['$scope', '$rootScope', '$timeout', '$modalInstance', '$location', 'rewardInfo', 'rewardService'];

    function previewModalController($scope, $rootScope, $timeout, $modalInstance, $location, rewardInfo, rewardService) {
        $scope.title = 'Review and Confirm:';
        $scope.selectedUsers = rewardInfo.selectedUsers;
        $scope.amount = rewardInfo.rewardType;
        $scope.reason = rewardInfo.reason;
        $scope.showButton = true;

        $scope.ok = function () {
            $scope.showButton = false;

            //want to go to rootScope here since the modal scope is a child of our main scope.
            rewardService.InsertRewards(rewardInfo.rewards)
                .then(
                function (data) {

                    //set a timeout on the alert to remove after a set amount of time
                    //$timeout(function () {
                    //$rootScope.alerts.splice($rootScope.alerts.indexOf(alert), 1);
                    //}, 15000);
                    $modalInstance.close();
                    $location.path('/myRewards/');
                }
            );
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }

})();
