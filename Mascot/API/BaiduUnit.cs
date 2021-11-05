using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mascot
{
    public class BaiduUnit
    {
		// 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
		// 返回token示例
		public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

		// 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
		private static String clientId = "xkyG01aMsrte23TBvzHZgidq";
		// 百度云中开通对应服务应用的 Secret Key
		private static String clientSecret = "x3sOo8FrG6wAFrTtV2OkKenHc6xixW1y";

		public static String getAccessToken()
		{
			string authHost = "https://aip.baidubce.com/oauth/2.0/token";
			HttpClient client = new HttpClient();
			List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
			paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
			paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
			paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

			HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
			String result = response.Content.ReadAsStringAsync().Result;
			//Console.WriteLine(result);
			string fp = @"../../info.json";
			File.WriteAllText(fp, result, Encoding.UTF8);
			string jsonfile = "../../info.json";//JSON文件路径

			using (StreamReader file = File.OpenText(jsonfile))
			{
				using (JsonTextReader reader = new JsonTextReader(file))
				{
					JObject o = (JObject)JToken.ReadFrom(reader);
					var value = o["access_token"].ToString();
					return value;
				}
			}
			//return result;
		}

		// unit对话接口
		public static async Task<string> unit_utterance(string query)
		{
			return await Task.Run(() =>
			{
				string token = getAccessToken();
				string host = "https://aip.baidubce.com/rpc/2.0/unit/service/v3/chat?access_token=" + token;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
				request.Method = "post";
				request.ContentType = "application/json";
				request.KeepAlive = true;
				string str = $"{{\"version\":\"3.0\",\"service_id\":\"S60417\",\"session_id\":\"\",\"log_id\":\"7758521\",\"request\":{{\"terminal_id\":\"88888\",\"query\":\"{query}\"}}" + "}";
				byte[] buffer = Encoding.UTF8.GetBytes(str);
				request.ContentLength = buffer.Length;
				request.GetRequestStream().Write(buffer, 0, buffer.Length);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				string result = reader.ReadToEnd();
				string fp = @"../../info1.json";
				File.WriteAllText(fp, result, Encoding.UTF8);
				string jsonfile = "../../info1.json";//JSON文件路径

				using (StreamReader file = File.OpenText(jsonfile))
				{
					using (JsonTextReader reader1 = new JsonTextReader(file))
					{
						JObject o = (JObject)JToken.ReadFrom(reader1);
						var value = o["result"]["responses"][0]["actions"][0]["say"].ToString();
						Console.WriteLine(value);
					}
				}
				//Console.WriteLine("对话接口返回:");
				//Console.WriteLine(result);
				return result;
			});
		}
	}
}
