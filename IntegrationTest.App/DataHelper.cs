using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace IntegrationTest.App
{
    class DataHelper
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ToString();

        /// <summary>
        /// 执行数据读取
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>数据集</returns>
        public static DataSet ExeRead(string sql)
        {
            DataSet set = new DataSet();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand comm = new OracleCommand(sql, conn);
                OracleDataAdapter dad = new OracleDataAdapter(comm);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                dad.Fill(set);
            }
            return set;
        }


        /// <summary>
        /// 执行数据读取
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>数据集</returns>
        public static int ExeComm(string sql)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand comm = new OracleCommand(sql, conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                return comm.ExecuteNonQuery();
            }
        }
    }
}
