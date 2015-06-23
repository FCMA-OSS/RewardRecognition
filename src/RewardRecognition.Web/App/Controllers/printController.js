function printController($rootScope, rewardService) {
    $rootScope.title = 'Print Reward Voucher';

    $rootScope.print = function () {
        window.print();
        close();
    };

    $rootScope.cancel = function () {
        close();
    };

    function close() {
        $rootScope.id = undefined;
        $rootScope.printModalInstance.close();
    }
};