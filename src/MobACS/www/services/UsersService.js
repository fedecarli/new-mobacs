angular.module( 'mobacs')
  .factory('UsersService', ['$q', 'SQLiteService', function ($q, SQLiteService) {
    return {
      getAllUsers: function () {
        var query = 'SELECT * FROM Users';
        return $q.when(SQLiteService.getItems(query));
      },
      getUser: function ( condition ) {
        var query = 'SELECT * FROM Users WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },
      addUser: function ( user ) {
        var query = "INSERT INTO Users ( nome, email, userLogin, profissionalCNS, cboCodigo_2002, cnes, ine, codigoIbgeMunicipio, senha, statusSincronizacao ) VALUES ( ?,?,?,?,?,?,?,?,?,? )";
        return $q.when( SQLiteService.executeSql( query, [ user.nome, user.email, user.userLogin, user.profissionalCNS, user.cboCodigo_2002, user.cnes.toString(), user.ine, user.codigoIbgeMunicipio, user.senha, user.statusSincronizacao ] ) );
      },
      dropTable: function () {
        var query = 'DROP TABLE IF EXISTS Users';
        return $q.when(SQLiteService.executeSql(query));
      },
      createTable: function () {
        var query = 'CREATE TABLE IF NOT EXISTS Users ( id integer primary key autoincrement not null, nome varchar(70) not null, email varchar(255) not null, userLogin varchar(255) not null,profissionalCNS varchar(15) not null, cboCodigo_2002 varchar(6) not null, cnes varchar(7) not null, ine varchar(10) null, codigoIbgeMunicipio varchar(7) not null, senha varchar(30) not null, statusSincronizacao integer null )';
        return $q.when( SQLiteService.executeSql( query ) );
      }
    };
  }]);
