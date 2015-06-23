function detailModalController($rootScope, rewardService) {
    $rootScope.title = 'Reward Details';

    var promise = rewardService.getUserImage($rootScope.reward.recipient, 'm');
    promise.then(function (url) {
        $rootScope.imageURL = url;
    });

    $rootScope.ok = function () {
        $rootScope.startFlippin;
        $rootScope.detailModalInstance.close();
    };
};