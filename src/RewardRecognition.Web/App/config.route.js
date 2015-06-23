/// <reference path="create.html" />
(function () {
	'use strict';

	var app = angular.module('app');

	app.config(['$logProvider', '$locationProvider', function ($logProvider, $locationProvider) {
		// turn debugging off/on (no info or warn)
		if ($logProvider.debugEnabled) {
			$logProvider.debugEnabled(true);
		}

		$locationProvider.html5Mode(false);//.hashPrefix('!');

	}]);

	// Collect the routes
	app.constant('routes', getRoutes());

	// Configure the routes and route resolvers
	app.config(['$routeProvider', 'routes', routeConfigurator]);
	function routeConfigurator($routeProvider, routes) {

		routes.forEach(function (r) {
			$routeProvider.when(r.url, r.config);
		});
		$routeProvider.otherwise({ redirectTo: '/create/' });
	}

	// Define the routes 
	function getRoutes() {
		return [
				{
					url: '/create/',
					config: {
						templateUrl: 'App/Views/create.html'
					}
				},
				{
					url: '/approval/',
					config: {
						templateUrl: 'App/Views/approval.html'
					}
				},
				{
				    url: '/redeem/',
				    config: {
				        templateUrl: 'App/Views/redeem.html'
				    }
				},
				{
				    url: '/print/:id',
				    config: {
				        templateUrl: 'App/Views/print.html'
				    }
				},
				{
				    url: '/myRewards/',
				    config: {
				        templateUrl: 'App/Views/myRewards.html'
				    }
				},
                {
                    url: '/myTeamRewards/',
                    config: {
                        templateUrl: 'App/Views/myTeamRewards.html'
                    }
                },
				{
				    url: '/report/',
				    config: {
				        templateUrl: 'App/Views/report.html'
				    }
				}

		];
	}
})();