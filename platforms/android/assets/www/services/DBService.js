angular.module('mobacs')
    .factory('DBService', function($q, DB_CONFIG, $cordovaSQLite) {
        db = null;

        init = function() {
            // Use self.db = window.sqlitePlugin.openDatabase({name: DB_CONFIG.name}); in production
            db = window.openDatabase(DB_CONFIG.name, '1.0', 'database', -1);

            angular.forEach(DB_CONFIG.tables, function(table) {
                var columns = [];

                angular.forEach(table.columns, function(column) {
                    columns.push(column.name + ' ' + column.type);
                });

                $cordovaSQLite.execute(db, 'CREATE TABLE IF NOT EXISTS ' + table.name + ' (' + columns.join(',') + ')');

                /*var query = 'CREATE TABLE IF NOT EXISTS ' + table.name + ' (' + columns.join(',') + ')';
                DBService.query(query);
                console.log('Table ' + table.name + ' initialized');*/
            });
        };

        //init();

        return {
            query: function(query, bindings) {
                bindings = typeof bindings !== 'undefined' ? bindings : [];
                var deferred = $q.defer();

                db.transaction(function(transaction) {
                    transaction.executeSql(query, bindings, function(transaction, result) {
                        deferred.resolve(result);
                    }, function(transaction, error) {
                        deferred.reject(error);
                    });
                });

                return deferred.promise;
            },

            fetchAll: function(result) {
                var output = [];

                for (var i = 0; i < result.rows.length; i++) {
                    output.push(result.rows.item(i));
                }

                return output;
            },

            fetch: function(result) {
                return result.rows.item(0);
            }

        };


    })

.factory('completaLogradouroService', function($q, $timeout) {

    var allTiposLogradouros = $rootScope.todosTiposLogradouros;

    var searchTiposDeLogradouros = function(searchFilter) {

        console.log('Searching tiposDeLogradouros for ' + searchFilter);

        var deferred = $q.defer();

        var matches = allTiposLogradouros.filter(function(tipo) {
            if (tipo.descricao.toLowerCase().indexOf(searchFilter.toLowerCase()) !== -1) return true;
        })

        $timeout(function() {

            deferred.resolve(matches);

        }, 100);

        return deferred.promise;

    };

    return {

        searchTiposDeLogradouros: searchTiposDeLogradouros

    }
})