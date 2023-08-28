using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000419 RID: 1049
public static class MapModUtil {
	// Token: 0x06001670 RID: 5744
	public static string GetHash(string data) {
		if (data == "") {
			return "NONE";
		}
		string str;
		using (SHA256Managed sha256Managed = new SHA256Managed()) {
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			str = BitConverter.ToString(sha256Managed.ComputeHash(bytes)).Replace("-", string.Empty).Substring(0, 6);
		}
		return str;
	}
	
	public static string GetHash(byte[] data) {
		string str;
		using (SHA256Managed sha256Managed = new SHA256Managed()) {
			str = BitConverter.ToString(sha256Managed.ComputeHash(data)).Replace("-", string.Empty).Substring(0, 6);
		}
		return str;
	}

	// Token: 0x06001671 RID: 5745
	public static string DownloadWebPage(string url, int timeoutMilliseconds = 5000) {
		WebRequest webRequest = WebRequest.Create(url);
		webRequest.Timeout = timeoutMilliseconds;
		string result;
		using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream())) {
			result = sr.ReadToEnd();
		}
		return result;
	}
}
