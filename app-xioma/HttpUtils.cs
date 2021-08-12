using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace app_xioma
{
    public class HttpUtils
    {
        //Execute http request (type Get) to the server and receiving a response from the server
        public static void SendHttpGetRequest(string url, Action<string> whatToDo)
        {
            Thread thread = new Thread(() =>
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream stream = response.GetResponseStream())
                    {
                        string responseText = (new StreamReader(stream)).ReadToEnd();
                        whatToDo(responseText);
                    }
                }

            });
            thread.Start();
        }


        //Execute http request (type Post) to the server and receiving a response from the server
        public static void SendHttpPostRequest(string url, Action<string> whatToDo, string body)
        {
            Thread thread = new Thread(() =>
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bodyBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(body);
                request.ContentLength = bodyBytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                }
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream stream = response.GetResponseStream())
                    {
                        string responseText = (new StreamReader(stream)).ReadToEnd();
                        whatToDo(responseText);
                    }
                }

            });
            thread.Start();
        }

        public static void SendHttpGetRequest(string url, Action<string> whatToDo, Form form)
        {
            SendHttpGetRequest(url, (responseString) =>
            {
                Action action = new Action(() =>
                {
                    whatToDo(responseString);
                });
                form.Invoke(action);
            });
        }

    }
}
