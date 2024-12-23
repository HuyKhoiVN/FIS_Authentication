using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Utils
{
    public class FISAuthenticationResponse : IActionResult
    {
        public static string STATUS_SUCCESS = "200";
        public string status { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public IList<Object> data { get; set; }
        public object resources { get; set; }




        public FISAuthenticationResponse(string status, string message, IList<Object> data)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }
        public FISAuthenticationResponse(string status, string message, object data)
        {
            this.status = status;
            this.message = message;
            this.resources = data;
        }
        public FISAuthenticationResponse(string status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public FISAuthenticationResponse()
        {
        }

        public static FISAuthenticationResponse Success<T>(T? data, string? message = "SUCCESS") where T : class
        {
            return new FISAuthenticationResponse()
            {
                status = "200",
                message = message,
                resources = data
            };
        }

        public static FISAuthenticationResponse Failed(string status, string message, object data)
        {
            return new FISAuthenticationResponse(status, message, new List<object> { data });
        }

        public static FISAuthenticationResponse SUCCESS(IList<Object> data)
        {
            return new FISAuthenticationResponse("200", "SUCCESS", data);
        }
        public static FISAuthenticationResponse ISEXISTUSERNAME()
        {
            return new FISAuthenticationResponse("205", "IS_EXIST_USERNAME");
        }
        public static FISAuthenticationResponse ISEXISTIDSTAFF()
        {
            return new FISAuthenticationResponse("206", "IS_EXIST_IDSTAFF");
        }
        public static FISAuthenticationResponse ISEXISTPHONENUMBER()
        {
            return new FISAuthenticationResponse("207", "IS_EXIST_PHONE_NUMBER");
        }
        public static FISAuthenticationResponse SUCCESS(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("200", "SUCCESS", returnData);
        }
        public static FISAuthenticationResponse BAD_REQUEST(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("400", "BAD_REQUEST", returnData);
        }

        public static FISAuthenticationResponse BAD_REQUEST()
        {
            List<Object> returnData = new List<Object>();
            var obj = new { Code = 400, Message = "BAD_REQUEST" };
            returnData.Add(obj);
            return new FISAuthenticationResponse("400", "BAD_REQUEST", returnData);
        }
        public static FISAuthenticationResponse FAIL_BOOKING_TIME()
        {
            return new FISAuthenticationResponse("207", "SUCCESS");
        }
        public static FISAuthenticationResponse SUCCESS()
        {
            return new FISAuthenticationResponse("200", "SUCCESS");
        }
        //trả về SUCCESSNOTBIDDING trong hoàn tiền đặt cọc
        //public static FISAuthenticationResponse SUCCESSNOTBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new FISAuthenticationResponse("205", "SUCCESSNOTBIDDING", returnData);
        //}
        public static FISAuthenticationResponse SUCCESSNOTBIDDING(IList<Object> data)
        {
            return new FISAuthenticationResponse("205", "SUCCESSNOTBIDDING", data);
        }
        //trả về SUCCESSHAVEBIDDING trong hoàn tiền đặt cọc
        //public static FISAuthenticationResponse SUCCESSHAVEBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new FISAuthenticationResponse("206", "SUCCESSHAVEBIDDING", returnData);
        //}
        public static FISAuthenticationResponse SUCCESSHAVEBIDDING(IList<Object> data)
        {
            return new FISAuthenticationResponse("206", "SUCCESSHAVEBIDDING", data);
        }

        public static FISAuthenticationResponse CREATED(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("201", "CREATED", returnData);
        }
        public static FISAuthenticationResponse CREATED(List<Object> data)
        {
            List<Object> returnData = new List<Object>();
            returnData = data;
            return new FISAuthenticationResponse("201", "CREATED", returnData);
        }
        public static FISAuthenticationResponse Faild()
        {
            return new FISAuthenticationResponse("099", "FAILD");
        }
        public static FISAuthenticationResponse UNAUTHORIZED()
        {
            return new FISAuthenticationResponse("401", "UNAUTHORIZED");
        }
        public static FISAuthenticationResponse BiddingFaildEnded(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("096", "BIDDINGFAILD", returnData);
        }

        public static FISAuthenticationResponse EmailExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("202", "EMAILEXIST", data);
        }
        public static FISAuthenticationResponse EmailNotValid(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("204", "EMAILNOTVALID");
        }
        public static FISAuthenticationResponse UsernameExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("203", "USENAMEEXIST");
        }
        public static FISAuthenticationResponse IdCardNumberExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("205", "IDCARNUMBEREXIST");
        }
        public static FISAuthenticationResponse BiddingRequestExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("203", "BIDDINGREQUESTEXIST", returnData);
        }
        public static FISAuthenticationResponse PhoneExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("204", "PHONEEXIST");
        }
        public static FISAuthenticationResponse CompanyIdExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("206", "COMPANYIDEXIST");
        }
        public static FISAuthenticationResponse ItemExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("203", "ITEMEXIST", returnData);
        }
        public static FISAuthenticationResponse NotFoundBiddingMax()
        {
            return new FISAuthenticationResponse("999", "FAILD");
        }
        public static FISAuthenticationResponse PostNameExist()
        {
            return new FISAuthenticationResponse("203", "POSTNAMEEXIST");
        }
        public static FISAuthenticationResponse NotFoundBiddingSecond()
        {
            return new FISAuthenticationResponse("998", "FAILD");
        }
        public static FISAuthenticationResponse PasswordExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("202", "PASSWORDEXIST", returnData);
        }
        public static FISAuthenticationResponse PasswordIsNotFormat(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new FISAuthenticationResponse("205", "PASSWORDISNOTINCORRECTFORMAT", returnData);
        }

        public static FISAuthenticationResponse OTP_REQUIRED(IList<Object> data)
        {
            return new FISAuthenticationResponse("200", "OTP_REQUIRED", data);
        }
        public static FISAuthenticationResponse OTP_OVER_LIMIT(IList<Object> data)
        {
            return new FISAuthenticationResponse("099", "OTP_OVER_LIMIT", data);
        }
        public static FISAuthenticationResponse OTP_INVALID_DATA(IList<Object> data)
        {
            return new FISAuthenticationResponse("098", "INVALID_DATA", data);
        }
        public static FISAuthenticationResponse OTP_EXIST()
        {
            return new FISAuthenticationResponse("204", "OTP_EXIST");
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
