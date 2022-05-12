using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlexaNotify.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlexaNotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        string Better_rates_Msg;
        private IConfiguration configuration;
        public NotifyController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        
        [HttpPost]
        [Route("SendNotification")]
        public async Task<HttpResponseMessage> Send_Notification([FromBody] Request req)
        {
            try
            {
                //To initialize time as per sending notification currently
                var time_stamp = DateTime.UtcNow.ToString("s") + "Z";
                var expiry_time = DateTime.UtcNow.AddHours(23).ToString("s") + "Z";

                StreamReader Req = new StreamReader("./JsonConfig/AuthTokenRequest.json"); //will read the Requestbody for access token
                string AuthReq = Req.ReadToEnd();

                var data = new StringContent(AuthReq, Encoding.UTF8, "application/json");

                var url = configuration.GetValue<string>("TokenUrl");// will read the URL for token request call
                using var client = new HttpClient();

                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;

                string acc_t = JObject.Parse(result)["access_token"].ToString();
                string Access_token = "bearer " + acc_t;
                //return Access_token;
                using var client2 = new HttpClient();

                //will read the Requestbody for proactive API call
                StreamReader r = new StreamReader("./JsonConfig/tokenRequest.json");
                string jsonString = r.ReadToEnd();

                //updating values based on post request of the API
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);
                jsonObject.timestamp = time_stamp;
                jsonObject.expiryTime = expiry_time;
                jsonObject.referenceId = req.Ref_id;
                jsonObject.localizedAttributes[0].contentName = req.Message;
                jsonObject.relevantAudience.payload.user = req.user_id;
                string modifiedJsonString = JsonConvert.SerializeObject(jsonObject);

                var content = new StringContent(modifiedJsonString, Encoding.UTF8, "application/json");
                client2.DefaultRequestHeaders.Add("Authorization", Access_token);


                var url2 = configuration.GetValue<string>("ProactiveApiUrl");// will read the URL for token request call
                var response2 = await client2.PostAsync(url2, content);
                return response2;
               
                //return result2;
            }
            catch(Exception e)
            {
                return null;
            }


        }
        
        [HttpGet]
        [Route("Generate_Message")]
        public string Generate_Message(string Hotel_name, string Hotel_location, string check_in_date, string optimised_rate, string saving_rate, string Booking_type)
        {

            try 
            {
               

                if (Booking_type == "#" && Hotel_name!=null)
                {
                    Better_rates_Msg = $"A better rate has been found for your hotel reservation at {Hotel_name}, {Hotel_location} for {check_in_date}. The new rate is {optimised_rate} and offers a saving of {saving_rate} from your existing booking. To book the reduced rate, please check your emails for notification.";
                    return Better_rates_Msg;
                }
                else
                {
                    Better_rates_Msg = $"A room {Booking_type} has been found for your hotel reservation at at {Hotel_name}, {Hotel_location} for {check_in_date}.  The upgrade is included in your existing rate and you now have complimentary access to the Business Lounge for the duration of your stay.";
                }
            
                return Better_rates_Msg;
            }
            catch(Exception e)
            {
                //throw new System.ArgumentNullException();
                return e.Message;
            }
        }


        [HttpGet]
        [Route("CallNotify")]
        public string CallNotify(OptimisedRateObject R)
        {
            //string Hotel_name = null, Hotel_location = null, check_in_date = null, optimised_rate = null, saving_rate = null, Booking_type = null;
            //Request R = new Request();
            //R.Ref_id = "#";
            //R.Message = Generate_Message(Hotel_name, Hotel_location, check_in_date, optimised_rate, saving_rate, Booking_type);
            //R.user_id = "#";
            //var Res = Send_Notification(R);
            return R.ToString();
        }

    }
}
