using System.Collections.Generic;

namespace DataModels
{
    public class DbObject<T> where T : new()
    {
        public string TableName { get; }

        public DbObject(string tableName) =>
            TableName = tableName;

        public List<T> ListAll()
        {
            
        }
    }
}
