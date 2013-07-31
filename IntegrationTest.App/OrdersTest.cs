using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using CN100.EnterprisePlatform.Wcf.Core;
using CN100.Orders.IBLL;
using CN100.Orders.ViewModel;
using CN100.Orders.Utility;
using System.IO;
using System.Xml;
using CN100.EnterprisePlatform.Wcf.Core.Config;

namespace IntegrationTest.App
{
    public class OrdersTest : IntegrationTest.TestItem
    {
        private readonly static string runCount = ConfigurationManager.AppSettings["runCount"];
        private static WcfClientFactory factory = WcfClients.GetFactory("Orders");
        private static int i = 0;
        private static DataSet dsCart = new DataSet();
        private static Queue<CartModel> mDatas = new Queue<CartModel>();

        static OrdersTest()
        {
            GetCartId();
        }

        private static int GetIndex()
        {
            return System.Threading.Interlocked.Increment(ref i);
        }

        protected override void OnExecute()
        {
            // InsShopCart();
            CartModel cm;
            lock (mDatas)
            {
                if (mDatas.Count < 1)
                    return;
                cm = mDatas.Dequeue();
                AddOrder(cm);
            }

            System.Threading.Thread.Sleep(1);
        }

        /// <summary>
        /// 读取有效店铺插入购物车
        /// </summary>
        //private static void InsShopCart()
        //{
        //    string sql = "select storeid  from Sales_curd.Test_OrderList where rownum <=" + storeNum + " order by storeid desc ";
        //    DataSet ds = DataHelper.ExeRead(sql);

        //    //获取买家用户信息
        //    string usersql = "select id,name from user_curd.userbase where isdeleted=0 and status=1 and usertype=0 ";
        //    DataSet userds = DataHelper.ExeRead(usersql);

        //    if (userds.Tables[0].Rows.Count < int.Parse(proNum))
        //    {
        //        Console.WriteLine("用户数不够 " + int.Parse(proNum) + " 个。");
        //        return;
        //    }
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        int storeCount = 0;
        //        while (storeCount < ds.Tables[0].Rows.Count)
        //        {

        //            string storeId = ds.Tables[0].Rows[storeCount]["storeid"].ToString();
        //            //根据店铺id，找到商品id
        //            string prosql = "select p.id as proid,p.productname,p.storeid,p.storename,s.id as skuid from productInfo p inner join product_curd.productsku s on p.id=s.productid  where p.storeid =" + storeId;
        //            DataSet set = DataHelper.ExeRead(prosql);

        //            int count = 0;
        //            for (int k = 0; k < set.Tables[0].Rows.Count; k++)
        //            {
        //                DataRow dr = set.Tables[0].Rows[k];
        //                DataRow row = userds.Tables[0].Rows[count];
        //                string cartsql = string.Format("insert into Sales_curd.shoppingcart(ID,  BUYERID,  BUYERNAME, STOREID, STORENAME, PRODUCTID, PRODUCTNAME,SKUID, SALEUNITPRICE, "
        //                                                       + "MAINATTRIBUTENAME, SECONDATTRIBUTENAME, MAINATTRIBUTEVALUENAME, SECONDATTRIBUTEVALUENAME, BUYQTY, STATUS,MAINATTRIBUTEVALUEID, MAINATTRIBUTEID,"
        //                                                       + "SECONDATTRIBUTEVALUERID, SECONDATTRIBUTEID,CREATEDTIME, UPDATEDTIME, ISDELETED, DELETETIME,DATASOURCESTYPE) values (SEQ_SHOPPINGCART.nextval,"
        //                                                       + "{5},  '{6}',  {0},  '{1}',  {2}, '{3}', {4},500.00,'颜色',  '尺码',  '紫罗兰',  '60厘米 ( 1尺8）',  1,  0,  10979,  1309,  11016,  1308,sysdate,null,0,null,8)",
        //                                                       storeId, dr["storename"], dr["proid"], dr["productname"], dr["skuid"], row["id"], row["name"]);
        //                int j = DataHelper.ExeComm(cartsql);

        //                if (j > 0)
        //                {
        //                    count++;
        //                }

        //                //当此店铺产品已循环完，但不够5K则清零继续循环
        //                if (k == set.Tables[0].Rows.Count - 1)
        //                {
        //                    k = 0;
        //                }
        //                //当循环够5K，则停止循环
        //                if (count == int.Parse(proNum))
        //                {
        //                    k = set.Tables[0].Rows.Count;
        //                }
        //            }

        //            storeCount++;
        //        }
        //    }
        //}

        /// <summary>
        /// 获取所有购物车信息
        /// </summary>
        /// <returns>数据列表</returns>
        public static void GetCartId()
        {
            //WriteLog("读取数据时间：");
            string sql = "select id,buyerId,buyerName,storeId,storeName,productId,productName,skuId from sales_curd.shoppingcart unlock where DATASOURCESTYPE = 9 and status=0 and rownum< " + runCount;
            dsCart = DataHelper.ExeRead(sql);

            foreach (DataRow item in dsCart.Tables[0].Rows)
            {
                CartModel cart = null;
                SetCartInfo(item, out cart);
                mDatas.Enqueue(cart);
            }
        }

