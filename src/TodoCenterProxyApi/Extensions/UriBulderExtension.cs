using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TodoCenterProxyApi.Extensions
{
    public static class UriBulderExtension
    {
        public static Uri BuildQueryString(this UriBuilder builder, NameValueCollection query)
        {
            var collection = HttpUtility.ParseQueryString(string.Empty);
            foreach (var key in query.Cast<string>().Where(key => !string.IsNullOrEmpty(query[key])))
            {
                collection[key] = query[key];
            }
            builder.Query = collection.ToString();
            return builder.Uri;
        }
    }
}
