using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace server
{
    /// <summary>
    /// DBModel抽象基类
    /// </summary>
    public abstract class AbstractSQLDBModel<T>
    {
        #region 抽象属性或方法
        #region ConnectionString 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected abstract string ConnectionString { get; }
        #endregion

        #region TableName 表名
        /// <summary>
        /// 表名
        /// </summary>
        protected abstract string TableName { get; }
        #endregion

        #region ConverEntityToParam 将实体属性转换为参数
        /// <summary>
        /// 将实体属性转换为参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract SqlParameter[] ConverEntityToParam(T entity);
        #endregion

        #region  封装实体
        protected abstract T GetEntitySelfProperty(IDataReader reader,DataTable table);
        #endregion

        #region ColumnList 列集合
        /// <summary>
        /// 列集合
        /// </summary>
        protected abstract IList<string> ColumnList { get; }
        #endregion

        #region 增 删 改
        #region Create 新增对象,无事物(存储过程）
        /// <summary>
        /// 新增对象,无事物(存储过程）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Create(T entity)
        {
            return Create(null, entity);
           
        }
        #endregion

        #region Create 新增对象,有事物(存储过程）
        /// <summary>
        /// 新增对象,有事物(存储过程）
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Create(SqlTransaction trans,T entity)
        {
            ReturnValue<object> ret = new ReturnValue<object>();
            SqlParameter[] paramArray = ConverEntityToParam(entity);
            paramArray[0].Direction = ParameterDirection.Output;

            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;
            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;

            if (trans == null)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Create", TableName), paramArray);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}_Create", TableName), paramArray);
            }
            int nReturnCode =Convert.ToInt32(paramArray[paramArray.Length - 1].Value);
            if (nReturnCode < 0)
            {
                ret.HasError = true;
            }
            else
            {
                ret.HasError = false;
                ret.Id = Convert.ToInt32(paramArray[0].Value);
            }
            ret.Message = paramArray[paramArray.Length - 2].ToString();
            ret.ReturnCode = nReturnCode;
            return ret;
        }
        #endregion

        #region Updata 修改对象,无事物（存储过程）
        /// <summary>
        /// 修改对象,无事物（存储过程）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Update(T entity)
        {
            return Update(null, entity);
        }
        #endregion

        #region Updata 修改对象，有事物(存储过程)
        /// <summary>
        /// 修改对象，有事物(存储过程)
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Update(SqlTransaction trans, T entity)
        {
            ReturnValue<object> ret = new ReturnValue<object>();
            SqlParameter[] paramArray = ConverEntityToParam(entity);
            paramArray[0].Direction = ParameterDirection.Output;

            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;
            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;

            if (trans == null)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Update", TableName), paramArray);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}_Update", TableName), paramArray);
            }
            int nReturnCode = Convert.ToInt32(paramArray[paramArray.Length - 1].Value);
            if (nReturnCode < 0)
            {
                ret.HasError = true;
            }
            else
            {
                ret.HasError = false;             
            }
            ret.Message = paramArray[paramArray.Length - 2].Value.ToString();
            ret.ReturnCode = nReturnCode;
            return ret;
        }

        #endregion

        #region Delete 删除对象，无事物（删除指定id的对象,存储过程）
        /// <summary>
        /// 删除对象，无事物（删除指定id的对象,存储过程）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnValue<object> Delete(int? id)
        {
            return Delete(id, null);
        }
        #endregion

        #region Delete 删除语句,有事物(删除指定id的对象,存储过程)
        /// <summary>
        /// 删除语句,有事物(删除指定id的对象,存储过程)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public ReturnValue<object> Delete(int? id ,SqlTransaction trans)
        {
            ReturnValue<object> ret = new ReturnValue<object>();
            int nId;
            if (id.HasValue)
            {
                nId = id.Value;
            }
            else
            {
                ret.HasError = true;
                ret.Message = "不合规的主键";
                return ret ;
            }
            SqlParameter[] paramArray = new SqlParameter[]
            {
                new SqlParameter("@Id",nId),
                new SqlParameter("@RetMsg",SqlDbType.NVarChar,255),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;
            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;

            if (trans == null)
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Delete", TableName), paramArray);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}_Delete", TableName), paramArray);
            }
            int nReturnCode = Convert.ToInt32(paramArray[paramArray.Length - 1].Value);
            if (nReturnCode < 0)
            {
                ret.HasError = true;
            }
            else
            {
                ret.HasError = false;
            }
            ret.Message = paramArray[paramArray.Length - 2].Value.ToString();
            ret.ReturnCode = nReturnCode;
            return ret;
        }
        #endregion
        #endregion

        #region 查
        #region GetEntity 根据id查询实体(存储过程)
        /// <summary>
        /// 根据id查询实体(存储过程)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity(int ? id)
        {
            int nId;
            if (id.HasValue)
            {
                nId = id.Value;
            }
            else
            {
                throw new Exception("主键不符合规定。");
            }
            SqlParameter[] paramArray = new SqlParameter[]
            {
                new SqlParameter("@Id",nId)
            };
            return GetEntity(string.Format("{0}_GetEntity",TableName),paramArray);
        }
        #endregion

        #region GetEntity 根据自定义条件查询实体
        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isAutoStatus"></param>
        /// <returns></returns>
        public T GetEntity(string condition,bool isAutoStatus=true)
        {
            if (isAutoStatus && ColumnList.Contains("status") && condition.IndexOf("status") == -1)
            {
                var statusString = "status = 1";
                condition = statusString + (condition == null ? string.Empty : "And") + condition;
            }
            return GetEntity(string.Format("select * from [{0}] where {1}", TableName, condition), null, CommandType.Text);
        }
        #endregion

        #region  GetEntity 查询实体（自定义sql语句或存储过程）
        /// <summary>
        /// 查询实体（自定义sql语句或存储过程）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        public T GetEntity(string sql, SqlParameter[] paramArray,CommandType commandType =CommandType.StoredProcedure)
        {
            T entity = default(T);
            using (IDataReader reader = SqlHelper.ExecuteReader(ConnectionString,commandType,sql,paramArray))
            {
                if (reader != null && reader.Read())
                {
                    DataTable columnData= reader.GetSchemaTable();
                    entity = GetEntitySelfProperty(reader,columnData);
                }
            }
                return entity;
        }
        #endregion

        #region

        #endregion

        #endregion

        #endregion
    }
}
