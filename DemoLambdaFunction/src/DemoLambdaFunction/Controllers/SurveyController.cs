using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoLambdaFunction.Data;
using DemoLambdaFunction.Models;

namespace DemoLambdaFunction.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly SurveyDbContext _context;

        public SurveyController(SurveyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var html = System.IO.File.ReadAllText("wwwroot/index.html");
            return Content(html, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSurvey([FromBody] SurveyResponse survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a survey with this email already exists
            bool emailExists = await _context.SurveyResponses.AnyAsync(s => s.Email == survey.Email);
            if (emailExists)
            {
                return Conflict(new { message = "A survey has already been submitted with this email address. Only one survey submission per email is allowed." });
            }

            survey.SubmittedAt = DateTime.UtcNow;
            _context.SurveyResponses.Add(survey);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Survey submitted successfully", redirectUrl = $"/api/survey/thank-you?{BuildQueryString(survey)}" });
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetSurveyResults()
        {
            var results = await _context.SurveyResponses
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            var summary = new
            {
                roleSatisfaction = results
                    .GroupBy(s => s.RoleSatisfaction)
                    .Select(g => new { response = g.Key, count = g.Count() })
                    .ToList(),
                managerSupport = results
                    .GroupBy(s => s.ManagerSupport)
                    .Select(g => new { response = g.Key, count = g.Count() })
                    .ToList(),
                valueRecognition = results
                    .GroupBy(s => s.ValueRecognition)
                    .Select(g => new { response = g.Key, count = g.Count() })
                    .ToList(),
                growthOpportunities = results
                    .GroupBy(s => s.GrowthOpportunities)
                    .Select(g => new { response = g.Key, count = g.Count() })
                    .ToList(),
                companyRecommendation = results
                    .GroupBy(s => s.CompanyRecommendation)
                    .Select(g => new { response = g.Key, count = g.Count() })
                    .ToList()
            };

            return Ok(new { results, summary });
        }

        [HttpGet("admin")]
        public IActionResult AdminDashboard()
        {
            var html = System.IO.File.ReadAllText("wwwroot/admin.html");
            return Content(html, "text/html");
        }

        [HttpGet("all-data")]
        public IActionResult AllDataGrid()
        {
            var html = System.IO.File.ReadAllText("wwwroot/all-data.html");
            return Content(html, "text/html");
        }

        [HttpGet("thank-you")]
        public IActionResult ThankYou()
        {
            var html = System.IO.File.ReadAllText("wwwroot/thank-you.html");
            return Content(html, "text/html");
        }

        private string BuildQueryString(SurveyResponse survey)
        {
            var queryParams = new List<string>
            {
                $"name={Uri.EscapeDataString(survey.Name)}",
                $"email={Uri.EscapeDataString(survey.Email)}",
                $"roleSatisfaction={Uri.EscapeDataString(survey.RoleSatisfaction)}",
                $"managerSupport={Uri.EscapeDataString(survey.ManagerSupport)}",
                $"valueRecognition={Uri.EscapeDataString(survey.ValueRecognition)}",
                $"growthOpportunities={Uri.EscapeDataString(survey.GrowthOpportunities)}",
                $"companyRecommendation={Uri.EscapeDataString(survey.CompanyRecommendation)}"
            };
            return string.Join("&", queryParams);
        }
    }
}
