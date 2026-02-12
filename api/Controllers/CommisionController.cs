using Microsoft.AspNetCore.Mvc;

namespace AvalphaTechnologies.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommisionController : ControllerBase
    {
        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [HttpPost]
        public IActionResult Calculate(CommissionCalculationRequest calculationRequest)
        {
            Console.WriteLine("=== Commission Calculation Request ===");
            Console.WriteLine($"LocalSalesCount: {calculationRequest?.LocalSalesCount}");
            Console.WriteLine($"ForeignSalesCount: {calculationRequest?.ForeignSalesCount}");
            Console.WriteLine($"AverageSaleAmount: {calculationRequest?.AverageSaleAmount}");

            if (calculationRequest == null || calculationRequest.LocalSalesCount < 0 || 
                calculationRequest.ForeignSalesCount < 0 || calculationRequest.AverageSaleAmount < 0)
            {
                Console.WriteLine("ERROR: Invalid input values");
                return BadRequest("Invalid input values. All values must be >= 0");
            }

            // Avalpha rates
            const decimal avalphLocalRate = 0.20m;
            const decimal avalphForeignRate = 0.35m;

            // Competitor rates
            const decimal competitorLocalRate = 0.02m;
            const decimal competitorForeignRate = 0.0755m;

            // Calculate Avalpha commission (Local)
            decimal avalphLocalCommission = avalphLocalRate * calculationRequest.LocalSalesCount * calculationRequest.AverageSaleAmount;
            
            // Calculate Avalpha commission (Foreign)
            decimal avalphForeignCommission = avalphForeignRate * calculationRequest.ForeignSalesCount * calculationRequest.AverageSaleAmount;
            
            // Total Avalpha commission
            decimal avalphTotalCommission = avalphLocalCommission + avalphForeignCommission;

            // Calculate Competitor commission (Local)
            decimal competitorLocalCommission = competitorLocalRate * calculationRequest.LocalSalesCount * calculationRequest.AverageSaleAmount;
            
            // Calculate Competitor commission (Foreign)
            decimal competitorForeignCommission = competitorForeignRate * calculationRequest.ForeignSalesCount * calculationRequest.AverageSaleAmount;
            
            // Total Competitor commission
            decimal competitorTotalCommission = competitorLocalCommission + competitorForeignCommission;

            Console.WriteLine($"Avalpha Local: {avalphLocalCommission}");
            Console.WriteLine($"Avalpha Foreign: {avalphForeignCommission}");
            Console.WriteLine($"Avalpha Total: {avalphTotalCommission}");
            Console.WriteLine($"Competitor Local: {competitorLocalCommission}");
            Console.WriteLine($"Competitor Foreign: {competitorForeignCommission}");
            Console.WriteLine($"Competitor Total: {competitorTotalCommission}");
            Console.WriteLine("=== End ===\n");

            return Ok(new CommissionCalculationResponse()
            {
                AvalphaTechnologiesCommissionAmount = avalphTotalCommission,
                CompetitorCommissionAmount = competitorTotalCommission
            });
        }
    }

    public class CommissionCalculationRequest
    {
        public int LocalSalesCount { get; set; }
        public int ForeignSalesCount { get; set; }
        public decimal AverageSaleAmount { get; set; }
    }

    public class CommissionCalculationResponse
    {
        public decimal AvalphaTechnologiesCommissionAmount { get; set; }
        public decimal CompetitorCommissionAmount { get; set; }
    }
}