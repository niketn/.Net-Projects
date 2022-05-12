using AlexaNotify.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlexaNotify.Tests
{
    public class NotifyControllerTest 
    {
        private NotifyController _testClass;
        [SetUp]
        public void Setup()
        {
            _testClass = new NotifyController(Substitute.For<IConfiguration>());
        }

        [Test]
        public async Task CanCallSend_Notification()
        {
            //using var client = new HttpClient();
            Request req = new Request { Ref_id = "TestValue757990735", Message = "TestValue 621271429", user_id = "amzn1.ask.account.AF5I6NYIBTFR5SMLFWTT3INRVL2WFYD4KCKA6ARRNGFEBPF6ZWMRQQBD7ZXQG5UM4KGI5VVX7FE7DVA4DLKDYM6GWHMDU244R2RKB7FMRLCWIVDCSUO4GNZ67M2YST7HPM6BDBP5C6XXZBWM7MUWIFAL4T4BY2ONYJCQNZMNGEISW7PRU3ATQQVXXFE2KH36YJR3IQ3PS7N4WPQ" };
            //string url = "https://localhost:44311/api/Notify/SendNotification?"+req;//HttpResponseMessage result = await _testClass.Send_Notification(req);
            //HttpContent e = null;
            var result = await _testClass.Send_Notification(req);
            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            //Assert
            //Assert.IsNotNull(result);
            Console.WriteLine(result);
            //Assert.IsNotNull(result);
            Assert.Pass();
            //Assert.AreEqual(result, typeof(OkResult));

        }
        [Test]
        public void CannotCallSend_NotificationWithNullReq()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.Send_Notification(default(Request)));
        }
        [Test]
        public void CanCallGenerate_Message()
        {
            var Hotel_name = "TestValue1667831851";
            var Hotel_location = "TestValue589245790";
            var check_in_date = "TestValue751348026";
            var optimised_rate = "TestValue1773607939";
            var saving_rate = "TestValue176341473";
            var Booking_type = "#";
            var result = _testClass.Generate_Message(Hotel_name, Hotel_location, check_in_date, optimised_rate, saving_rate, Booking_type);
            if (result == $"A better rate has been found for your hotel reservation at {Hotel_name}, {Hotel_location} for {check_in_date}. The new rate is {optimised_rate} and offers a saving of {saving_rate} from your existing booking. To book the reduced rate, please check your emails for notification.")
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

       

    }

}