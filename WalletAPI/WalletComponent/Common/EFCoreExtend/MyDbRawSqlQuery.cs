using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace WalletComponent.Common.EFCoreExtend
{
    /// <summary>
    /// 扩展EFCore 的SQL查询接口
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public class MyDbRawSqlQuery<T> where T : new()
    {
        public MyDbRawSqlQuery(){}  
        public MyDbRawSqlQuery(DbConnection dbConn, string sql, params object[] parameters)
        {
            MyConnection = dbConn;
            SqlStr = sql;
            MyParameters = parameters;
        }
        public DbConnection MyConnection { get; set; }
        public string SqlStr { get; set; }
        public object[] MyParameters { get; set; }

        public List<T> ToList()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    var rtnList = new List<T>();
                    using (var reader = command.ExecuteReader())
                    {
                        rtnList = AutoFillEntity<T>(reader);
                    }
                    return rtnList;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }
        public async Task<List<T>> ToListAsync()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    var rtnList = new List<T>();

                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        rtnList = await AutoFillEntityAsync<T>(reader).ConfigureAwait(false);
                    }
                    return rtnList;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public T SingleOrDefault()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    return (T)command.ExecuteScalar();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }
        public async Task<T> SingleOrDefaultAsync()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    return (T)await command.ExecuteScalarAsync().ConfigureAwait(false);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public T FirstOrDefault()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    var rtnList = new List<T>();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            rtnList = AutoFillEntity<T>(reader);
                        }
                        else
                        {
                            rtnList.Add(new T());
                        }
                    }
                    return rtnList[0];
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }
        public async Task<T> FirstOrDefaultAsync()
        {
            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                using (var command = MyConnection.CreateCommand())
                {
                    command.CommandText = SqlStr;

                    if (MyParameters != null && MyParameters.Length > 0)
                        command.Parameters.AddRange(MyParameters);

                    var rtnList = new List<T>();
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        rtnList = await AutoFillEntityAsync<T>(reader).ConfigureAwait(false);
                    }

                    if (rtnList.Count == 0)
                        rtnList.Add(new T());

                    return rtnList[0];
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public static ConcurrentDictionary<string, ConcurrentDictionary<string,PropertyInfo>> MyModels = new ConcurrentDictionary<string, ConcurrentDictionary<string, PropertyInfo>>();

        private async Task<List<TResult>> AutoFillEntityAsync<TResult>(DbDataReader reader) where TResult : new()
        {
            var modelName = typeof(TResult).Name;
            if (!MyModels.ContainsKey(modelName))
            {
                RefelectionModels<TResult>(modelName);
            }

            List<TResult> list = new List<TResult>();
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                TResult entity = SetObjectValues<TResult>(reader, modelName);
                list.Add(entity);
            }

            return list;
        }

        private List<TResult> AutoFillEntity<TResult>(DbDataReader reader) where TResult : new()
        {
            var modelName = typeof(TResult).Name;

            if (!MyModels.ContainsKey(modelName))
                RefelectionModels<TResult>(modelName);

            List<TResult> list = new List<TResult>();
            while (reader.Read())
            {
                TResult entity = SetObjectValues<TResult>(reader, modelName);
                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 反射实体属性
        /// </summary>
        private static void RefelectionModels<TResult>(string modelName) where TResult : new()
        {
            var props = typeof(TResult).GetProperties();
            ConcurrentDictionary<string, PropertyInfo> ModelProps = new ConcurrentDictionary<string, PropertyInfo>();
            foreach (var prop in props)
            {
                ModelProps.GetOrAdd(prop.Name, prop);
            }

            MyModels.GetOrAdd(modelName, ModelProps);
        }

        /// <summary>
        /// 给实体类赋值
        /// </summary>
        private static TResult SetObjectValues<TResult>(DbDataReader reader, string modelName) where TResult : new()
        {
            var modelProps = MyModels[modelName];
            TResult entity = new TResult();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                if (modelProps.ContainsKey(fieldName))
                {
                    var proType = modelProps[fieldName];
                    var value = reader[fieldName];

                    if (value == DBNull.Value)
                    {
                        proType.SetValue(entity, null);
                    }
                    else
                    {
                        proType.SetValue(entity, Convert.ChangeType(value, proType.PropertyType));
                    }
                }
            }

            return entity;
        }

    }

    /// <summary>
    /// 扩展EFCore 的SQL查询接口
    /// </summary>
    public static class ExtendSql
    {
        public static MyDbRawSqlQuery<T> SqlQuery<T>(this DatabaseFacade database, string sql, params object[] parameters) where T : new()
        {
            try
            {
                return new MyDbRawSqlQuery<T>(database.GetDbConnection(), sql, parameters);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
