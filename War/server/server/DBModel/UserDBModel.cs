using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
 namespace server
{
    /// <summary>
    /// DBModel
    /// </summary>
    public partial class UserDBModel : AbstractSQLDBModel<UserEntity>
    {
        #region UserDBModel 私有构造
        /// <summary>
        /// 私有构造
        /// </summary>
        private UserDBModel()
        {

        }
        #endregion

        #region 单例
        private static object lock_object = new object();
        private static UserDBModel instance = null;
        public static UserDBModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lock_object)
                    {
                        if (instance == null)
                        {
                            instance = new UserDBModel();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 实现基类的属性和方法

        #region ConnectionString 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected override string ConnectionString
        {
            get { return "Data Source=T4F-PC-18238;Initial Catalog=Account;User ID=lsx;Password=Amegztgz13"; }
        }
        #endregion

        #region TableName 表名
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "User"; }
        }
        #endregion

        #region ColumnList 列名集合
        private IList<string> _ColumnList;
        /// <summary>
        /// 列名集合
        /// </summary>
        protected override IList<string> ColumnList
        {
            get
            {
                if (_ColumnList == null)
                {
                    _ColumnList = new List<string> { "Id", "Status", "UserName", "Pwd", "NickName", "GoldCoin", "Diamond", "CreateTime", "UpdateTime" };
                }
                return _ColumnList;
            }
        }
        #endregion

        #region ValueParas 转换参数
        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override SqlParameter[] ConverEntityToParam(UserEntity entity)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Id", entity.Id) { DbType = DbType.Int32 },
                new SqlParameter("@Status", entity.Status) { DbType = DbType.Byte },
                new SqlParameter("@UserName", entity.UserName) { DbType = DbType.String },
                new SqlParameter("@Pwd", entity.Pwd) { DbType = DbType.String },
                new SqlParameter("@NickName", entity.NickName) { DbType = DbType.String },
                new SqlParameter("@GoldCoin", entity.GoldCoin) { DbType = DbType.Int32 },
                new SqlParameter("@Diamond", entity.Diamond) { DbType = DbType.Int32 },
                new SqlParameter("@CreateTime", entity.CreateTime) { DbType = DbType.DateTime },
                new SqlParameter("@UpdateTime", entity.UpdateTime) { DbType = DbType.DateTime },
                new SqlParameter("@RetMsg", SqlDbType.NVarChar, 255),
                new SqlParameter("@ReturnValue", SqlDbType.Int)
            };
            return parameters;
        }
        #endregion

        #region GetEntitySelfProperty 封装对象
        /// <summary>
        /// 封装对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        protected override UserEntity GetEntitySelfProperty(IDataReader reader, DataTable table)
        {
            UserEntity entity = new UserEntity();
            foreach (DataRow row in table.Rows)
            {
                var colName = (string)row[0];
                if (reader[colName] is DBNull)
                    continue;
                switch (colName.ToLower())
                {
                    case "id":
                        if (!(reader["Id"] is DBNull))
                            entity.Id = Convert.ToInt32(reader["Id"]);
                        break;
                    case "status":
                        if (!(reader["Status"] is DBNull))
                            entity.Status = (EnumEntityStatus)Convert.ToInt32(reader["Status"]);
                        break;
                    case "username":
                        if (!(reader["UserName"] is DBNull))
                            entity.UserName = Convert.ToString(reader["UserName"]);
                        break;
                    case "pwd":
                        if (!(reader["Pwd"] is DBNull))
                            entity.Pwd = Convert.ToString(reader["Pwd"]);
                        break;
                    case "nickname":
                        if (!(reader["NickName"] is DBNull))
                            entity.NickName = Convert.ToString(reader["NickName"]);
                        break;
                    case "goldcoin":
                        if (!(reader["GoldCoin"] is DBNull))
                            entity.GoldCoin = Convert.ToInt32(reader["GoldCoin"]);
                        break;
                    case "diamond":
                        if (!(reader["Diamond"] is DBNull))
                            entity.Diamond = Convert.ToInt32(reader["Diamond"]);
                        break;
                    case "createtime":
                        if (!(reader["CreateTime"] is DBNull))
                            entity.CreateTime = Convert.ToDateTime(reader["CreateTime"]);
                        break;
                    case "updatetime":
                        if (!(reader["UpdateTime"] is DBNull))
                            entity.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                        break;
                }
            }
            return entity;
        }
        #endregion

        #endregion
    }
}