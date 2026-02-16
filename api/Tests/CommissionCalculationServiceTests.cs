using Xunit;
using AvalphaTechnologies.CommissionCalculator.Services;

namespace AvalphaTechnologies.CommissionCalculator.Tests
{
    /// <summary>
    /// Unit tests for CommissionCalculationService
    /// Tests pure calculation logic in isolation
    /// 
    /// This demonstrates my understanding of:
    /// - Separation of concerns (business logic isolated from HTTP)
    /// - Testable architecture (no dependencies on framework)
    /// - Clear formula verification (each test validates one specific calculation)
    /// </summary>
    public class CommissionCalculationServiceTests
    {
        private readonly CommissionCalculationService _service;

        public CommissionCalculationServiceTests()
        {
            _service = new CommissionCalculationService();
        }

        #region Avalpha Technologies Commission Tests

        [Fact]
        public void CalculateAvalphaTechnologiesCommission_LocalSalesOnly_AppliesLocalRate()
        {
            // Arrange: Only test local sales component
            // Rate: 20% (0.20)
            int localSalesCount = 100;
            int foreignSalesCount = 0;
            decimal averageSaleAmount = 1m;

            // Act
            var result = _service.CalculateAvalphaTechnologiesCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: 100 × 0.20 × 1 = 20
            Assert.Equal(20m, result);
        }

        [Fact]
        public void CalculateAvalphaTechnologiesCommission_ForeignSalesOnly_AppliesForeignRate()
        {
            // Arrange: Only test foreign sales component
            // Rate: 35% (0.35)
            int localSalesCount = 0;
            int foreignSalesCount = 100;
            decimal averageSaleAmount = 1m;

            // Act
            var result = _service.CalculateAvalphaTechnologiesCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: 100 × 0.35 × 1 = 35
            Assert.Equal(35m, result);
        }

        [Fact]
        public void CalculateAvalphaTechnologiesCommission_MixedSales_CombinesRates()
        {
            // Arrange: Test combined calculation
            int localSalesCount = 10;
            int foreignSalesCount = 5;
            decimal averageSaleAmount = 100m;

            // Act
            var result = _service.CalculateAvalphaTechnologiesCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: (10 × 0.20 × 100) + (5 × 0.35 × 100) = 200 + 175 = 375
            decimal expectedLocal = 10 * 0.20m * 100m;  // 200
            decimal expectedForeign = 5 * 0.35m * 100m; // 175
            Assert.Equal(expectedLocal + expectedForeign, result);
            Assert.Equal(375m, result);
        }

        [Fact]
        public void CalculateAvalphaTechnologiesCommission_DecimalAmounts_MaintainsPrecision()
        {
            // Arrange: Test decimal precision
            int localSalesCount = 10;
            int foreignSalesCount = 5;
            decimal averageSaleAmount = 50.5m;

            // Act
            var result = _service.CalculateAvalphaTechnologiesCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: (10 × 0.20 × 50.5) + (5 × 0.35 × 50.5) = 101 + 88.375 = 189.375
            decimal expectedLocal = 10 * 0.20m * 50.5m;    // 101
            decimal expectedForeign = 5 * 0.35m * 50.5m;   // 88.375
            Assert.Equal(expectedLocal + expectedForeign, result);
            Assert.Equal(189.375m, result);
        }

        [Fact]
        public void CalculateAvalphaTechnologiesCommission_ZeroValues_ReturnsZero()
        {
            // Arrange
            int localSalesCount = 0;
            int foreignSalesCount = 0;
            decimal averageSaleAmount = 100m;

            // Act
            var result = _service.CalculateAvalphaTechnologiesCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert
            Assert.Equal(0m, result);
        }

        #endregion

        #region Competitor Commission Tests

