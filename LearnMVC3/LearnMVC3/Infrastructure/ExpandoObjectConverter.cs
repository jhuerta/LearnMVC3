using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Web.Script.Serialization;

namespace LearnMVC3.Infrastructure
{
    // The purpose of this is to serialize properly the dates back to JSON :0000000000
    public class  ExpandoObjectConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(ExpandoObject) })); }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            ExpandoObject expando = (ExpandoObject)obj;

            if (expando != null)
            {
                // Create the representation.
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> item in expando)
                {
                    var value = item.Value ?? "";
                    if (value.GetType() == typeof(DateTime))

                        result.Add(item.Key, ((DateTime)value).ToShortDateString());
                    else
                        result.Add(item.Key, value.ToString());
                }
                return result;
            }
            return new Dictionary<string, object>();
        }
        public override dynamic Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            dynamic result = new ExpandoObject();

            if (dictionary != null)
            {
                // Create the representation.
                var dc = (IDictionary<string, object>)result;
                foreach (KeyValuePair<string, object> item in dictionary)
                {
                    var value = item.Value ?? "";
                    dc.Add(item.Key, value);
                }
            }
            return result;
        }
    }
}