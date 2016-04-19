using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class Connection
    {
        private static string OBJECT_NAME = "connection";

        public class Item
        {
            public string aor { get; set; }
            public string domain { get; set; }
            public string role { get; set; }
            public string termination_match { get; set; }
            public string req_user_trans { get; set; }
            public string req_host_trans { get; set; }
            public string to_user_trans { get; set; }
            public string to_host_trans { get; set; }
            public string from_user_trans { get; set; }
            public string from_host_trans { get; set; }
            public string address { get; set; }
            public string origination_allowed { get; set; }
            public string termination_allowed { get; set; }
            public string nat_wan { get; set; }
            public string authenticate_invite { get; set; }
            public string authentication_alg { get; set; }
            public string auth_user { get; set; }
            public string authentication_realm { get; set; }
            public string authentication_key { get; set; }
            public string registration_required { get; set; }
            public string registration_time { get; set; }
            public string registration_expires { get; set; }
            public string rate { get; set; }
            public string rate_account { get; set; }
            public string rate_max { get; set; }
            public string rate_ratio { get; set; }
            public string rate_margin { get; set; }
            public string max_orig { get; set; }
            public string max_term { get; set; }
            public string max_total { get; set; }
            public string min_orig_prd { get; set; }
            public string min_term_prd { get; set; }
            public string min_dura { get; set; }
            public string nameout_dial_translation { get; set; }
            public string out_dial_delay { get; set; }
            public string out_dial_mode { get; set; }
            public string dial_plan { get; set; }
            public string dial_policy { get; set; }
            public string gmt_offset { get; set; }
            public string time_zone { get; set; }
            public string call_processing_rule { get; set; }
            public string description { get; set; }
            public string count_in { get; set; }
            public string count_out { get; set; }
            public string total_orig { get; set; }
            public string total_term { get; set; }
            public string period_orig { get; set; }
            public string period_term { get; set; }
            public string ts { get; set; }
            public string mac { get; set; }
        }

        public static IRestResponse<List<Item>> List(Connector connector, string domain = null, string territory = null, string aor = null)
        {
            var request = connector.CreateRequest<List<Item>>(@"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(aor), aor);
            return connector.Send(request);
        }

        public static IRestResponse<Common.Count> Count(Connector connector, string territory = null, string domain = null)
        {
            var request = connector.CreateRequest<Common.Count>(@"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(domain), domain);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Delete(Connector connector, string domain, string aor)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(aor), aor);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Create(Connector connector, Item item)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=create");
            request.AddFields(item);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Update(Connector connector, Item item)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=update");
            request.AddFields(item);
            return connector.Send(request);
        }

    }
}
