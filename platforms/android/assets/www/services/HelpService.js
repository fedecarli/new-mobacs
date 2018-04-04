angular.module('mobacs').factory('HelpService',['$q','SQLiteService',function($q, SQLiteService){
  return{

    selectEtnias : function(search){
      var query = 'SELECT * FROM Etnia where descricao like "'+search+'%"';
      return $q.when(SQLiteService.getItems(query));
    },
    selectMunicipos : function(search){
      var query = 'SELECT * FROM Municipios where like "'+search+'%"';
      console.log(query);
      return $q.when(SQLiteService.getItems(query));
    },
    selectPaises : function(search){
      var query = 'SELECT * FROM Paises where nome like "'+search+'%"';
      return $q.when(SQLiteService.getItems(query));
    },
    selectOcupacao : function(search){
      var query = 'SELECT * FROM OcupacaoCodigoCbo2002 where nome like "%'+search+'%"';
      return $q.when(SQLiteService.getItems(query));
    }
  }
}]);
