using Bookkeeping.Common;
using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.DAL
{
    public class UserInfoDal
    {
        /// <summary>
        /// 查询用户名
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable SelectUserName(string UserName)
        {
            //准备sql语句
            string sql = "SELECT UserName FROM UserInfo WHERE UserName = @UserName";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", UserName)
            };
            //找sqlhelper帮助类
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 查询Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable SelectUserId()
        {
            //准备sql语句
            string sql = "SELECT MAX(UserId) FROM UserInfo";
            //找sqlhelper帮助类
            return SQLHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int InsertUserInfo(UserInfo userInfo)
        {
            //密码MD5加密
            string UserPasswordMD5 = MD5Helper.GenerateMD5(userInfo.UserPassword);
            //准备sql语句
            string sql = "INSERT INTO UserInfo(UserID, UserName, UserPassword, UserMail, UserPhoto) VALUES(@UserID, @UserName, @UserPassword, @UserMail, @UserPhoto)";
            //UserPhoto参数
            SqlParameter UserPhotoParameter = new SqlParameter("@UserPhoto", SqlDbType.Image);
            UserPhotoParameter.Value = userInfo.UserPhoto;
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userInfo.UserID),
                new SqlParameter("@UserName", userInfo.UserName),
                new SqlParameter("@UserPassword", UserPasswordMD5),
                new SqlParameter("@UserMail", userInfo.UserMail),
                UserPhotoParameter
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }

        /// <summary>
        /// 查询用户名和密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataTable SelectUserInfo(string userName, string password) 
        {
            //准备sql语句
            string sql = "SELECT UserName, UserPassword FROM UserInfo WHERE UserName = @UserName AND UserPassword = @UserPassword";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", userName),
                new SqlParameter("@UserPassword", password)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 查询邮箱
        /// </summary>
        /// <param name="userMail"></param>
        /// <returns></returns>
        public DataTable SelectUserMail(string userMail)
        {
            //准备sql语句
            string sql = "SELECT UserMail FROM UserInfo WHERE UserMail = @UserMail";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserMail", userMail)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="userMail"></param>
        /// <returns></returns>
        public int UpdatePassword(string userPassword, string userMail)
        {
            //密码MD5加密
            string userPasswordMD5 = MD5Helper.GenerateMD5(userPassword);
            //准备sql语句
            string sql = "UPDATE UserInfo SET UserPassword = @UserPassword WHERE UserMail = @UserMail";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserPassword", userPasswordMD5),
                new SqlParameter("@UserMail", userMail)
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable SelectUserInfo(string userName)
        {
            //准备sql语句
            string sql = "SELECT * FROM UserInfo WHERE UserName = @UserName";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", userName)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUserInfo(long userId)
        {
            //准备sql语句
            string sql = "DELETE FROM UserInfo WHERE UserID = @UserID";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId)
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPhoto"></param>
        /// <returns></returns>
        public int UpdateUserPhoto(long userId, byte[] userPhoto)
        {
            //准备sql语句
            string sql = "UPDATE UserInfo SET UserPhoto = @UserPhoto WHERE UserID = @UserID";
            //UserPhoto参数
            SqlParameter UserPhotoParameter = new SqlParameter("@UserPhoto", SqlDbType.Image);
            UserPhotoParameter.Value = userPhoto;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId),
                UserPhotoParameter
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }
    }
}
