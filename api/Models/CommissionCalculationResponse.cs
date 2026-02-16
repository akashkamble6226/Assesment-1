namespace AvalphaTechnologies.CommissionCalculator
{
    /// <summary>
    /// Response model for commission calculation API
    /// Contains calculated commission amounts for both Avalpha and Competitor
    /// </summary>
    public class CommissionCalculationResponse
    {
        /// <summary>
        /// Total commission amount for Avalpha Technologies
        /// Calculated as: (LocalSales × 0.20 × AvgAmount) + (ForeignSales × 0.35 × AvgAmount)
        /// </summary>
        public decimal AvalphaTechnologiesCommissionAmount { get; set; }

        /// <summary>
        /// Total commission amount for the competitor
        /// Calculated as: (LocalSales × 0.02 × AvgAmount) + (ForeignSales × 0.0755 × AvgAmount)
        /// </summary>
        public decimal CompetitorCommissionAmount { get; set; }

        /// <summary>
        /// Competitive advantage - difference between Avalpha and Competitor commissions
        /// </summary>
        public decimal CompetitiveAdvantage { get; set; }
    }
}
