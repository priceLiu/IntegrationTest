using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CN100.EnterprisePlatform.Wcf.Core;
using CN100.Member.IBLL;
using CN100.EnterprisePlatform.Wcf.Core.Config;
using CN100.Member.IBLL.Modules;
using System.Data;

namespace IntegrationTest.App
{
    public class LoginTest : IntegrationTest.TestItem
    {
        private static WcfClientFactory factory = WcfClients.GetFactory("Member");


        protected override void OnExecute()
        {
            SetUserInfo();

            System.Threading.Thread.Sleep(1);
        }

        public static void SetUserInfo()
        {
            string sql = "select t.NAME,t.PASSWORD from user_curd.userbase t where rownum <10 and t.password='E10ADC3949BA59ABBE56E057F20F883E'";
            DataSet dad = DataHelper.ExeRead(sql);

            if (dad.Tables[0].Rows.Count > 0 && dad != null)
            {
                for (int j = 0; j < dad.Tables[0].Rows.Count; j++)
                {
                    DataRow row = dad.Tables[0].Rows[j];
                    LoginWCF(row["name"].ToString(), row["password"].ToString());
                }
            }
        }


        public static void LoginWCF(string userName, string userPWD)
        {
            //登陆wcf
            using (WcfTcpClient<IMemberRetister> client = factory.CreateClient<IMemberRetister>())
            {
                LoginInfo loginInfo = new LoginInfo();
                loginInfo.UserName = userName;
                loginInfo.UserPWD = "123456";

                LoginResult result = new LoginResult();
                loginInfo.IP = "192.168.1.1";
                loginInfo.Url = "";
                if (loginInfo.Url.Length > 300)
                {
                    loginInfo.Url = loginInfo.Url.Substring(0, 250);
                }
                if (string.IsNullOrEmpty(loginInfo.SubUserName))
                {
                    result = client.Channel.Login(loginInfo);
                }
                else
                {
                    result = client.Channel.Login(loginInfo, true);
                }
            }
        }


    }
}
