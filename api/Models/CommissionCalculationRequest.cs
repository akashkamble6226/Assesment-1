namespace AvalphaTechnologies.CommissionCalculator
{
    /// <summary>
    /// Request model for commission calculation API
    /// Contains sales data needed to calculate commissions
    /// </summary>
    public class CommissionCalculationRequest
    {
        /// <summary>
        /// Number of local sales transactions
        /// </summary>
        public int LocalSalesCount { get; set; }

        /// <summary>
        /// Number of foreign sales transactions
        /// </summary>
        public int ForeignSalesCount { get; set; }

        /// <summary>
        /// Average sale amount in currency units
        /// </summary>
        public decimal AverageSaleAmount { get; set; }
    }
}
