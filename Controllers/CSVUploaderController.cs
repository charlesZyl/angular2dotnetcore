using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace csv_uploader.Controllers
{
    [Route("api/[controller]")]
    public class CSVUploaderController : Controller
    {
        /// <summary>
        /// api/csvUploader
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("it works!");
        }


        [HttpPost("uploadCsv")]
        public IActionResult UploadCSV()
        {
            var csvFile = Request.Form.Files[0];

            var emailList = new List<string>();

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    emailList.AddRange(values);
                }
            }

            return Ok(emailList);
        }

        [HttpGet("account-data.csv")]
        [Produces("text/csv")]
        public IActionResult GetDataAsCsv()
        {
            return Ok(GetAccountInfo());
        }

        private List<AccountInfo> GetAccountInfo()
        {
            var accountInfoList = new List<AccountInfo>();
            Enumerable.Range(1, 10).ToList().ForEach(id =>
                accountInfoList.Add(new AccountInfo
                {
                    UserId = id,
                    Username = $"user-{id}",
                    Email = $"user-{id}@someemail.com"
                }));
            return accountInfoList;
        }


        [HttpGet("download")]
        public FileResult DownloadCSV()
        {
            var accountInfoList = new List<AccountInfo>();
            Enumerable.Range(1, 10).ToList().ForEach(id =>
                accountInfoList.Add(new AccountInfo
                {
                    UserId = id,
                    Username = $"user-{id}",
                    Email = $"user-{id}@someemail.com"
                }));

            // Not working
            var binaryFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binaryFormatter.Serialize(mStream, accountInfoList);


            //var csvFile = File(mStream, "text/csv", "account-info-list.csv");
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(mStream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "account-info.csv"
            };

            return File(mStream, "text/csv", "account-info-list.csv");
        }

        //[HttpGet("downloadPart2")]
        //public HttpResponseMessage DownloadCSV()
        //{
        //    var accountInfoList = new List<AccountInfo>();
        //    Enumerable.Range(1, 10).ToList().ForEach(id =>
        //        accountInfoList.Add(new AccountInfo
        //        {
        //            UserId = id,
        //            Username = $"user-{id}",
        //            Email = $"user-{id}@someemail.com"
        //        }));

        //    IContentNegotiator
        //    var negotiator = IContentNegotiator
        //}


    }

    [Serializable]
    public class AccountInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class CsvWriter
    {
        private const string CSV_DELIMITER = ",";

        public void Write<T>(List<T> objectLists, string fileName, bool includeHeaders = true)
        {
            Type type = typeof(T);
            var sb = new StringBuilder();
        
            // Include headers
            if (includeHeaders)
                sb.Append(CreateCsv2HeaderLine(type.GetProperties()));

            
            
        }

        public StringBuilder CreateCsv2HeaderLine(PropertyInfo[] properties)
        {
            var sb = new StringBuilder();
            foreach (var property in properties)
            {
                sb.Append(property.Name);
                sb.Append(CSV_DELIMITER);
            }

            return sb;
        }

    }
}
