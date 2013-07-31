using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTest.App
{
    public class CartModel
    {
        private string cartId;
        private string uId;
        private string uName;
        private string sId;
        private string sName;
        private string pId;
        private string pName;
        private string skuid;
        private string prokzid;
        private string skukzid;

        public string CartID
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = value;
            }
        }
        public string UID
        {
            get
            {
                return this.uId;
            }
            set
            {
                this.uId = value;
            }
        }
        public string UName
        {
            get
            {
                return this.uName;
            }
            set
            {
                this.uName = value;
            }
        }
        public string SID
        {
            get
            {
                return this.sId;
            }
            set
            {
                this.sId = value;
            }
        }
        public string SName
        {
            get
            {
                return this.sName;
            }
            set
            {
                this.sName = value;
            }
        }
        public string PId
        {
            get
            {
                return this.pId;
            }
            set
            {
                this.pId = value;
            }
        }
        public string PName
        {
            get
            {
                return this.pName;
            }
            set
            {
                this.pName = value;
            }
        }
        public string SkuId
        {
            get
            {
                return this.skuid;
            }
            set
            {
                this.skuid = value;
            }
        }
        public string ProkzID
        {
            get
            {
                return this.prokzid;
            }
            set
            {
                this.prokzid = value;
            }
        }
        public string SkukzID
        {
            get
            {
                return this.skukzid;
            }
            set
            {
                this.skukzid = value;
            }
        }
    }
}
