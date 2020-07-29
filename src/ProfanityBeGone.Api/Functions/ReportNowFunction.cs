using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProfanityBeGone.Api.Requests;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityBeGone.Api.Functions
{
    public class ReportNowFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<ReportNowFunction> _logger;

        public ReportNowFunction(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options, ILogger<ReportNowFunction> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName("report-now")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            var contentType = "Text";

            var textToReview = JsonConvert.DeserializeObject<CreateReviewRequest>(await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false)).ContentValue;

            if (!string.IsNullOrWhiteSpace(textToReview))
            {
                const int MaxSplitLength = 1024;
                var iteration = 0;

                do
                {
                    var remainingTextLength = textToReview.Length - (iteration * MaxSplitLength);
                    var splitLength = remainingTextLength < MaxSplitLength ? remainingTextLength : MaxSplitLength;
                    var startingIndex = iteration * MaxSplitLength;
                    var textSection = textToReview.Substring(startingIndex, splitLength);

                    if (!string.IsNullOrWhiteSpace(textSection))
                    {
                        var contentId = Guid.NewGuid().ToString();
                        var endpoint = $"https://westus2.api.cognitive.microsoft.com/contentmoderator/review/v1.0/teams/hackathon2020wus2/jobs?ContentType={contentType}&ContentId={contentId}&WorkflowName=hackathontext";
                        var httpClient = _httpClientFactory.CreateClient();

                        var requestBody = new CreateReviewRequest()
                        {
                            ContentValue = textSection
                        };

                        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                        content.Headers.Add("Ocp-Apim-Subscription-Key", _options.Value.ContentModeratorApiKey);

                        await httpClient.PostAsync(endpoint, content).ConfigureAwait(false);
                    }

                    iteration++;
                } while (iteration * MaxSplitLength < textToReview.Length);
            }

            return new OkResult();
        }
    }
}