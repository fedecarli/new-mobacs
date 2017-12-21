angular.module('mobacs')
.config( function( $stateProvider, $urlRouterProvider ){
	$urlRouterProvider.otherwise( 'login/index' );

	$stateProvider
	.state( 'app', {
		url: '/app',
		templateUrl: 'views/general/side-menu.html',
		abstract: true
	} )
	.state( 'login/index', {
		url: '/login/index',
		templateUrl: 'views/login/index.html',
		controller: 'LoginController'
	} )
	.state( 'app.dashboard/index', {
		url: '/dashboard/index',
		views: {
			'menuContent': {
				templateUrl: 'views/dashboard/index.html',
				controller: 'DashboardController'
			}
		}
	} )
	.state( 'pessoas/index', {
		url: '/pessoas/index',
		templateUrl: 'views/pessoas/index.html',
		controller: 'ListPeopleController'
	})
  .state( 'pessoas/tabs/add-identification', {
    url: '/pessoas/tabs/add/add-identification',
    templateUrl: 'views/pessoas/tabs/add/add-identification.html',
    controller: 'PessoasController'
  })
  .state( 'pessoas/tabs/add-refused', {
      url: '/pessoas/tabs/add/add-refused',
      templateUrl: 'views/pessoas/tabs/add/add-refused.html',
      controller: 'PessoasController'
  })
  .state('view/people', {
    url: '/pessoas/tabs/view/:item',
    templateUrl: 'views/pessoas/tabs/view/viewpeople.html',
    controller: 'PessoasController'
  })
  .state( 'pessoas/tabs/edit-identification', { //Edit
    url: '/pessoas/tabs/add/edit-identification/:item',
    templateUrl: 'views/pessoas/tabs/add/add-identification.html',
    controller: 'PessoasController'
  } )
  .state( 'pessoas/tabs/add-information', {
    url: '/pessoas/tabs/add/add-information/:item',
    templateUrl: 'views/pessoas/tabs/add/add-information.html',
    controller: 'PessoasController'
  } )
  .state( 'pessoas/tabs/add-helth', {
    url: '/pessoas/tabs/add/add-helth/:item',
    templateUrl: 'views/pessoas/tabs/add/add-helth.html',
    controller: 'PessoasController'
  } )
  .state('pessoas/tabs/street',{
    url: '/pessoas/tabs/add/add-street/:item',
    templateUrl: 'views/pessoas/tabs/add/add-street.html',
    controller: 'PessoasController'
  })
  .state('pessoas/tabs/add-exit',{
    url: '/pessoas/tabs/add/add-exit/:item',
    templateUrl: 'views/pessoas/tabs/add/add-exit.html',
    controller: 'PessoasController'
  })
	.state( 'domicilios/index', {
		url: '/domicilios/index',
		templateUrl: 'views/domicilios/index.html',
		controller: 'ListAdressController'
	} )
  .state('view/adress', {
    url: '/domicilios/tabs/view/:id',
    templateUrl: 'views/domicilios/tabs/view/viewadress.html',
    controller: 'HomeRegistrationController'
  })
  .state( 'domicilios/tabs/add/add-adress', {
    url: '/domicilios/tabs/add/add-adress',
    templateUrl: 'views/domicilios/tabs/add/add-adress.html',
    controller: 'HomeRegistrationController'
  } )

  .state( 'domicilios/tabs/add/add-livingconditions', {
    url: '/domicilios/tabs/add/add-livingconditions/:id',
    templateUrl: 'views/domicilios/tabs/add/add-livingconditions.html',
    controller: 'HomeRegistrationController'
  } )

  .state( 'domicilios/tabs/add/edit-adress', { //Edit
    url: '/domicilios/tabs/add/edit-adress/:id',
    templateUrl: 'views/domicilios/tabs/add/add-adress.html',
    controller: 'HomeRegistrationController'
  } )

  .state( 'domicilios/tabs/add/add-animals', {
    url: '/domicilios/tabs/add/add-animals/:id',
    templateUrl: 'views/domicilios/tabs/add/add-animals.html',
    controller: 'HomeRegistrationController'
  } )
  .state( 'domicilios/tabs/add/add-families', {
    url: '/domicilios/tabs/add/add-families/:id',
    templateUrl: 'views/domicilios/tabs/add/add-families.html',
    controller: 'HomeRegistrationController'
  } )
  .state( 'domicilios/tabs/add/add-all-families', {
    url: '/domicilios/tabs/add/add-all-families',
    templateUrl: 'views/domicilios/tabs/add/add-all-families.html',
    controller: 'FamilyController'
  } )
  .state( 'domicilios/tabs/add/edit-all-families', {
    url: '/domicilios/tabs/add/edit-all-families/:id',
    templateUrl: 'views/domicilios/tabs/add/add-all-families.html',
    controller: 'FamilyController'
  } )
  .state( 'domicilios/tabs/add/add-institution', {
    url: '/domicilios/tabs/add/add-institution/:id',
    templateUrl: 'views/domicilios/tabs/add/add-institution.html',
    controller: 'HomeRegistrationController'
  } )
  .state( 'home-visit/index', {
    url: '/home-visit/index',
    templateUrl: 'views/home-visit/index.html',
    // controller: 'HomeVisitController'
    controller: 'ListHomeVisitController'
  } )
  .state( 'home-visit/add/informations', {
    url: '/home-visit/add/informations',
    templateUrl: 'views/home-visit/add/informations.html',
    controller: 'HomeVisitController'
  } )
  .state( 'home-visit/add/motive-visit', {
    url: '/home-visit/add/motive-visit/:item',
    templateUrl: 'views/home-visit/add/motive-visit.html',
    controller: 'HomeVisitController'
  } )
  .state( 'home-visit/add/anthropometry', {
    url: '/home-visit/add/anthropometry/:item',
    templateUrl: 'views/home-visit/add/anthropometry.html',
    controller: 'HomeVisitController'
  } )
  .state( 'home-visit/add/outcome', {
    url: '/home-visit/add/outcome/:item',
    templateUrl: 'views/home-visit/add/outcome.html',
    controller: 'HomeVisitController'
  } )
  .state('home-visit/edit/informations',{
    url: '/home-visit/edit/informations/:item',
    templateUrl: 'views/home-visit/add/informations.html',
    controller: 'HomeVisitController'
  })
  .state('view/home-visit', {
    url: '/home-visit/view/:item',
    templateUrl: 'views/home-visit/view/viewhomevisit.html',
    controller: 'HomeVisitController'
  })
  .state( 'synchronization', {
    url: '/synchronization',
    templateUrl: 'views/synchronization/synchronization.html',
    controller: 'SynchronizationController'
  })
  .state('production', {
    url: '/production',
    templateUrl: 'views/production/production.html',
    controller: 'ProductionController'
  })
  .state('usersbyhouseholds', {
    url : 'usersbyhouseholds/:cns',
    templateUrl: 'views/home-visit/list/usersbyhouseholds.html',
    controller: 'UsersByHouseholdsController'
  })

  //New Routes version 2.0


} );
