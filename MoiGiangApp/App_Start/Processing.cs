using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MoiGiangApp.App_Start
{
	public class Processing
	{
		public static string ConvertToUnSign3(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace(":", "").Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "-").Replace("&", "").Replace("%", "").Replace(@"\", "-").Replace("–", "-").Replace("---", "-").Replace("--", "-").Replace(".", "").Replace("+", "").Replace("\"", "").Replace("/", "").Replace("?", "").Replace(",", "").Replace("[", "").Replace("]", "").Replace("!", "").ToLower();
		}
		public static string UrlImages(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace(":", "").Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "-").Replace("&", "").Replace("%", "").Replace(@"\", "-").Replace("---", "-").Replace("--", "-").Replace("+", "").Replace("\"", "").Replace("/", "").Replace("?", "").Replace(",", "").Replace("[", "").Replace("]", "").Replace("!", "").ToLower();

		}
		public static string SinhMaCode(string s1, string s2)
		{
			string s3 = "";
			string[] arr = s1.Split(' ');
			s3 = ConvertToUnSign3(arr[arr.Length - 1]).ToUpper();
			for (int i = 0; i <= arr.Length - 2; i++)
			{
				s3 += arr[i].Substring(0, 1).ToUpper();
			}
			s3 += s2.Replace("/", "");
			return s3;
		}

		//MA HOA
		public static string EncodePasswordToBase64(string password)
		{
			try
			{
				byte[] encData_byte = new byte[password.Length];
				encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
				string encodedData = Convert.ToBase64String(encData_byte);
				return encodedData;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in base64Encode" + ex.Message);
			}
		}
		//END MA HOA
	}
}