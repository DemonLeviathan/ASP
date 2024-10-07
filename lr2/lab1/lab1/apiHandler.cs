using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text.Json;

namespace lab1
{
    public class apiHandler : IHttpHandler, IRequiresSessionState
    {
        private static int result = 0;
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.HttpMethod;

            switch (method)
            {
                case "GET":
                    HandleGetRequest(context);
                    break;
                case "POST":
                    HandlePostRequest(context);
                    break;
                case "PUT":
                    HandlePutRequest(context);
                    break;
                case "DELETE":
                    HandleDeleteRequest(context);
                    break;
                default:
                    context.Response.StatusCode = 405; 
                    break;
            }
        }

        private Stack<int> GetStack(HttpContext context) {
            if (context.Session["Stack"] == null)
            {
                context.Session["Stack"] = new Stack<int>();
            }
            return (Stack<int>)context.Session["Stack"];
        }

        private void HandleGetRequest(HttpContext context)
        {
            var results = GetStack(context);

            if (results.Count != 0)
            {
                var response = new { RESULT = result + results.Peek() };
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonSerializer.Serialize(response));
            }
            else
            {
                var response = new { RESULT = result };
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonSerializer.Serialize(response));
            }
        }

        private void HandlePostRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string resultValue = context.Request.Path.ToString().Split('/').Last();

            if (int.TryParse(resultValue, out int newResult))
            {
                result = newResult; 
                context.Response.StatusCode = 200; 
                var response = new { RESULT = result };
                context.Response.Write(JsonSerializer.Serialize(response));
            }
            else
            {
                context.Response.StatusCode = 400; 
                context.Response.Write(JsonSerializer.Serialize(new { Error = "Invalid input. 'RESULT' must be an integer." }));
            }
        }


        private void HandlePutRequest(HttpContext context)
        {
            var results = GetStack(context);

            context.Response.ContentType = "application/json";

            string stackValue = context.Request.Path.ToString().Split('/').Last();

            if (int.TryParse(stackValue, out int newResult))
            {
                results.Push(newResult);
                context.Response.StatusCode = 200;
                
                context.Response.Write(JsonSerializer.Serialize(results.Peek()));
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write(JsonSerializer.Serialize(new { Error = "Invalid input. 'ADD' must be an integer." }));
            }
        }

        private void HandleDeleteRequest(HttpContext context)
        {
            var results = GetStack(context);
            context.Response.ContentType = "application/json";

            if (results.Count > 0)
            {
                results.Pop();
                context.Response.StatusCode = 200;
                context.Response.Write(JsonSerializer.Serialize(results.Count()));
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write(JsonSerializer.Serialize(new { Error = "Stack is empty" }));
            }
        }
    }
}