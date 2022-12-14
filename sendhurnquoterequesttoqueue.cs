using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Bendando.SendHurnQuoteToQueue
{
    public class sendhurnquoterequesttoqueue
    {
        private readonly ILogger _logger;

        public sendhurnquoterequesttoqueue(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<sendhurnquoterequesttoqueue>();
        }

        [Function("sendhurnquoterequesttoqueue")]
        public static MultiResponse Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("HttpExample");
        logger.LogInformation("C# HTTP trigger function processed a request.");

			string requestBody = String.Empty;
			using (StreamReader streamReader =  new  StreamReader(req.Body))
			{
				requestBody = ' ' + streamReader.ReadToEnd();
			}

        var message = requestBody;

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(message);

        // Return a response to both HTTP trigger and storage output binding.
        return new MultiResponse()
        {
            // Write a single message.
            Messages = new string[] { message },
            HttpResponse = response
        };
		}
	}

	public class MultiResponse
	{
    [QueueOutput("outqueue",Connection = "AzureWebJobsStorage")]
    public string[] Messages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
	}




}
