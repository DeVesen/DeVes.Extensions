using System.Linq;
using RestSharp;

namespace DeVes.Extensions.Helpers
{
    public class UriHelper
    {
        public string Uri { get; set; }
        public UriVariables Variables { get; }


        public UriHelper(string uri)
        {
            Uri = uri;
            Variables = new UriVariables();
        }


        public string GetFinal()
        {
            return ToString();
        }

        public RestRequest BuildGetRequest()
        {
            var _request = GetFinal();
            return new RestRequest(_request, Method.GET);
        }
        public RestRequest BuildPostRequest()
        {
            return new RestRequest(GetFinal(), Method.POST);
        }
        public RestRequest BuildPutRequest()
        {
            return new RestRequest(GetFinal(), Method.PUT);
        }
        public RestRequest BuildDeleteRequest()
        {
            return new RestRequest(GetFinal(), Method.DELETE);
        }


        public override string ToString()
        {
            var _variables = !Variables.Any() ? string.Empty : Variables.GetFinal();

            return Uri + _variables;
        }

        public static explicit operator string(UriHelper value)
        {
            return value.GetFinal();
        }
    }
}
