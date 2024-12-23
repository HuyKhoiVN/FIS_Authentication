using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace appData.Utils
{
    public class SecurityUtil
    {
        // phân quyền api
        public static bool AuthorizeRequestSimple(string RequestedURL, int AccountId, int RoleId)
        {
            RequestedURL = RequestedURL.ToLower();
            bool isAuthorizingNeeded = false;
            bool result = true;

            string entityRequested = "";
            string resourceRequested = "";
            string actionRequested = "";

            try
            {
                if (RequestedURL.IndexOf("/") == 0) RequestedURL = RequestedURL.Substring(1);
                entityRequested = RequestedURL.Substring(0, RequestedURL.IndexOf("/"));
                string temp = RequestedURL.Substring(RequestedURL.IndexOf("/") + 1);
                resourceRequested = temp.Substring(0, temp.IndexOf("/"));
                actionRequested = temp.Substring(temp.IndexOf("/") + 1);
            }
            catch (Exception) { }

            //Nếu tài nguyên được yêu cầu là API --> cần xác thực
            if (resourceRequested == "api")
            {
                isAuthorizingNeeded = true;
            }

            //Nếu không cần kiểm tra thì ngừng
            if (isAuthorizingNeeded == false) return result;

            //List
            if (actionRequested == "list")
            {
                //Cho phép truy cập trừ trang account
                if (entityRequested == "account")
                {
                    //if(RoleId == SystemConstant.ROLE_UNVERIFIED_USER || RoleId == SystemConstant.ROLE_VERIFIED_USER)
                    //{
                    //    result = false;
                    //}
                }
            }

            //Add
            if (actionRequested == "add")
            {
                if (AccountId == 0)
                {
                    //result = false;
                    result = true;

                    //Không cho phép trừ: (cần hoàn thiện sau)
                    //activitylog
                    //contact
                    //subscribe
                    if (
                        entityRequested == "activitylog" ||
                        entityRequested == "contact" ||
                        entityRequested == "subscribe"
                    ) result = true;
                }
            }

            //Update
            if (actionRequested == "update")
            {
                if (AccountId == 0)
                {
                    result = false;
                }
            }

            //Delete
            if (actionRequested == "delete")
            {
                if (AccountId == 0)
                {
                    result = false;
                }
            }

            return result;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
