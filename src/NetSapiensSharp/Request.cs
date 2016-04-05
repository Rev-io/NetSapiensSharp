using RestSharp;

namespace NetSapiensSharp
{
    public class Request<T> where T : class, new()
    {
        public IRestClient _Client { get; set; }
        public IRestRequest _Request { get; set; }

        public void AddFields<Tmodel>(Tmodel item)
        {
            if (item != null)
            {
                foreach (var p in item.GetType().GetProperties())
                {
                    var value = p.GetValue(item, null);
                    if (value != null)
                    {
                        if (value.GetType() == typeof(bool?))
                        {
                            AddField(p.Name, (bool?)value);
                        }
                        else
                        {
                            AddField(p.Name, value.ToString());
                        }
                    }
                }
            }
        }

        public void AddField(string name, bool? value)
        {
            AddField(name, value.Value ? "yes" : "no");
        }

        public void AddField(string name, string value)
        {
            if (value != null && value != "")
            {
                _Request.AddQueryParameter(name, value);
            }
        }
    }
}