        /// <summary>
        /// 购物车数据的赋值
        /// </summary>
        /// <param name="dr">数据行</param>
        private static void SetCartInfo(DataRow dr, out CartModel cart)
        {
            cart = new CartModel();
            cart.CartID = dr["id"].ToString();
            cart.UID = dr["buyerId"].ToString();
            cart.UName = dr["buyerName"].ToString();
            cart.SID = dr["storeId"].ToString();
            cart.SName = dr["storeName"].ToString();
            cart.PId = dr["productId"].ToString();
            cart.PName = dr["productName"].ToString();
            cart.SkuId = dr["skuId"].ToString();

            //根据sku编号获取对应快照编号
            string skusql = "select a.id as skukzid, a.productsnapshotid as productkzid from product_curd.productskusnapshot a "
                            + " inner join product_curd.productsku b on a.skuid = b.id and a.snapshotversion = b.latestsnapshotversion and b.id = " + cart.SkuId;
            DataSet dst = DataHelper.ExeRead(skusql);
            if (dst != null && dst.Tables[0].Rows.Count > 0)
            {
                DataRow drow = dst.Tables[0].Rows[0];
                cart.ProkzID = drow["productkzid"].ToString();
                cart.SkukzID = drow["skukzid"].ToString();
            }

        }


        private static void AddOrder(CartModel cart)
        {
            using (WcfTcpClient<IProcedureFunctionService> client = factory.CreateClient<IProcedureFunctionService>())
            {

                string strxml = XMLHelper.ConvertXmlOjbectToString<ConfirmOrdeSaveXMLVM.xml>(setXmls(cart));
                byte[] by = System.Text.Encoding.UTF8.GetBytes(strxml);
                Stream s = new MemoryStream(by);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(s);
                s.Dispose();

                XmlNode xn = xmldoc.SelectSingleNode("xml");
                string xEleStr = xn.InnerXml.ToString();
                string outMessage = string.Empty;
                string payPrice = "0";
                long ticks = DateTime.Now.Ticks;

                int saveResult = client.Channel.SaveOrderFromShoppingCart2(long.Parse(cart.UID), xEleStr, out outMessage, out payPrice);

                if (saveResult == 0)
                {
                    Console.WriteLine("下单失败！" + cart.CartID + "\t " + outMessage + "，时间：" + DateTime.Now);
                    throw new Exception("下单失败");
                }


            }
        }


        /// <summary>
        /// 拼凑xml字符串
        /// </summary>
        private static ConfirmOrdeSaveXMLVM.xml setXmls(CartModel model)
        {
            ConfirmOrdeSaveXMLVM.xml xml = new ConfirmOrdeSaveXMLVM.xml();
            xml.root = new ConfirmOrdeSaveXMLVM.root();
            xml.root.stores = new List<ConfirmOrdeSaveXMLVM.store>();

            ConfirmOrdeSaveXMLVM.store store = new ConfirmOrdeSaveXMLVM.store();
            store.carts = new List<ConfirmOrdeSaveXMLVM.cart>();
            //店铺中的商品（子订单详细）
            ConfirmOrdeSaveXMLVM.cart cart = new ConfirmOrdeSaveXMLVM.cart()
            {
                id = model.CartID,
                proname = model.PName,
                picpath = "01220121101008a1011bda5f9d4239a1547ae229fe70d6.jpg",
                count = "1",
                proid = model.PId,
                prokzid = model.ProkzID,
                skukzid = model.SkukzID,
                skuid = model.SkuId
            };
            store.carts.Add(cart);
            store.storename = model.SName;
            store.storetype = "1";
            store.id = model.SID;
            store.couponid = "-1";
            store.expresstype = "1";
            store.fullsendid = "-1";
            store.message = string.Empty;
            xml.root.stores.Add(store);

            Random rand = new Random();
            string pointRD = "0";// rand.Next(100, 500).ToString();

            //买家信息
            xml.root.buyer = new ConfirmOrdeSaveXMLVM.buyer()
            {
                ip = "10.10.10.43",
                areacode = "440102",
                areaname = "东山区",
                citycode = "440100",
                cityname = "广州市",
                consigneename = "test1",
                contactqq = "123456789",
                email = string.Empty,
                exchanges = "0.01",
                id = model.UID,
                mobilephone = "13800138000",
                phone = "0762-8888888-99999",
                points = pointRD,//准备用掉的积分
                postcode = "517100",
                street = "1",
                provincecode = "440000",
                provincename = "广东省",
                platformcouponid = string.Empty,//平台优惠券
                redpacket = "0",
                username = model.UName,
                datasourcestype = 9
            };
            return xml;
        }





    }
}
