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
    public class RecordInfoBll
    {
        /// <summary>
        /// 查询钱
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public float[] ExistMoney(long userId)
        {
            RecordInfoDal dal = new RecordInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectRecordInfo(userId);
            if(dt.Rows.Count > 0 )
            {
                float inMoney = 0;
                float outMoney = 0;
                float totalMoney = 0;
                string inOrOut;
                float money;
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    inOrOut = dt.Rows[i][2].ToString().Trim();
                    if(inOrOut.Equals("支出"))
                    {
                        money = Convert.ToSingle(dt.Rows[i][3].ToString());
                        outMoney += money;
                    }
                    else if(inOrOut.Equals("收入"))
                    {
                        money = Convert.ToSingle(dt.Rows[i][3].ToString());
                        inMoney += money;
                    }
                }
                totalMoney = inMoney - outMoney;
                return new float[] { inMoney, outMoney, totalMoney };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询支出或者收入记录信息
        /// </summary>
        /// <param name="inOrOut"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable ExistRecordInfo(string inOrOut, long userId)
        {
            RecordInfoDal dal = new RecordInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectRecordInfo(inOrOut, userId);
            if(dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加支出或者收入记录信息
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public bool AddRecordInfo(RecordInfo recordInfo)
        {
            return new RecordInfoDal().InsertRecordInfo(recordInfo) > 0;
        }

        /// <summary>
        /// 查询某一天的收入或支出
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inOrOut"></param>
        /// <param name="recordTime"></param>
        /// <returns></returns>
        public float ExistMoney(long userId, string inOrOut, DateTime recordTime)
        {
            RecordInfoDal dal = new RecordInfoDal();
            DataTable dt = new DataTable(); //用来接收从dal层查回来的数据
            dt = dal.SelectMoney(userId, inOrOut, recordTime);
            if (dt.Rows.Count > 0)
            {
                string strMoney = dt.Rows[0][0].ToString();
                if(string.IsNullOrEmpty(strMoney))  //查询到钱的数据为空
                {
                    return 0;
                }
                else
                {
                    return Convert.ToSingle(strMoney);  //查询到钱的数据不为空
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除支出或者收入记录信息
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public bool DeleteRecordInfo(RecordInfo recordInfo)
        {
            return new RecordInfoDal().DeleteRecordInfo(recordInfo) > 0;
        }

        /// <summary>
        /// 删除用户支出或者收入记录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteRecordInfo(long userId)
        {
            return new RecordInfoDal().DeleteRecordInfo(userId) > 0;
        }
    }
}
