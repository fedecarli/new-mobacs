angular.module('mobacs').factory('FamilyService',['$q','SQLiteService',function($q, SQLiteService){
  return{

    createTableFamilies : function(){
      var query = 'CREATE TABLE IF NOT EXISTS FamiliaRow ('
        + 'id integer primary key autoincrement not null,'
        + 'cadastroDomiciliarId integer null,'
        + 'dataNascimentoResponsavel text null,'
        + 'numeroCnsResponsavel text null,'
        + 'numeroMembrosFamilia text null,'
        + 'numeroProntuario text null,'
        + 'rendaFamiliar text null,'
        + 'resideDesde text null,'
        + 'stMudanca integer null'
        + ')';
      return $q.when( SQLiteService.executeSql( query ) );
    },

    dropTableFamilies : function (){
      var query = 'DROP TABLE IF EXISTS FamiliaRow';
      return $q.when(SQLiteService.executeSql(query));
    },

    getFamilies : function(condition){
      var query = 'SELECT * FROM FamiliaRow WHERE '+condition+'';
      return $q.when(SQLiteService.getItems(query));
    },

    getAllFamilies : function(){
      var query = 'SELECT * FROM FamiliaRow';
      return $q.when(SQLiteService.getItems(query));
    },

    getAllFamiliesWithDetails : function(){
      var query = 'SELECT FamiliaRow.cadastroDomiciliarId as cadastroDomiciliarId, Cadastro_Individual.nomeCidadao as nomeCidadao FROM FamiliaRow inner join Cadastro_Individual on FamiliaRow.numeroCnsResponsavel = Cadastro_Individual.cnsCidadao';
      return $q.when(SQLiteService.getItems(query));
    },

    insertFamily : function(dataInformation, idHomeRegistration){

      var query = 'INSERT INTO FamiliaRow ('
        + 'cadastroDomiciliarId,'
        + 'dataNascimentoResponsavel,'
        + 'numeroCnsResponsavel,'
        + 'numeroMembrosFamilia,'
        + 'numeroProntuario,'
        + 'rendaFamiliar,'
        + 'resideDesde,'
        + 'stMudanca'
        + ') VALUES ('
        + '?,'
        + '?,'
        + '?,'
        + '?,'
        + '?,'
        + '?,'
        + '?,'
        + '?)';
      return $q.when( SQLiteService.executeSql( query,[
        idHomeRegistration,
        dataInformation.dataNascimentoResponsavel,
        dataInformation.numeroCnsResponsavel,
        dataInformation.numeroMembrosFamilia,
        dataInformation.numeroProntuario,
        dataInformation.rendaFamiliar,
        dataInformation.resideDesde,
        dataInformation.stMudanca
      ]));
    },

    updateFamily : function(dataInformation, condition){

      var query = 'UPDATE FamiliaRow SET ';
      if(dataInformation.numeroProntuario != null){
        query+= 'numeroProntuario = "'+dataInformation.numeroProntuario+'",';
      }else{
        query+= 'numeroProntuario = null,';
      }
      if(dataInformation.numeroCnsResponsavel!=null){
        query+='numeroCnsResponsavel = "'+dataInformation.numeroCnsResponsavel+'",';
      }else{
        query+='numeroCnsResponsavel = null,';
      }
      if(dataInformation.dataNascimentoResponsavel!= null){
        query+= 'dataNascimentoResponsavel = "'+dataInformation.dataNascimentoResponsavel+'",';
      }else{
        query+= 'dataNascimentoResponsavel = null,';
      }
      query += 'rendaFamiliar = '+dataInformation.rendaFamiliar+',';
      if(dataInformation.numeroMembrosFamilia!=null){
        query+= 'numeroMembrosFamilia = "'+dataInformation.numeroMembrosFamilia+'",';
      }else{
        query+= 'numeroMembrosFamilia = null,';
      }
      if(dataInformation.resideDesde!=null){
        query+='resideDesde = '+dataInformation.resideDesde+',';
      }else{
        query+='resideDesde = null,';
      }
      query +='stMudanca = '+dataInformation.stMudanca+''
        + ' WHERE '+condition+'';
      return $q.when(SQLiteService.getItems(query));
    },

    deleteFamilies : function(condition){
      var query = 'DELETE from FamiliaRow WHERE '
        + condition;
      return $q.when( SQLiteService.executeSql( query ) );
    },

    updateCNS : function(previousCns, nextCns){
      var query = 'UPDATE FamiliaRow set numeroCnsResponsavel = "'+nextCns+'" where numeroCnsResponsavel = "'+previousCns+'"';
      console.log(query);
      return $q.when( SQLiteService.executeSql( query ) );
    },

    updateGenericFamiliaRow : function(data, condition){
      var query = 'UPDATE FamiliaRow set '+data+' WHERE '+condition+' ';
      return $q.when( SQLiteService.executeSql( query ) );
    }
  }
}])
