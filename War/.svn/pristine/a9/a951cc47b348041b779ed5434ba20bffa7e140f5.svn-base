
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace server
{
    /// <summary>
    /// CacheModel
    /// </summary>
    public partial class UserCacheModel : AbstractCacheModel
    {
        #region UserCacheModel 私有构造
        /// <summary>
        /// 私有构造
        /// </summary>
        private UserCacheModel()
        {

        }
        #endregion

        #region 单例
        private static object lock_object = new object();
        private static UserCacheModel instance = null;
        public static UserCacheModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lock_object)
                    {
                        if (instance == null)
                        {
                            instance = new UserCacheModel();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region DBModel 数据模型层单例
        /// <summary>
        /// 数据模型层单例
        /// </summary>
        private UserDBModel DBModel { get { return UserDBModel.Instance; } }
        #endregion

        #region Create 创建
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Create(UserEntity entity)
        {
            return this.DBModel.Create(entity);
        }
        #endregion

        #region Update 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue<object> Update(UserEntity entity)
        {
            return this.DBModel.Update(entity);
        }
        #endregion

        #region Delete 根据编号删除
        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnValue<object> Delete(int? id)
        {
            return this.DBModel.Delete(id);
        }

        #region GetEntity
        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserEntity GetEntity(int? id)
        {
            return this.DBModel.GetEntity(id);
        }

        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isAutoStatus"></param>
        /// <returns></returns>
        public UserEntity GetEntity(string condition, bool isAutoStatus = true)
        {
            return this.DBModel.GetEntity(condition, isAutoStatus);
        }
        #endregion
        #endregion
    }
}