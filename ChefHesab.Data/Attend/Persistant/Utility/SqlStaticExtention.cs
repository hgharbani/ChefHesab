using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Dynamic;
using System.Text;
using Ksc.HR.Share.Model;
using Microsoft.EntityFrameworkCore;
using DNTPersianUtils.Core;
using DNTPersianUtils.Core.Normalizer;

namespace Ksc.HR.Share.General
{
    public static class SqlStaticExtention
    {
        public static List<List<PivoteMonthTimesheet>> CollectionFromSql(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters)
        {


            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }
                var retObject = new List<List<PivoteMonthTimesheet>>();


                try
                {
                    cmd.CommandTimeout = 5000;
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = GetDataRow(dataReader);
                            retObject.Add(dataRow);
                        }
                        return retObject;
                    }
                }
                catch (Exception ex)
                {
                    return retObject;
                }


            }
        }
        private static List<PivoteMonthTimesheet> GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            var models = new List<PivoteMonthTimesheet>();
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
            {
                var model = new PivoteMonthTimesheet();
                model.FiledName = dataReader.GetName(fieldCount);
                model.Filedvalue = dataReader[fieldCount].ToString();
                models.Add(model);
                //dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);

            }

            return models;
        }

        public static List<IDictionary<string, object>> CollectionFromSqlReturnDictionary(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                var retObject = new List<IDictionary<string, object>>();

                using (var dataReader = cmd.ExecuteReader())
                {
                    

                    while (dataReader.Read())
                    {

                        retObject.Add(GetAddDataRow(dataReader));


                    }
                }

                return retObject;
            }
        }

        public static List<T> CollectionFromSqlReturnGeneric<T>(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters) where T : new()
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                var retObject = new List<T>();
                var cc = 0;
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var getData = GetAddDataRow(dataReader);
                    
                        var result = new T();
                        foreach (var p in typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                        {
                            if (getData.TryGetValue(p.Name, out object v))
                            {
                                if((v.GetType()).FullName!="System.DBNull")
                                p.SetValue(result, v);
                            }
                        }
                        retObject.Add(result);

                    }
                }

                return retObject;
            }
        }
        public static List<IDictionary<string, object>> CollectionFromSqlReturnListDictunary(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters) 
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                var retObject = new List<IDictionary<string, object>>();

                using (var dataReader = cmd.ExecuteReader())
                {
                    

                    while (dataReader.Read())
                    {
                        var getData = GetAddDataRow(dataReader);

                       
                        retObject.Add(getData);

                    }
                }

                return retObject;
            }
        }

        private static IDictionary<string, object> GetAddDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;


            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
            {
                dataRow.Add(dataReader.GetName(fieldCount).Trim().NormalizeYeHeHalfSpace().ApplyCorrectYeKe(), dataReader[fieldCount]);
            }

            return dataRow;
        }

        public static T TransFormtoModel<T>(this IDictionary<string, object> item) where T : new()
        {
            T result = new T();
            foreach (var p in typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (item.TryGetValue(p.Name, out object v))
                {
                    p.SetValue(result, v);
                }
            }
            return result;
        }

    }

}

