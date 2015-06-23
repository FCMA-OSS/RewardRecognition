(function () {
    'use strict';

    angular
        .module('app')
        .controller('sidebar', sidebar);

    sidebar.$inject = ['$location']; 

    function sidebar($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'sidebar';

        activate();

        function activate() { }
    }
})();
