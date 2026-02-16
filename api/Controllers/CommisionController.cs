using Microsoft.AspNetCore.Mvc;
using AvalphaTechnologies.CommissionCalculator.Services;
using AvalphaTechnologies.CommissionCalculator;

namespace AvalphaTechnologies.CommissionCalculator.Controllers
{
    /// <summary>
    /// API Controller for commission calculations
    /// Handles HTTP requests and delegates calculation logic to CommissionCalculationService
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CommisionController : ControllerBase
    {
        private readonly CommissionCalculationService _commissionService;

        public CommisionController()
        {
            // Initialize service
            _commissionService = new CommissionCalculationService();
        }

        /// <summary>
        /// Calculates sales commissions for Avalpha Technologies and competitor
        /// POST /api/commision/calculate
        /// </summary>
        /// <param name="request">Commission calculation request containing sales data</param>
        /// <returns>Commission calculation response with results</returns>
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] CommissionCalculationRequest request)
        {
            // Null check
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            // Validate inputs using service
            var (isValid, errorMessage) = _commissionService.ValidateInputs(
                request.LocalSalesCount,
                request.ForeignSalesCount,
                request.AverageSaleAmount
            );

            if (!isValid)
            {
                return BadRequest(errorMessage);
            }

            // Calculate commissions using service
            decimal avalphaTechnologiesCommission = _commissionService.CalculateAvalphaTechnologiesCommission(
                request.LocalSalesCount,
                request.ForeignSalesCount,
                request.AverageSaleAmount
            );

            decimal competitorCommission = _commissionService.CalculateCompetitorCommission(
                request.LocalSalesCount,
                request.ForeignSalesCount,
                request.AverageSaleAmount
            );

            decimal competitiveAdvantage = _commissionService.CalculateCompetitiveAdvantage(
                avalphaTechnologiesCommission,
                competitorCommission
            );

            // Return response
            var response = new CommissionCalculationResponse
            {
                AvalphaTechnologiesCommissionAmount = avalphaTechnologiesCommission,
                CompetitorCommissionAmount = competitorCommission,
                CompetitiveAdvantage = competitiveAdvantage
            };

            return Ok(response);
        }
    }
}