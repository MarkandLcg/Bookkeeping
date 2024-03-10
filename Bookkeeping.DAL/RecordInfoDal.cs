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
    public class RecordInfoDal
    {
        /// <summary>
        /// 查询记录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable SelectRecordInfo(long userId)
        {
            //准备sql语句
            string sql = "SELECT * FROM RecordInfo WHERE UserID = @UserID";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 查询支出或者收入记录信息
        /// </summary>
        /// <param name="inOrOut"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable SelectRecordInfo(string inOrOut, long userId)
        {
            //准备sql语句
            string sql = "SELECT BillType AS '类型', Money AS '金额', Remark AS '备注', RecordTime AS '日期' FROM  RecordInfo WHERE InOrOut = @InOrOut AND UserID = @UserID";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@InOrOut", inOrOut),
                new SqlParameter("@UserID", userId)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 添加支出或者收入记录信息
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public int InsertRecordInfo(RecordInfo recordInfo)
        {
            //准备sql语句
            string sql = "INSERT INTO RecordInfo(UserID, BillType, InOrOut, Money, Remark, RecordTime) values (@UserID, @BillType, @InOrOut, @Money, @Remark, @RecordTime)";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", recordInfo.UserID),
                new SqlParameter("@BillType", recordInfo.BillType),
                new SqlParameter("@InOrOut", recordInfo.InOrOut),
                new SqlParameter("@Money", recordInfo.Money),
                new SqlParameter("@Remark", recordInfo.Remark),
                new SqlParameter("@RecordTime", recordInfo.RecordTime)
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }

        /// <summary>
        /// 查询某一天的收入或支出
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inOrOut"></param>
        /// <param name="recordTime"></param>
        /// <returns></returns>
        public DataTable SelectMoney(long userId, string inOrOut, DateTime recordTime)
        {
            //准备sql语句
            string sql = "SELECT SUM(Money) FROM RecordInfo WHERE UserID = @UserID AND InOrOut = @InOrOut AND RecordTime = @RecordTime";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@InOrOut", inOrOut),
                new SqlParameter("@RecordTime", recordTime)
            };
            //找SQLHelper
            return SQLHelper.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// 删除支出或者收入记录信息
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public int DeleteRecordInfo(RecordInfo recordInfo)
        {
            //准备sql语句
            string sql = "DELETE RecordInfo WHERE UserID = @UserID AND BillType = @BillType AND InOrOut = @InOrOut AND Money = @Money AND Remark = @Remark And RecordTime = @RecordTime";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", recordInfo.UserID),
                new SqlParameter("@BillType", recordInfo.BillType),
                new SqlParameter("@InOrOut", recordInfo.InOrOut),
                new SqlParameter("@Money", recordInfo.Money),
                new SqlParameter("@Remark", recordInfo.Remark),
                new SqlParameter("@RecordTime", recordInfo.RecordTime)
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }

        /// <summary>
        /// 删除用户支出或者收入信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteRecordInfo(long userId)
        {
            //准备sql语句
            string sql = "DELETE FROM RecordInfo WHERE UserID = @UserID";
            //准备参数
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId)
            };
            //找sqlhelper帮助类
            return SQLHelper.SqlCommand(sql, parameters);
        }
    }
}
