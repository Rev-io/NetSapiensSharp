using System;
using NetSapiensSharp.Objects;

namespace NetSapiensSharp.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var auth = new Auth();

            using (var c = new Connector(auth.url, auth.clientId, auth.clientSecret, auth.username, auth.password))
            {
                ListResellers(c);
                Console.WriteLine("--------------------------");

                ListDomains(c, "reseller_name");
                Console.WriteLine("--------------------------");

                ListUsers(c, "reseller_name", "domain_name");
                Console.WriteLine("--------------------------");

                ListPhoneNumbers(c, "domain_name", "extension_number");
                Console.WriteLine("--------------------------");
            };

            Console.WriteLine("done");
            Console.ReadLine();
        }

        static void ListResellers(Connector c)
        {
            var x = Reseller.List(c);
            foreach (var item in x.Data)
            {
                Console.WriteLine(item.territory + ":" + item.description);
            }
        }
        static void ListDomains(Connector c, string territory)
        {
            var x = Domain.List(c, territory:territory);
            foreach (var item in x.Data)
            {
                Console.WriteLine(item.domain + ":" + item.description);
            }
        }
        static void ListUsers(Connector c, string territory, string domain)
        {
            var x = User.List(c, territory: territory, domain: domain);
            foreach (var item in x.Data)
            {
                Console.WriteLine(item.domain + ":" + item.user + ":");
            }
        }
        static void ListPhoneNumbers(Connector c, string domain, string user)
        {
            var x = PhoneNumber.ListByUser(c, domain: domain, user: user);
            foreach (var item in x.Data)
            {
                Console.WriteLine(user + ":" + PhoneNumber.UnformatPhoneNumber(item.matchrule));
            }
        }
    }
}
