﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using FhirStarter.Bonfire.STU3.DotNetCore.Exceptions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Filter
{
    public abstract class AbstractExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //HttpActionExecutedContext
        public override void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            var expectedType = GetExceptionType();
            var exceptionMessage = context.Exception.Message;

            if (exceptionType != expectedType && !(expectedType == typeof(Exception))) return;

            Resource operationOutcome = null;
            if (exceptionMessage.Contains("<" + nameof(OperationOutcome)))
            {
                var serializer = new FhirXmlParser();
                operationOutcome = serializer.Parse<OperationOutcome>(exceptionMessage);
            }
            var outCome = operationOutcome ?? GetOperationOutCome(context.Exception);
            var xmlSerializer = new FhirXmlSerializer();
            var xml = xmlSerializer.SerializeToString(outCome);
            var internalOutCome = new FhirXmlParser().Parse<OperationOutcome>(xml);
            internalOutCome.Issue[0].Diagnostics = context.Exception.StackTrace;
            xml = xmlSerializer.SerializeToString(internalOutCome);
            var xmlDoc = XDocument.Parse(xml);
            var error = xmlDoc.ToString();
            var htmlDecode = WebUtility.HtmlDecode(error);
            Log.Error(htmlDecode);
            SetResponseForClient(context, outCome);
        }

        private static string GetAcceptHeaders(HttpContext context)
        {
            var accept = $"{context.Request.Headers["accept"]}";
            return accept;
        }

        private static void SetResponseForClient(ExceptionContext context, Resource outCome)
        {
            // "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"
            //     var acceptEntry = HttpContext.Current.Request.Headers["Accept"];
            var acceptEntry = GetAcceptHeaders(HttpContext);
            var acceptJson = acceptEntry.Contains(FhirMediaType.HeaderTypeJson);
            var jsonSerializer = new FhirJsonSerializer();
            var xmlSerializer = new FhirXmlSerializer();
            var statusCode = HttpStatusCode.InternalServerError;
            if (context.Exception is ValidateInputException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            if (acceptJson)
            {
                var json = jsonSerializer.SerializeToString(outCome);
                context.Response = new HttpResponseMessage
                {
                    Content = new StringContent(json, Encoding.UTF8, FhirMediaType.JsonResource),
                    StatusCode = statusCode
                };
            }
            else
            {
                var xml = xmlSerializer.SerializeToString(outCome);
                context.Response = new HttpResponseMessage
                {
                    Content = new StringContent(xml, Encoding.UTF8, FhirMediaType.XmlResource),
                    StatusCode = statusCode
                };
            }
        }

        protected abstract Resource GetOperationOutCome(Exception exception);

        protected abstract Type GetExceptionType();
    }
}
