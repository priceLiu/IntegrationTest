using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CN100.EnterprisePlatform.Wcf.Core;
using CN100.Orders.IBLL;
using System.Configuration;
using CN100.EnterprisePlatform.Wcf.Core.Config;

namespace IntegrationTest.App
{
    class OrderListTest : IntegrationTest.TestItem
    {
        private readonly static string test = ConfigurationManager.AppSettings["Test"];
        private static WcfClientFactory factory = WcfClients.GetFactory("Orders");
        private readonly static string OrderStatus = ConfigurationManager.AppSettings["type"];//订单状态


        protected override void OnExecute()
        {
            //if (test == "1")
            //{
            SellerOrderListTest();
            //    //GetOrderByStatus();
            //}
            //else
            //{
            //    BuyerOrderByStatus();
            //}


            System.Threading.Thread.Sleep(1);
        }



        public static void SellerOrderListTest()
        {
            List<int> storeidList = new List<int>() { 5533, 5541, 5937, 7115, 5633, 5710, 6326, 5647 };
            foreach (var item in storeidList)
            {
                GetOrderList("", item.ToString());
            }
        }


        /// <summary>
        /// 循环调用卖家订单列表方法
        /// </summary>
        private static void GetOrderByStatus()
        {
            string sql = "select distinct(storeid) from orderbase where DATASOURCESTYPE=9";
            DataSet dad = DataHelper.ExeRead(sql);

            if (dad.Tables[0].Rows.Count > 0 && dad != null)
            {
                for (int j = 0; j < dad.Tables[0].Rows.Count; j++)
                {
                    DataRow row = dad.Tables[0].Rows[j];
                    GetOrderList("", row["storeid"].ToString());
                }
            }

        }

        /// <summary>
        /// 循环调用买家订单列表方法
        /// </summary>
        private static void BuyerOrderByStatus()
        {

            string sql = "select distinct(buyerid) from orderbase where DATASOURCESTYPE=9";
            DataSet dad = DataHelper.ExeRead(sql);

            if (dad.Tables[0].Rows.Count > 0 && dad != null)
            {
                for (int j = 0; j < dad.Tables[0].Rows.Count; j++)
                {
                    DataRow row = dad.Tables[0].Rows[j];
                    GetOrderList(row["buyerid"].ToString(), "");
                }
            }

        }


        private static void GetOrderList(string buyerid, string storeid)
        {
            int TotalPageCount = 0;
            int TotalCount = 0;
            using (WcfTcpClient<IOrderService> client = factory.CreateClient<IOrderService>())
            {
                Random rand = new Random();
                int pointRD = rand.Next(1, 10);

                IList<CN100.Orders.BLL.Model.OrderAndOrderDetailAndOrderConsigneeBM> m = client.Channel.GetOrderAndOrderDetailAndOrderConsigneeList("", "", buyerid, storeid, "", "", "", "", "", "", "", "", "", 1, 20, ref TotalPageCount, ref TotalCount);
                //client.Channel.GetOrderAndOrderDetailAndOrderConsigneeList(OrderCode, OrderID, BuyerID, SellerID, BuyerName, SellerName, ProductName, OrderStatus, EvaluationState, AfterSaleService, IsHideCloseOrder, BeginTime, EndTime, PageIndex, PageSize, ref TotalPageCount, ref TotalCount);
                if (m != null && m.Count > 0)
                {
                }
                else
                {
                    Console.WriteLine("查无数据");
                    throw new Exception("查无数据");
                }
            }
        }
    }
}
