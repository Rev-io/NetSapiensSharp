using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class Domain
    {
        private static string OBJECT_NAME = "domain";

        public class Item
        {
            public string domain { get; set; }
            public string territory { get; set; }
            public string dial_match { get; set; }
            public string description { get; set; }
            public string moh { get; set; }
            public string mor { get; set; }
            public string mot { get; set; }
            public string rmoh { get; set; }
            public string rating { get; set; }
            public string resi { get; set; }
            public string mksdir { get; set; }
            public int call_limit { get; set; }
            public int sub_limit { get; set; }
            public int max_call_queue { get; set; }
            public int max_aa { get; set; }
            public int max_conference { get; set; }
            public int max_department { get; set; }
            public int max_user { get; set; }
            public int max_device { get; set; }
            public string time_zone { get; set; }
            public string dial_plan { get; set; }
            public string dial_policy { get; set; }
            public string policies { get; set; }
            public string email_sender { get; set; }
            public string smtp_host { get; set; }
            public string smtp_port { get; set; }
            public string smtp_uid { get; set; }
            public string smtp_pwd { get; set; }
            public string from_user { get; set; }
            public string from_host { get; set; }
            public int active_call { get; set; }
            public int sub_count { get; set; }
        }

        public static IRestResponse<List<Item>> List(Connector connector, string territory = null, string domain = null)
        {
            var request = connector.CreateRequest<List<Item>>($"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(domain), domain);
            var response = connector.Send(request);
            if ((response?.Data?.Count).GetValueOrDefault() == 1)
            {
                if (response.Data.ToArray()[0].domain == null)
                {
                    response.Data = new List<Item>();
                }
            }
            return response;
        }
        public static IRestResponse<Common.Count> Count(Connector connector, string territory = null, string domain = null)
        {
            var request = connector.CreateRequest<Common.Count>($"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(domain), domain);
            return connector.Send(request);
        }
        public static IRestResponse<Common.OK> Delete(Connector connector, string domain)
        {
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddField(nameof(domain), domain);
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
        public static bool Exists(Connector connector, string territory = null, string domain = null)
        {
            var x = List(connector, territory, domain);
            return (x != null && x.Data != null && x.Data.Count > 0);
        }
    }
}
