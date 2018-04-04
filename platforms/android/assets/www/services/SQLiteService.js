angular.module('mobacs')
  .service('SQLiteService', ['$q', '$cordovaSQLite', function($q, $cordovaSQLite) {
    var self = this;
    var _db;

    self.db = function() {
      if (!_db) {
        if (window.sqlitePlugin !== undefined) {
          //www/demo.db is a file created with SqliteBrowser tool :)
          _db = window.sqlitePlugin.openDatabase({ name: "mobacs.db", location: 2, createFromLocation: 1 });
        } else {
          // For debugging in the browser
          _db = window.openDatabase("mobacs.db", "1.0", "MobACS", 200000);
        }
      }
      return _db;
    };

    self.getFirstItem = function(query, parameters) {
      var deferred = $q.defer();
      self.executeSql(query, parameters).then(function(res) {
        if (res.rows.length > 0)
          return deferred.resolve(res.rows.item(0));
        else
          return deferred.reject("There aren't items matching");
      }, function(err) {
        return deferred.reject(err);
      });

      return deferred.promise;
    };

    self.getFirstOrDefaultItem = function(query, parameters) {
      var deferred = $q.defer();
      self.executeSql(query, parameters).then(function(res) {
        if (res.rows.length > 0)
          return deferred.resolve(res.rows.item(0));
        else
          return deferred.resolve(null);
      }, function(err) {
        return deferred.reject(err);
      });

      return deferred.promise;
    };

    self.getItems = function(query, parameters) {
      var deferred = $q.defer();
      self.executeSql(query, parameters).then(function(res) {
        var items = [];
        for (var i = 0; i < res.rows.length; i++) {
          items.push(res.rows.item(i));
        }
        // console.log(items);
        return deferred.resolve(items);
      }, function(err) {
        return deferred.reject(err);
      });

      return deferred.promise;
    };

    /*self.preloadDataBase = function (enableLog) {
      var deferred = $q.defer();
      if ( window.sqlitePlugin ) {
        enableLog && console.log('%c ***************** Starting the creation of the database in the browser ***************** ', 'background: #222; color: #bada55');
        self.db().transaction(function (tx) {
          for (var i = 0; i < queries.length; i++) {
            enableLog && console.log(queries[i]);
            tx.executeSql(queries[i]);
          }
        }, function (error) {
          deferred.reject(error);
        }, function () {
          enableLog && console.log('%c ***************** Completing the creation of the database in the browser ***************** ', 'background: #222; color: #bada55');
          deferred.resolve("OK");
        });
      }
      else {
        deferred.resolve("OK");
      }

      return deferred.promise;
    };*/

    self.executeSql = function(query, parameters) {
      return $cordovaSQLite.execute(self.db(), query, parameters);
    };
  }]);
