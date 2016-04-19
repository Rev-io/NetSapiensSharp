using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class Reseller
    {
        private static string OBJECT_NAME = "reseller";

        public class Item
        {
            public string territory { get; set; }
            public string description { get; set; }
        }

        public static IRestResponse<List<Item>> List(Connector connector, string territory = null)
        {
            var request = connector.CreateRequest<List<Item>>($"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(territory), territory);
            return connector.Send(request);
        }
        public static IRestResponse<Common.Count> Count(Connector connector, string territory = null)
        {
            var request = connector.CreateRequest<Common.Count>($"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(territory), territory);
            return connector.Send(request);
        }
        public static IRestResponse<Common.OK> Delete(Connector connector, string territory)
        {
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddField(nameof(territory), territory);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Create(Connector connector, Item item)
        {
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=create");
            request.AddFields(item);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Update(Connector connector, Item item)
        {
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=update");
            request.AddFields(item);
            return connector.Send(request);
        }
        public static bool Exists(Connector connector, string territory = null)
        {
            var x = List(connector, territory);
            return (x != null && x.Data != null && x.Data.Count > 0);
        }
    }
}
