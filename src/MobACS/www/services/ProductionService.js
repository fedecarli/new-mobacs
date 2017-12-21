angular.module( 'mobacs')
.factory('ProductionService', ['$q', 'SQLiteService', function ($q, SQLiteService) {
  return {
    selectStatusProduction : function(){
      var query = 'SELECT dado from ( '
        + 'SELECT count(*) as dado FROM Cadastro_Individual WHERE status = "Aguardando Sincronismo" union all'
        + ' SELECT count(*) as dado FROM cadastroDomiciliarTerritorial WHERE status = "Aguardando Sincronismo" union all'
        + ' SELECT count(*) as dado FROM Visita_Domiciliar WHERE status = "Aguardando Sincronismo"'
        + ') as Numbers';
      return $q.when(SQLiteService.getItems(query));
    }
  };
}]);
