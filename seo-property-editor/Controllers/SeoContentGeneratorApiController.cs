using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace SeoPropertyEditor.Controllers
{
    [PluginController("SeoPropertyEditor")]
    public class SeoContentGeneratorApiController : UmbracoAuthorizedApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SeoContentGeneratorApiController> _logger;

        public SeoContentGeneratorApiController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<SeoContentGeneratorApiController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] SeoRequest request)
        {
            if (request == null || request.Fields == null)
            {
                return BadRequest("No fields provided");
            }

            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "API key not configured");
            }

            var prompt = string.Join(" \n", request.Fields.Select(f => f.Value));
            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new [] {
                    new { role = "system", content = "You are an SEO assistant." },
                    new { role = "user", content = $"Generate SEO optimized text based on the following content: {prompt}" }
                }
            };

            var response = await http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", body);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("OpenAI request failed: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, "AI request failed");
            }

            var resultJson = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            var generated = resultJson?.choices?.FirstOrDefault()?.message?.content ?? string.Empty;
            return Ok(generated);
        }
    }

    public class SeoRequest
    {
        public Dictionary<string, string>? Fields { get; set; }
    }

    public class OpenAiResponse
    {
        public List<Choice>? choices { get; set; }
    }

    public class Choice
    {
        public OpenAiMessage? message { get; set; }
    }

    public class OpenAiMessage
    {
        public string? content { get; set; }
    }
}
