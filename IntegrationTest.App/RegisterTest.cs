using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CN100.EnterprisePlatform.Wcf.Core;
using CN100.EnterprisePlatform.Wcf.Core.Config;
using CN100.Member.IBLL;
using CN100.Member.IBLL.Modules;

namespace IntegrationTest.App
{
    public class RegisterTest : TestItem
    {
        private static WcfClientFactory factory = WcfClients.GetFactory("Member");
        private static Random rd = new Random();


        protected override void OnExecute()
        {
            RegisterWCF();
        }

        public static void RegisterWCF()
        {
            try
            {
                using (WcfTcpClient<IMemberRetister> client = factory.CreateClient<IMemberRetister>())
                {


                    RegisterInfo ri = new RegisterInfo
                    {
                        ContactQQ = "",
                        Email = "",
                        Phone = "150" + rd.Next(10000000, 99999999).ToString(),
                        RealName = "",
                        RegistrationIP = "192.168.0.0",
                        Status = CN100.Member.Enums.UserStatus.Available,
                        UserName = "WCF压力测试" + rd.Next(100, 10000000).ToString(),
                        UserType = CN100.Member.Enums.UserType.Member,
                        UserPWD = "123456",
                        strength_V = Convert.ToInt16("1")
                    };
                    long userId = client.Channel.RegisterResUserId(ri);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}

