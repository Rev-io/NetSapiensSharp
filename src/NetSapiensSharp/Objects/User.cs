using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class User
    {
        private static string OBJECT_NAME = "subscriber";

        public class Item
        {
            public string user { get; set; }
            public string domain { get; set; }
            public string subscriber_login { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string pwd { get; set; }
            public string pwd_hash { get; set; }
            public string group { get; set; }
            public string dir { get; set; }
            public string email { get; set; }
            public string presence { get; set; }
            public string message { get; set; }
            public string vmail_provisioned { get; set; }
            public string vmail_enabled { get; set; }
            public string vmail_greeting { get; set; }
            public string vmail_notify { get; set; }
            public string vmail_annc_time { get; set; }
            public string vmail_annc_cid { get; set; }
            public string vmail_sort_lifo { get; set; }
            public string vmail_transcribe { get; set; }
            public string data_limit { get; set; }
            public string call_limit { get; set; }
            public string dial_plan { get; set; }
            public string dial_policy { get; set; }
            public string area_code { get; set; }
            public string callid_name { get; set; }
            public string callid_nmbr { get; set; }
            public string callid_emgr { get; set; }
            public string no_answer_timeout { get; set; }
            public string time_zone { get; set; }
            public string dir_anc { get; set; }
            public string dir_List { get; set; }
            public string date_created { get; set; }
            public string scope { get; set; }
            public string rej_anony { get; set; }
            public string directory_order { get; set; }
            public string screen { get; set; }
            public string srv_code { get; set; }
            public string ntfy_missed_call { get; set; }
            public string ntfy_data_limit { get; set; }
            public string language { get; set; }
            public string gauSession { get; set; }
            public string last_update { get; set; }
        }

        public static IRestResponse<List<Item>> List(Connector connector, string uid = null, string login = null, string domain = null, string territory = null, string user = null, string limit = null, string name = null, string first_name = null, string last_name = null, string group = null, string fields = null, string email = null, string dir = null, string filter_users = null, string directory_match = null, string owner = null, string scope = null, string start = null, string sort = null)
        {
            var request = connector.CreateRequest<List<Item>>(@"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(uid), uid);
            request.AddField(nameof(login), login);
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(user), user);
            request.AddField(nameof(limit), limit);
            request.AddField(nameof(name), name);
            request.AddField(nameof(first_name), first_name);
            request.AddField(nameof(last_name), last_name);
            request.AddField(nameof(group), group);
            request.AddField(nameof(fields), fields);
            request.AddField(nameof(email), email);
            request.AddField(nameof(filter_users), filter_users);
            request.AddField(nameof(directory_match), directory_match);
            request.AddField(nameof(owner), owner);
            request.AddField(nameof(scope), scope);
            request.AddField(nameof(start), start);
            request.AddField(nameof(sort), sort);
            return connector.Send(request);
        }

        public static IRestResponse<Common.Count> Count(Connector connector, string territory = null, string domain = null)
        {
            var request = connector.CreateRequest<Common.Count>(@"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(territory), territory);
            request.AddField(nameof(domain), domain);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Delete(Connector connector, string domain, string user)
        {
            var request = connector.CreateRequest<Common.OK>(@"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddField(nameof(domain), domain);
            request.AddField(nameof(user), user);
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

        public static Item Lookup(Connector connector, string uid = null, string login = null, string domain = null, string territory = null, string user = null)
        {
            var x = List(connector, uid, login, domain, territory, user);
            return (x != null && x.Data != null && x.Data.Count > 0 ? x.Data.ToArray()[0] : null);
        }
    }
}
