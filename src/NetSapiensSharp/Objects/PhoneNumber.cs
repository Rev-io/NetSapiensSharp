using System.Collections.Generic;
using RestSharp;

namespace NetSapiensSharp.Objects
{
    public class PhoneNumber
    {
        private static string OBJECT_NAME = "phonenumber";

        public class Item
        {
            public string dest_domain { get; set; }
            public string matchrule { get; set; }
            public string dialplan { get; set; }
            public string responder { get; set; }
            public string domain { get; set; }
            public string domain_description { get; set; }
            public string domain_owner { get; set; }
            public string dow { get; set; }
            public string from_scheme { get; set; }
            public string match_from { get; set; }
            public string parameter { get; set; }
            public string plan_description { get; set; }
            public string to_host { get; set; }
            public string to_scheme { get; set; }
            public string to_user { get; set; }
            public string tod_from { get; set; }
            public string tod_to { get; set; }
            public string valid_from { get; set; }
            public string valid_to { get; set; }

            public List<string> from_host { get; set; }
            public List<string> from_name { get; set; }
            public List<string> from_user { get; set; }

            public bool IsAssignedToUser()
            {
                return to_user != null && to_user != "[*]";
            }
        }

        public static IRestResponse<List<Item>> List(Connector connector, string dialplan = null, string dest_domain = null, string matchrule = null, string matchrule_LIKE = null, string to_user = null)
        {
            if (dialplan == null)
            {
                dialplan = "DID Table";
            }
            var request = connector.CreateRequest<List<Item>>($"/?format=json&object={OBJECT_NAME}&action=read");
            request.AddField(nameof(dialplan), dialplan);
            request.AddField(nameof(dest_domain), dest_domain);
            request.AddField(nameof(matchrule), matchrule);
            request.AddField(nameof(matchrule_LIKE), matchrule_LIKE);
            request.AddField(nameof(to_user), to_user);
            return connector.Send(request);
        }

        public static IRestResponse<Common.Count> Count(Connector connector, string dialplan = null, string dest_domain = null, string matchrule = null, string matchrule_LIKE = null)
        {
            if (dialplan == null)
            {
                dialplan = "DID Table";
            }
            var request = connector.CreateRequest<Common.Count>($"/?format=json&object={OBJECT_NAME}&action=count");
            request.AddField(nameof(dialplan), dialplan);
            request.AddField(nameof(dest_domain), dest_domain);
            request.AddField(nameof(matchrule), matchrule);
            request.AddField(nameof(matchrule_LIKE), matchrule_LIKE);
            return connector.Send(request);
        }

        public static IRestResponse<Common.OK> Delete(Connector connector, Item item)
        {
            if (item.dialplan == null)
            {
                item.dialplan = "DID Table";
            }
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=delete");
            request.AddFields(item);
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
            if (item.dialplan == null)
            {
                item.dialplan = "DID Table";
            }
            var request = connector.CreateRequest<Common.OK>($"/?format=json&object={OBJECT_NAME}&action=update");
            request.AddFields(item);
            return connector.Send(request);
        }

        public static Item Lookup(Connector connector, string dialplan = null, string dest_domain = null, string matchrule = null)
        {
            if (dialplan == null)
            {
                dialplan = "DID Table";
            }
            var x = List(connector, dialplan, dest_domain, matchrule);
            return (x != null && x.Data != null && x.Data.Count > 0 ? x.Data.ToArray()[0] : null);
        }

        public static IRestResponse<Common.OK> AssignToUser(Connector connector, string domain, string did, string user)
        {
            var phonenumber = new Item()
            {
                dest_domain = domain,
                dialplan = "DID Table",
                matchrule = FormatPhoneNumber(did),
                responder = "sip:start@to-user-resi",
                to_host = domain,
                to_user = user
            };
            return Update(connector, phonenumber);
        }

        public static IRestResponse<Common.OK> UnassignFromUser(Connector connector, string domain, string did)
        {
            var phonenumber = new Item()
            {
                dest_domain = domain,
                dialplan = "DID Table",
                matchrule = FormatPhoneNumber(did),
                responder = "AvailableDID",
                to_host = domain,
                to_user = "[*]"
            };
            return Update(connector, phonenumber);
        }

        public static IRestResponse<Common.OK> CreateForDomain(Connector connector, string domain, string did)
        {
            var phonenumber = new Item()
            {
                dest_domain = domain,
                dialplan = "DID Table",
                matchrule = FormatPhoneNumber(did),
                responder = "AvailableDID",
                to_host = domain,
                to_user = "[*]"
            };
            return Create(connector, phonenumber);
        }

        public static IRestResponse<List<Item>> ListByUser(Connector connector, string domain, string user)
        {
            return List(connector, dest_domain: domain, to_user: user);
        }

        public static string FormatPhoneNumber(string number)
        {
            return (number != null && number != "") ? number.StartsWith("sip") ? number : $"sip:1{number.TrimStart('1')}@*" : number;
        }

        public static string UnformatPhoneNumber(string number)
        {
            return number.StartsWith("sip") ? number.Substring(5, 10) : number;
        }
    }
}
