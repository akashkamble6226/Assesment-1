namespace AvalphaTechnologies.CommissionCalculator.Services
{
    /// <summary>
    /// Service responsible for calculating sales commissions.
    /// Separates business logic from controller concerns for better testability.
    /// 
    /// Commission Rates:
    /// - Avalpha Local: 20% (0.20)
    /// - Avalpha Foreign: 35% (0.35)
    /// - Competitor Local: 2% (0.02)
    /// - Competitor Foreign: 7.55% (0.0755)
    /// </summary>
    public class CommissionCalculationService
    {
        // Commission rate constants
        private const decimal AVALPHA_LOCAL_RATE = 0.20m;
        private const decimal AVALPHA_FOREIGN_RATE = 0.35m;
        private const decimal COMPETITOR_LOCAL_RATE = 0.02m;
        private const decimal COMPETITOR_FOREIGN_RATE = 0.0755m;

        /// <summary>
        /// Validates that all input values are non-negative.
        /// </summary>
        /// <returns>Tuple of (isValid, errorMessage)</returns>
        public (bool IsValid, string ErrorMessage) ValidateInputs(int localSalesCount, int foreignSalesCount, decimal averageSaleAmount)
        {
            if (localSalesCount < 0)
                return (false, "Local sales count cannot be negative");
            
            if (foreignSalesCount < 0)
                return (false, "Foreign sales count cannot be negative");
            
            if (averageSaleAmount < 0)
                return (false, "Average sale amount cannot be negative");

            return (true, string.Empty);
        }

        /// <summary>
        /// Calculates Avalpha Technologies commission.
        /// Formula: (LocalSales × 0.20 × AvgAmount) + (ForeignSales × 0.35 × AvgAmount)
        /// </summary>
        public decimal CalculateAvalphaTechnologiesCommission(int localSalesCount, int foreignSalesCount, decimal averageSaleAmount)
        {
            decimal localCommission = localSalesCount * AVALPHA_LOCAL_RATE * averageSaleAmount;
            decimal foreignCommission = foreignSalesCount * AVALPHA_FOREIGN_RATE * averageSaleAmount;
            
            return localCommission + foreignCommission;
        }

        /// <summary>
        /// Calculates Competitor commission.
        /// Formula: (LocalSales × 0.02 × AvgAmount) + (ForeignSales × 0.0755 × AvgAmount)
        /// </summary>
        public decimal CalculateCompetitorCommission(int localSalesCount, int foreignSalesCount, decimal averageSaleAmount)
        {
            decimal localCommission = localSalesCount * COMPETITOR_LOCAL_RATE * averageSaleAmount;
            decimal foreignCommission = foreignSalesCount * COMPETITOR_FOREIGN_RATE * averageSaleAmount;
            
            return localCommission + foreignCommission;
        }

        /// <summary>
        /// Calculates the commission advantage of Avalpha over the competitor.
        /// </summary>
        public decimal CalculateCompetitiveAdvantage(decimal avalphaTechnologiesCommission, decimal competitorCommission)
        {
            return avalphaTechnologiesCommission - competitorCommission;
        }
    }
}
