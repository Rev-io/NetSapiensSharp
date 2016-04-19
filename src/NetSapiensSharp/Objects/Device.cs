using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class Device
    {
        private static string OBJECT_NAME = "device";

        public class Item
        {
            public string aor { get; set; }
            public int line { get; set; }
            public string mode { get; set; }
            public string mac { get; set; }
            public string user_agent { get; set; }
            public string received_from { get; set; }
            public string registration_time { get; set; }
            public string subscriber_name { get; set; }
            public string subscriber_domain { get; set; }
            public string authentication_key { get; set; }
            public string call_processing_rule { get; set; }
            public string registration_expires_time { get; set; }
            public string sub_fullname { get; set; }
            public string sub_scope { get; set; }
            public string sub_login { get; set; }
            public string ndperror { get; set; }
        }

        public static IRestResponse<Common.OK> Create(Connector connector, string domain, string user, string device, string mac, string model)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=create");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(user), user);
            request.AddField(nameof(mac), mac);
            request.AddField(nameof(model), model);
            request.AddField(nameof(device), device);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Delete(Connector connector, string domain, string device)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(device), device);
            return connector.Send(request);
        }

        public static IRestResponse<List<Item>> List(Connector connector, string domain = null, string territory = null, string device = null, string user = null)
        {
            var request = connector.CreateRequest<List<Item>>(@"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(device), device);
            request.AddField(nameof(user), user);
            return connector.Send(request);
        }

        public static IRestResponse<Common.Count> Count(Connector connector, string territory = null, string domain = null)
        {
            var request = connector.CreateRequest<Common.Count>(@"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(domain), domain);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Update(Connector connector, string device, Item item)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=update");
            request.AddField(nameof(device),device);
            request.AddFields(item);
            return connector.Send(request);
        }
    }
}
