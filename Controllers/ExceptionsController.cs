using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace csv_uploader.Controllers
{
    [Route("api/[controller]")]
    public class ExceptionsController : Controller
    {

        /// <summary>
        /// api/exceptions/badrequest
        /// </summary>
        /// <returns></returns>
        [HttpGet("badrequest")]
        public IActionResult TestingExceptionsBadRequest()
        {
            return BadRequest("sample bad request message");
        }

        /// <summary>
        /// api/exceptions/badrequestException/{dataInt}
        /// </summary>
        /// <returns></returns>
        [HttpPost("badrequestException/{dataInt}")]
        public IActionResult TestingExceptionsBadRequest(int dataInt)
        {
            try
            {
                var divideByZero = 123 / dataInt;
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("webrequestTest")]
        public IActionResult TestingWebRequest()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:63046/api/exceptions/badrequestException/0");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json; charset=utf-8";

            try
            {
                using (var respone = httpWebRequest.GetResponse())
                {
                    return Ok();
                }
            }
            catch (WebException webEx)
            {
                using (WebResponse response = webEx.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                        return BadRequest(text);
                    }
                }
            }
        }


        [HttpGet("testData")]
        public IActionResult GetTestData()
        {
            return Ok(new { sampleData = "blah2xblah2x" });
        }

        /// <summary>
        /// get api/asyncTest
        /// </summary>
        /// <returns></returns>
        [HttpGet("asyncTest")]
        public IActionResult TestingAsyncOperations()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:63046/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("api/exceptions/testData");
            var taskResult = Task.Run(async () => await response).Result;
            //var responseResult = response.Result;
            if (taskResult.IsSuccessStatusCode)
            {
                var taskReadAsStringAsync = taskResult.Content.ReadAsStringAsync();
                var resultOfTask = Task.Run(async () => await taskReadAsStringAsync).Result;
                var result = JsonConvert.DeserializeObject(resultOfTask);
                return Ok(result);
            }
            return Ok();

        }

    }
}
