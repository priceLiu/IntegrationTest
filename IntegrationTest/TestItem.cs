using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace IntegrationTest
{
    public abstract class TestItem
    {

        public Exception LastError
        {
            get;
            set;
        }

        public double UserTime
        {
            get;
            set;
        }

        protected abstract void OnExecute();      

        public void Execute()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            try
            {
                OnExecute();
            }
            catch(Exception e)
            {
                LastError = e;
            }
            sw.Stop();
            UserTime = sw.Elapsed.TotalMilliseconds;

        }

        internal class DataBinder
        {

            private List<PropertyBinder> mProperties = new List<PropertyBinder>();

            public DataBinder(Type type, ParamCollection properties)
            {
                foreach (Param item in properties)
                {
                    try
                    {
                        PropertyInfo pi = type.GetProperty(item.Name, BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            PropertyBinder pb = new PropertyBinder();
                            pb.Property = pi;
                            pb.Value = item.Value;
                            mProperties.Add(pb);
                        }
                    }
                    catch
                    {
                    }

                }
            }
            public void Bind(object data)
            {
                foreach (PropertyBinder item in mProperties)
                {
                    try
                    {
                        item.Property.SetValue(data, Convert.ChangeType(item.Value, item.Property.PropertyType), null);
                    }
                    catch{
                    }
                }
            }
            public class PropertyBinder
            {
                public PropertyInfo Property;
                public string Value;
            }
        }
    }
}
