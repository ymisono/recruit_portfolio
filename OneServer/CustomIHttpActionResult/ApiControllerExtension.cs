using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OneServer.CustomIHttpActionResult
{
    public static class ApiControllerExtension
    {
        public static NotFoundPlainTextActionResult NotFound(ApiController controller, string message)
        {
            return new NotFoundPlainTextActionResult(controller.Request, message);
        }

        public static InternalServerErrorPlainTextActionResult InternalServerError(ApiController controller, string message)
        {
            return new InternalServerErrorPlainTextActionResult(controller.Request, message);
        }
    }

    public class NotFoundPlainTextActionResult : IHttpActionResult
    {
        public string Message { get; private set; }
        public HttpRequestMessage Request { get; private set; }

        public NotFoundPlainTextActionResult(HttpRequestMessage request, string message)
        {
            this.Request = request;
            this.Message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);

            response.Content = new StringContent(Message);
            response.RequestMessage = Request;
            return response;
        }
    }

    public class InternalServerErrorPlainTextActionResult : IHttpActionResult
    {
        public string Message { get; private set; }
        public HttpRequestMessage Request { get; private set; }

        public InternalServerErrorPlainTextActionResult(HttpRequestMessage request, string message)
        {
            this.Request = request;
            this.Message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            response.Content = new StringContent(Message);
            response.RequestMessage = Request;
            return response;
        }
    }
}