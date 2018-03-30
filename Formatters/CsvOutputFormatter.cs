using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace csv_uploader.Formatters
{
    public class CsvOutputFormatter : OutputFormatter
    {
        public readonly CsvFormatterOptions _options;
        public const string CSV_CONTENT_TYPE = "text/csv";

        public string ContentType { get; private set; }

        public CsvOutputFormatter(CsvFormatterOptions options)
        {
            ContentType = CSV_CONTENT_TYPE;
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));

            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=account-info.csv");
        }

        protected override bool CanWriteType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return IsTypeOfIEnumerable(type);
        }

        private bool IsTypeOfIEnumerable(Type type)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType == typeof(IList))
                    return true;
            }

            return false;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            var type = context.Object.GetType();

            Type itemType;

            if (type.GetGenericArguments().Length > 0)
                itemType = type.GetGenericArguments()[0];
            else
                itemType = type.GetElementType();

            var stringWriter = new StringWriter();

            // Adds the property names as header value
            if (_options.UseSingleLineHeaderInCsv)
            {
                // Try to make this async
                stringWriter.WriteLine(
                  string.Join(_options.CsvDelimiter, itemType.GetProperties().Select(p => p.Name))
                );
            }

            foreach (var obj in (IEnumerable<object>)context.Object)
            {

                var values = obj.GetType().GetProperties().Select(prop => 
                        new
                        {
                            Value = prop.GetValue(obj, null)
                        }
                    );

                string valueLine = string.Empty;

                foreach (var value in values)
                {
                    if (value.Value != null)
                    {
                        var valueInString = value.Value.ToString();

                        //Check if the value contains a comma and place it in quotes if so
                        if (valueInString.Contains(","))
                            valueInString = string.Concat("\"", valueInString, "\"");

                        // Replace any \r or \n special characters from a new line with a space
                        if (valueInString.Contains("\r"))
                            valueInString = valueInString.Replace("\r", " ");

                        if (valueInString.Contains("\n"))
                            valueInString = valueInString.Replace("\n", " ");

                        valueLine = string.Concat(valueLine, valueInString, _options.CsvDelimiter);
                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, _options.CsvDelimiter);
                    }
                }

                // removes the last delimiter
                stringWriter.WriteLine(valueLine.TrimEnd(_options.CsvDelimiter.ToCharArray()));
            }

            //response.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //response.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            

            //response.Headers.Add(
            //    new ContentDispositionHeaderValue("attachment")
            //    {
            //        FileName = "account-info.csv"
            //    });
            
            var streamWriter = new StreamWriter(response.Body);
            await streamWriter.WriteAsync(stringWriter.ToString());
            await streamWriter.FlushAsync();
        }
    }
}
