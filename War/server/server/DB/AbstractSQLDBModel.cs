using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace server.DB
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

        #endregion

        #region ColumnList 列集合
        /// <summary>
        /// 列集合
        /// </summary>
        protected abstract IList<string> ColumnList { get; }
        #endregion

        #region 增 删 改
        /// <summary>
        /// 新增对象（存储过程）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<T> Create(T entity)
        {
            return Create(null, entity);
        }
        
        /// <summary>
        /// 新增对象（存储过程）
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<T> Create(SqlTransaction trans,T entity)
        {
            ReturnValue<T> ret = new ReturnValue<T>();
            SqlParameter[] paramArray = ConverEntityToParam(entity);
            paramArray[0].Direction = ParameterDirection.Output;

            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;
            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;

            if (trans == null)
            {
                SqlHelper.ExcuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Create", TableName), paramArray);
            }
            else
            {
                SqlHelper.ExcuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}_Create", TableName), paramArray);
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

        /// <summary>
        /// 修改对象（存储过程）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<T> Updata(T entity)
        {
            return Update(null, entity);
        }

        /// <summary>
        /// 修改对象(存储过程)
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<T> Update(SqlTransaction trans, T entity)
        {
            ReturnValue<T> ret = new ReturnValue<T>();
            SqlParameter[] paramArray = ConverEntityToParam(entity);
            paramArray[0].Direction = ParameterDirection.Output;

            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;
            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;

            if (trans == null)
            {
                SqlHelper.ExcuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Update", TableName), paramArray);
            }
            else
            {
                SqlHelper.ExcuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}_Update", TableName), paramArray);
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

        /// <summary>
        /// 删除对象（删除指定id的对象,存储过程）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnValue<T> Delete(int? id)
        {
            return Delete(id, null);
        }

        /// <summary>
        /// 删除语句(删除指定id的对象,存储过程)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public ReturnValue<T> Delete(int? id ,SqlTransaction trans)
        {
            ReturnValue<T> ret = new ReturnValue<T>();
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

            }
            else
            {

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
    }
}
