using BlueCarGpsLib.Data;
using BlueCarGpsLib.Service.Implement;
using BlueCarGpsWeb.Properties;
using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace BlueCarGpsWeb.App_Start
{
    public class AuthCacheConfig
    {
        static string cacheKey = "authCache";

        public static void RegisterCacheAndDependency()
        {
            // 开启数据库依赖
            // 主要是权限改变时，自动更新Cache中缓存的权限值
            try
            {
                System.Data.SqlClient.SqlDependency.Start(Settings.Default.db);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
            Cache authCache = HttpRuntime.Cache[cacheKey] as Cache;
            if (authCache == null)
            {
                SetAuth();
                // HttpContext.Current.Cache.Insert("authCache", 1);
            }
        }


        public static void UnRegisterCacheAndDependency()
        {
            try
            {
                System.Data.SqlClient.SqlDependency.Stop(Settings.Default.db);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        public static List<User> GetAuth()
        {
            lock (typeof(AuthCacheConfig))
            {
                if (HttpRuntime.Cache[cacheKey] == null)
                {
                    SetAuth();
                    return HttpRuntime.Cache[cacheKey] as List<User>;
                }
                else
                {
                    return HttpRuntime.Cache[cacheKey] as List<User>;
                }
            }
        }


        private static void SetAuth()
        {
            lock (typeof(AuthCacheConfig))
            {
                SqlCacheDependency dep = null;
                try
                {
                    dep = new SqlCacheDependency("BlueCarGps", "User");
                }
                catch (DatabaseNotEnabledForNotificationException exDBDis)
                {
                    LogUtil.Logger.Error(exDBDis.Message, exDBDis);

                    try
                    {
                        SqlCacheDependencyAdmin.EnableNotifications(Settings.Default.db);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        LogUtil.Logger.Error(ex.Message, ex);
                    }
                }
                catch (TableNotEnabledForNotificationException exTabDis)
                {
                    LogUtil.Logger.Error(exTabDis.Message, exTabDis);

                    try
                    {
                        SqlCacheDependencyAdmin.EnableTableForNotifications(Settings.Default.db, "User");
                    }
                    catch (SqlException ex)
                    {
                        LogUtil.Logger.Error(ex.Message, ex);

                    }
                }
                finally
                {
                    //HttpRuntime.Cache.Insert(
                    //    cacheKey,
                    //    new UserService(Settings.Default.db).All(),
                    //    dep,
                    //    Cache.NoAbsoluteExpiration,
                    //    Cache.NoSlidingExpiration,
                    //    CacheItemPriority.High,
                    //    null);
                    HttpRuntime.Cache.Insert(
                       cacheKey,
                       new UserService(Settings.Default.db).All(),
                       dep);
                }
            }
        }

        #region 初始化依赖项
        /// <summary>
        /// 初始化依赖项
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static System.Web.Caching.SqlCacheDependency initalSqlCacheDependency(string tablename)
        {
            //依赖数据库codematic中的P_Product表变化 来更新缓存
            System.Web.Caching.SqlCacheDependency dep = new System.Web.Caching.SqlCacheDependency("OnLineTest", tablename);
            return dep;
        }
        #endregion

        /// <summary>
        /// 数据库依赖方法
        /// </summary>
        /// <param name="strCon"></param>
        /// <param name="strSql"></param>
        /// <param name="sqlDep_OnChange"></param>
        private static void AddSqlDependency(string strCon, string strSql, OnChangeEventHandler sqlDep_OnChange)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    SqlCommand comm = new SqlCommand(strSql, conn);
                    SqlDependency sqlDep = new SqlDependency(comm);
                    sqlDep.OnChange += sqlDep_OnChange;
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
    }
}

#region 参考资料
// [HttpContext.Current.Cache 过期时间] http://www.cnblogs.com/ruiati/p/3248451.html
// [Cache及(HttpRuntime.Cache与HttpContext.Current.Cache)] http://www.cnblogs.com/McJeremy/archive/2008/12/01/1344660.html
// [Sql Server 2005/2008 SqlCacheDependency查询通知的使用总结] http://www.cnblogs.com/liuzhendong/archive/2010/03/25/1695526.html
// [Query Notification using SqlDependency and SqlCacheDependency] http://www.codeproject.com/Articles/144344/Query-Notification-using-SqlDependency-and-SqlCach
#endregion