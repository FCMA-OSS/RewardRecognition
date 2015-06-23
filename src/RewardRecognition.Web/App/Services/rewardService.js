(function () {
    'use strict';

    angular
        .module('app')
        .factory('rewardService', rewardService);

    rewardService.$inject = ['$http', '$q', '$filter'];

    function rewardService($http, $q, $filter) {
        var salt = 'CakeInAJar';
        var hashids = new Hashids(salt);
        var service = {
            getUsers: getUsers,
            getRewardTypes: getRewardTypes,
            getRewardReasons: getRewardReasons,
            getPendingRewards: getPendingRewards,
            InsertRewards: InsertRewards,
            getRewardById: getRewardById,
            getApprovedRewardById: getApprovedRewardById,
            UpdateRewardStatus: UpdateRewardStatus,
            getMyRewards: getMyRewards,
            getRecentRewards: getRecentRewards,
            getMyTeamRewards: getMyTeamRewards,
            getCurrentUser: getCurrentUser,
            getRewardStatuses: getRewardStatuses,
            getUserImage: getUserImage,
            encode: encode,
            decode: decode
        };

        return service;

        function getRewardTypes() {
            return $http.get("api/Reward/RewardTypes");
        }

        function getRewardReasons() {
            return $http.get("api/Reward/RewardReasons");
        }

        function getRewardStatuses() {
            return $http.get("api/Reward/RewardStatuses");
        }


        function getUsers() {
            return $http.get("api/Reward/AllUsersList");
        }

        function getPendingRewards(supervisor) {
            var deferred = $q.defer();
            $http.get("api/Reward/GetPendingRewards", { params: { supervisor: supervisor } }).success(function (data) {
                if (data) {
                    data.forEach(function (item) {
                        item.rewardID = encode(item.rewardID, item.createdDate);
                    });
                    deferred.resolve(data);
                } else {
                    deferred.reject(data);
                }
            })
            .error(function (data) {
                deferred.reject(data);
            });
            return deferred.promise;
        }

        function InsertRewards(rewards) {
            return $http.post("api/Reward/InsertReward", rewards);
        }

        function UpdateRewardStatus(rewardID, statusCode) {
            return $http.put("api/Reward/UpdateRewardStatusAsync/?id=" + decode(rewardID) + "&statusCode=" + statusCode);
        }

        function getRewardById(id) {
            var deferred = $q.defer();
            if (id) {
                $http.get("api/Reward/GetRewardById/?id=" + decode(id)).success(function (data) {
                    if (data) {
                        data.rewardID = encode(data.rewardID, data.createdDate);
                        deferred.resolve(data);
                    } else {
                        deferred.reject(data)
                    }
                })
                .error(function (data) {
                    deferred.reject(data);
                });
            } else {
                deferred.reject(id);
            }
            return deferred.promise;
        }

        function getApprovedRewardById(id) {
            var deferred = $q.defer();
            if (id) {
                $http.get("api/Reward/GetApprovedRewardById/?id=" + decode(id)).success(function (data) {
                    if (data) {
                        data.rewardID = encode(data.rewardID, data.createdDate);
                        deferred.resolve(data);
                    } else {
                        deferred.reject(data);
                    }
                })
                .error(function (data) {
                    deferred.reject(data);
                });
            } else {
                deferred.reject(id);
            }
            return deferred.promise;
        }

        function getMyRewards() {
            var deferred = $q.defer();
            $http.get("api/Reward/GetMyRewards").success(function (data) {
                if (data) {
                    data.forEach(function (item) {
                        item.rewardID = encode(item.rewardID, item.createdDate);
                    });
                    deferred.resolve(data);
                } else {
                    deferred.reject(data);
                }
            })
            .error(function (data) {
                deferred.reject(data);
            });
            return deferred.promise;
        }

        function getRecentRewards() {
            var deferred = $q.defer();
            $http.get("api/Reward/getRecentRewards").success(function (data) {
                if (data) {
                    data.forEach(function (item) {
                        item.rewardID = encode(item.rewardID, item.createdDate);
                    });
                    deferred.resolve(data);
                } else {
                    deferred.reject(data);
                }
            })
            .error(function (data) {
                deferred.reject(data);
            });
            return deferred.promise;
        }

        function getMyTeamRewards() {
            var deferred = $q.defer();
            $http.get("api/Reward/GetMyTeamRewards").success(function (data) {
                if (data) {
                    data.forEach(function (item) {
                        item.rewardID = encode(item.rewardID, item.createdDate);
                    });
                    deferred.resolve(data);
                } else {
                    deferred.reject(data);
                }
            })
            .error(function (data) {
                deferred.reject(data);
            });
            return deferred.promise;
        }

        function getCurrentUser() {
            return $http.get("api/Reward/GetCurrentUser")
        }

        function getUserImage(userName, size) {
            var deferred = $q.defer();
            var url = 'http://' + userName + '_' + size + 'Thumb.jpg';
            var defaultTemplate = 'http://96.png';

            $http.get(url).success(function () {
                deferred.resolve(url);
            }).error(function () {
                deferred.resolve(defaultTemplate);
            });

            return deferred.promise;
        }

        function encode(id, date) {
            var d = $filter('date')(date, "yyyyMMdd");
            return hashids.encode(parseInt(d + id, 10));
        }

        function decode(code) {
            var decoded = hashids.decode(code) + "";
            return decoded.substring(8, decoded.length);
        }
    }
})();