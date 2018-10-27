using UnityEngine;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;

public class HttpQSCollection : NameValueCollection
{
    public override string ToString()
    {
        int count = Count;
        if (count == 0)
            return "";
        StringBuilder sb = new StringBuilder();
        string[] keys = AllKeys;
        for (int i = 0; i < count; i++)
        {
            // Query string # Tracking - Wikipedia https://en.wikipedia.org/wiki/Query_string
            if (string.IsNullOrEmpty(this[keys[i]]))
            {
                sb.AppendFormat("{0}", keys[i]);
            }
            else
            {
                sb.AppendFormat("{0}={1}&", keys[i], this[keys[i]]);
            }
        }
        if (sb.Length > 0)
            sb.Length--;
        return sb.ToString();
    }
}

public class HttpUtility
{
    public static HttpQSCollection ParseQueryString(string s)
    {
        HttpQSCollection nvc = new HttpQSCollection();
        // remove anything other than query string from url
        if (s.Contains("?"))
        {
            s = s.Substring(s.IndexOf('?') + 1);
        }
        foreach (string vp in Regex.Split(s, "&"))
        {
            string[] singlePair = Regex.Split(vp, "=");
            if (singlePair.Length == 2)
            {
                nvc.Add(singlePair[0], singlePair[1]);
            }
            else
            {
                // only one key with no value specified in query string
                nvc.Add(singlePair[0], string.Empty);
            }
        }
        return nvc;
    }
}