        [Fact]
        public void CalculateCompetitorCommission_LocalSalesOnly_AppliesLocalRate()
        {
            // Arrange: Only test local sales component
            // Rate: 2% (0.02)
            int localSalesCount = 100;
            int foreignSalesCount = 0;
            decimal averageSaleAmount = 1m;

            // Act
            var result = _service.CalculateCompetitorCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: 100 × 0.02 × 1 = 2
            Assert.Equal(2m, result);
        }

        [Fact]
        public void CalculateCompetitorCommission_ForeignSalesOnly_AppliesForeignRate()
        {
            // Arrange: Only test foreign sales component
            // Rate: 7.55% (0.0755)
            int localSalesCount = 0;
            int foreignSalesCount = 100;
            decimal averageSaleAmount = 1m;

            // Act
            var result = _service.CalculateCompetitorCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: 100 × 0.0755 × 1 = 7.55
            Assert.Equal(7.55m, result);
        }

        [Fact]
        public void CalculateCompetitorCommission_MixedSales_CombinesRates()
        {
            // Arrange: Test combined calculation
            int localSalesCount = 10;
            int foreignSalesCount = 5;
            decimal averageSaleAmount = 100m;

            // Act
            var result = _service.CalculateCompetitorCommission(
                localSalesCount, foreignSalesCount, averageSaleAmount
            );

            // Assert: (10 × 0.02 × 100) + (5 × 0.0755 × 100) = 20 + 37.75 = 57.75
            decimal expectedLocal = 10 * 0.02m * 100m;     // 20
            decimal expectedForeign = 5 * 0.0755m * 100m;  // 37.75
            Assert.Equal(expectedLocal + expectedForeign, result);
            Assert.Equal(57.75m, result);
        }

        #endregion

        #region Validation Tests

        [Fact]
        public void ValidateInputs_AllPositiveValues_ReturnsValid()
        {
            // Arrange
            var (isValid, errorMessage) = _service.ValidateInputs(10, 5, 100m);

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void ValidateInputs_NegativeLocalSales_ReturnsInvalid()
        {
            // Arrange
            var (isValid, errorMessage) = _service.ValidateInputs(-1, 5, 100m);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Local sales", errorMessage);
        }

        [Fact]
        public void ValidateInputs_NegativeForeignSales_ReturnsInvalid()
        {
            // Arrange
            var (isValid, errorMessage) = _service.ValidateInputs(10, -1, 100m);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Foreign sales", errorMessage);
        }

        [Fact]
        public void ValidateInputs_NegativeAmount_ReturnsInvalid()
        {
            // Arrange
            var (isValid, errorMessage) = _service.ValidateInputs(10, 5, -100m);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Average sale amount", errorMessage);
        }

        [Fact]
        public void ValidateInputs_ZeroValues_ReturnsValid()
        {
            // Arrange: Zero is valid (means no sales)
            var (isValid, errorMessage) = _service.ValidateInputs(0, 0, 0m);

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
        }

        #endregion

        #region Competitive Advantage Tests

        [Fact]
        public void CalculateCompetitiveAdvantage_ComputesDifference()
        {
            // Arrange
            decimal avalpha = 2750m;
            decimal competitor = 477.5m;

            // Act
            var result = _service.CalculateCompetitiveAdvantage(avalpha, competitor);

            // Assert: 2750 - 477.5 = 2272.5
            Assert.Equal(2272.5m, result);
            Assert.True(result > 0, "Advantage should be positive");
        }

        [Fact]
        public void CalculateCompetitiveAdvantage_AlwaysPositive()
        {
            // Arrange: Test multiple scenarios
            var scenarios = new[] {
                (avalpha: 100m, competitor: 10m),
                (avalpha: 1000m, competitor: 100m),
                (avalpha: 500m, competitor: 50m),
            };

            foreach (var (avalpha, competitor) in scenarios)
            {
                // Act
                var result = _service.CalculateCompetitiveAdvantage(avalpha, competitor);

                // Assert: Avalpha commission should always be higher
                Assert.True(result > 0, $"Advantage should be positive for Avalpha={avalpha}, Competitor={competitor}");
            }
        }

        #endregion
    }
}
