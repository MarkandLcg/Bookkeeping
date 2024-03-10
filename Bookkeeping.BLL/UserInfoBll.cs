using Bookkeeping.DAL;
using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.BLL
{
    public class UserInfoBll
    {
        /// <summary>
        /// 查询用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistUserName(string userName)
        {
            UserInfoDal dal = new UserInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectUserName(userName);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询Id
        /// </summary>
        /// <returns></returns>
        public long ExistUserId()
        {
            UserInfoDal dal = new UserInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectUserId();
            if(dt.Rows.Count > 0)
            {
                string strUserId = dt.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(strUserId))    //UserInfo表为空没有UserId
                {
                    return 1;
                }
                else
                {
                    return (Convert.ToInt64(dt.Rows[0][0].ToString()) + 1); //UserInfo表不为空
                }
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool AddUserInfo(UserInfo userInfo)
        {
            UserInfoDal dal = new UserInfoDal();
            int i = dal.InsertUserInfo(userInfo);
            if(i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string userName, string password)
        {
            return new UserInfoDal().SelectUserInfo(userName, password).Rows.Count > 0;
        }

        /// <summary>
        /// 查询邮箱
        /// </summary>
        /// <param name="userMail"></param>
        /// <returns></returns>
        public bool ExistUserMail(string userMail)
        {
            return new UserInfoDal().SelectUserMail(userMail).Rows.Count > 0;
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="userMail"></param>
        /// <returns></returns>
        public bool ForgetPassword(string userPassword, string userMail)
        {
            return new UserInfoDal().UpdatePassword(userPassword, userMail) > 0;
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] ExistUserInfo(string userName)
        {
            UserInfoDal dal = new UserInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectUserInfo(userName);
            if (dt.Rows.Count > 0)
            {
                string UserId = dt.Rows[0][0].ToString();
                string UserMail = dt.Rows[0][3].ToString();
                return new string[] { UserId, UserMail };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询头像
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public byte[] ExistUserPhoto(string userName) 
        {
            UserInfoDal dal = new UserInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectUserInfo(userName);
            if(dt.Rows.Count > 0)
            {
                byte[] UserPhoto = (Byte[])dt.Rows[0][4];
                return UserPhoto;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(long userId)
        {
            return new UserInfoDal().DeleteUserInfo(userId) > 0;
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPhoto"></param>
        /// <returns></returns>
        public bool ChangeUserPhoto(long userId, byte[] userPhoto)
        {
            return new UserInfoDal().UpdateUserPhoto(userId, userPhoto) > 0;
        }
    }
}
