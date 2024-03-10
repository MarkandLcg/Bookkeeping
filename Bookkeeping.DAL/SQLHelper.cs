using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.DAL
{
    /// <summary>
    /// SQL帮助类：用来执行增删改查操作
    /// </summary>
    public static class SQLHelper
    {
        //准备连接数据库的字符串
        private static string sqlCon = "server=.;database=bookkeepingdb;Trusted_Connection=True";

        /// <summary>
        /// 执行增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int SqlCommand(string sql, SqlParameter[] parameters)
        {
            //创建连接对象
            using(SqlConnection connection = new SqlConnection(sqlCon))
            {
                connection.Open();
                //准备操作对象
                using(SqlCommand command = new SqlCommand(sql, connection))
                {
                    //判断是否有参数
                    if(parameters != null)
                    {
                        //添加参数
                        command.Parameters.AddRange(parameters);
                        //执行sql语句
                        return command.ExecuteNonQuery();   //执行成功，会返回影响的行数
                    }
                    else
                    {
                        return command.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            //创建连接对象
            using(SqlConnection connection = new SqlConnection(sqlCon)) 
            {
                //准备操作对象
                using(SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection))
                {
                    //判断是否有参数
                    if(parameters != null)
                    {
                        //添加参数
                        dataAdapter.SelectCommand.Parameters.AddRange(parameters);
                        //准备容器
                        DataTable dt = new DataTable();
                        dataAdapter.Fill(dt);
                        return dt;
                    }
                    else
                    {
                        //准备容器
                        DataTable dt = new DataTable();
                        dataAdapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
