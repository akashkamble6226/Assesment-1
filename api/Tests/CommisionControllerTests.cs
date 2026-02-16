using Xunit;
using Microsoft.AspNetCore.Mvc;
using AvalphaTechnologies.CommissionCalculator.Controllers;
using AvalphaTechnologies.CommissionCalculator.Services;
using AvalphaTechnologies.CommissionCalculator;

namespace AvalphaTechnologies.CommissionCalculator.Tests
{
    /// <summary>
    /// Unit tests for CommissionController
    /// Focus: Verify calculation logic correctness and validation behavior
    /// </summary>
    public class CommissionControllerTests
    {
        private readonly CommisionController _controller;

        public CommissionControllerTests()
        {
            _controller = new CommisionController();
        }

        #region Basic Calculation Tests - Verifying Formulas

        [Fact]
        public void Calculate_BasicValidInput_VerifiesAvalphaTechnologiesFormula()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 5,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert
            // Formula: (LocalSales × 0.20) + (ForeignSales × 0.35) × AvgAmount
            // Expected: (10 × 0.20 × 100) + (5 × 0.35 × 100) = 200 + 175 = 375
            decimal expectedAvalpha = (10 * 0.20m * 100) + (5 * 0.35m * 100);
            Assert.Equal(expectedAvalpha, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(375, response.AvalphaTechnologiesCommissionAmount);
        }

        [Fact]
        public void Calculate_BasicValidInput_VerifiesCompetitorFormula()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 5,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert
            // Formula: (LocalSales × 0.02) + (ForeignSales × 0.0755) × AvgAmount
            // Expected: (10 × 0.02 × 100) + (5 × 0.0755 × 100) = 20 + 37.75 = 57.75
            decimal expectedCompetitor = (10 * 0.02m * 100) + (5 * 0.0755m * 100);
            Assert.Equal(expectedCompetitor, response.CompetitorCommissionAmount);
            Assert.Equal(57.75m, response.CompetitorCommissionAmount);
        }

        #endregion

        #region Commission Rate Verification Tests

        [Fact]
        public void Calculate_AvalphaTechnologiesLocalRate_IsCorrect()
        {
            // Arrange: Isolate local sales component
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 100,
                ForeignSalesCount = 0,
                AverageSaleAmount = 1
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify rate is exactly 20% (0.20)
            // 100 × 0.20 × 1 = 20
            Assert.Equal(20m, response.AvalphaTechnologiesCommissionAmount);
        }

        [Fact]
        public void Calculate_AvalphaTechnologiesForeignRate_IsCorrect()
        {
            // Arrange: Isolate foreign sales component
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 0,
                ForeignSalesCount = 100,
                AverageSaleAmount = 1
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify rate is exactly 35% (0.35)
            // 100 × 0.35 × 1 = 35
            Assert.Equal(35m, response.AvalphaTechnologiesCommissionAmount);
        }

        [Fact]
        public void Calculate_CompetitorLocalRate_IsCorrect()
        {
            // Arrange: Isolate local sales component
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 100,
                ForeignSalesCount = 0,
                AverageSaleAmount = 1
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify rate is exactly 2% (0.02)
            // 100 × 0.02 × 1 = 2
            Assert.Equal(2m, response.CompetitorCommissionAmount);
        }

        [Fact]
        public void Calculate_CompetitorForeignRate_IsCorrect()
        {
            // Arrange: Isolate foreign sales component
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 0,
                ForeignSalesCount = 100,
                AverageSaleAmount = 1
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify rate is exactly 7.55% (0.0755)
            // 100 × 0.0755 × 1 = 7.55
            Assert.Equal(7.55m, response.CompetitorCommissionAmount);
        }

        #endregion

        #region Edge Case Tests - Zero Values

        [Fact]
        public void Calculate_ZeroLocalSales_IgnoresLocalComponent()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 0,
                ForeignSalesCount = 10,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Only foreign sales contribute
            // Avalpha: 0 + (10 × 0.35 × 100) = 350
            // Competitor: 0 + (10 × 0.0755 × 100) = 75.5
            Assert.Equal(350m, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(75.5m, response.CompetitorCommissionAmount);
        }

        [Fact]
        public void Calculate_ZeroForeignSales_IgnoresForeignComponent()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 0,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Only local sales contribute
            // Avalpha: (10 × 0.20 × 100) + 0 = 200
            // Competitor: (10 × 0.02 × 100) + 0 = 20
            Assert.Equal(200m, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(20m, response.CompetitorCommissionAmount);
        }

        [Fact]
        public void Calculate_AllZeroSales_ReturnsZeroCommission()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 0,
                ForeignSalesCount = 0,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert
            Assert.Equal(0m, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(0m, response.CompetitorCommissionAmount);
        }

        [Fact]
        public void Calculate_ZeroAverageSaleAmount_ReturnsZeroCommission()
        {
            // Arrange: Sales counts are high but average amount is zero
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 100,
                ForeignSalesCount = 50,
                AverageSaleAmount = 0
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Anything × 0 = 0
            Assert.Equal(0m, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(0m, response.CompetitorCommissionAmount);
        }

        #endregion

        #region Validation Tests - Negative Values

        [Fact]
        public void Calculate_NegativeLocalSalesCount_ReturnsBadRequest()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = -1,
                ForeignSalesCount = 5,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Calculate_NegativeForeignSalesCount_ReturnsBadRequest()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 5,
                ForeignSalesCount = -1,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Calculate_NegativeAverageSaleAmount_ReturnsBadRequest()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 5,
                AverageSaleAmount = -100
            };

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Calculate_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            CommissionCalculationRequest request = null;

            // Act
            var result = _controller.Calculate(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        #endregion

        #region Decimal Precision Tests

        [Fact]
        public void Calculate_DecimalAverageSaleAmount_MaintainsPrecision()
        {
            // Arrange: Test with precise decimal amounts
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 5,
                AverageSaleAmount = 50.5m
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify precise calculation
            // Avalpha: (10 × 0.20 × 50.5) + (5 × 0.35 × 50.5) = 101 + 88.375 = 189.375
            decimal expectedAvalpha = (10 * 0.20m * 50.5m) + (5 * 0.35m * 50.5m);
            Assert.Equal(expectedAvalpha, response.AvalphaTechnologiesCommissionAmount);
            Assert.Equal(189.375m, response.AvalphaTechnologiesCommissionAmount);

            // Competitor: (10 × 0.02 × 50.5) + (5 × 0.0755 × 50.5) = 10.1 + 19.06375 = 29.16375
            decimal expectedCompetitor = (10 * 0.02m * 50.5m) + (5 * 0.0755m * 50.5m);
            Assert.Equal(expectedCompetitor, response.CompetitorCommissionAmount);
            Assert.Equal(29.16375m, response.CompetitorCommissionAmount);
        }

        #endregion

        #region Large Number Tests

        [Fact]
        public void Calculate_LargeSalesVolumes_CalculatesAccurately()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 1000,
                ForeignSalesCount = 500,
                AverageSaleAmount = 1000
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Verify large number calculations
            // Avalpha: (1000 × 0.20 × 1000) + (500 × 0.35 × 1000) = 200000 + 175000 = 375000
            decimal expectedAvalpha = (1000 * 0.20m * 1000) + (500 * 0.35m * 1000);
            Assert.Equal(expectedAvalpha, response.AvalphaTechnologiesCommissionAmount);

            // Competitor: (1000 × 0.02 × 1000) + (500 × 0.0755 × 1000) = 20000 + 37750 = 57750
            decimal expectedCompetitor = (1000 * 0.02m * 1000) + (500 * 0.0755m * 1000);
            Assert.Equal(expectedCompetitor, response.CompetitorCommissionAmount);
        }

        #endregion

        #region Business Logic Verification Tests

        [Fact]
        public void Calculate_AvalphaShouldAlwaysExceedCompetitor()
        {
            // Arrange: Multiple scenarios to verify competitive advantage
            var scenarios = new[]
            {
                new { Local = 10, Foreign = 5, Avg = 100m },
                new { Local = 50, Foreign = 30, Avg = 200m },
                new { Local = 100, Foreign = 0, Avg = 500m },
                new { Local = 0, Foreign = 100, Avg = 50m },
            };

            foreach (var scenario in scenarios)
            {
                // Act
                var request = new CommissionCalculationRequest
                {
                    LocalSalesCount = scenario.Local,
                    ForeignSalesCount = scenario.Foreign,
                    AverageSaleAmount = scenario.Avg
                };
                var result = _controller.Calculate(request);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

                // Assert: Avalpha must always be greater
                Assert.True(
                    response.AvalphaTechnologiesCommissionAmount > response.CompetitorCommissionAmount,
                    $"Failed for scenario: Local={scenario.Local}, Foreign={scenario.Foreign}, Avg={scenario.Avg}"
                );
            }
        }

        [Fact]
        public void Calculate_VerifyCompetitiveAdvantageCalculation()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 50,
                ForeignSalesCount = 50,
                AverageSaleAmount = 100
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert: Manually calculate to verify logic
            // Avalpha: (50 × 0.20 × 100) + (50 × 0.35 × 100) = 1000 + 1750 = 2750
            decimal expectedAvalpha = (50 * 0.20m * 100) + (50 * 0.35m * 100);
            Assert.Equal(2750m, expectedAvalpha);
            Assert.Equal(2750m, response.AvalphaTechnologiesCommissionAmount);

            // Competitor: (50 × 0.02 × 100) + (50 × 0.0755 × 100) = 100 + 377.5 = 477.5
            decimal expectedCompetitor = (50 * 0.02m * 100) + (50 * 0.0755m * 100);
            Assert.Equal(477.5m, expectedCompetitor);
            Assert.Equal(477.5m, response.CompetitorCommissionAmount);

            // Advantage: 2750 - 477.5 = 2272.5
            decimal expectedAdvantage = 2272.5m;
            decimal actualAdvantage = response.AvalphaTechnologiesCommissionAmount - response.CompetitorCommissionAmount;
            Assert.Equal(expectedAdvantage, actualAdvantage);
        }

        #endregion

        #region Real-World Scenario Tests

        [Fact]
        public void Calculate_SmallBusinessScenario_VerifiesCalculation()
        {
            // Arrange: Small business - 5 local, 2 foreign sales at £200 average
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 5,
                ForeignSalesCount = 2,
                AverageSaleAmount = 200
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert
            // Avalpha: (5 × 0.20 × 200) + (2 × 0.35 × 200) = 200 + 140 = 340
            Assert.Equal(340m, response.AvalphaTechnologiesCommissionAmount);
            // Competitor: (5 × 0.02 × 200) + (2 × 0.0755 × 200) = 20 + 30.2 = 50.2
            Assert.Equal(50.2m, response.CompetitorCommissionAmount);
        }

        [Fact]
        public void Calculate_MediumBusinessScenario_VerifiesCalculation()
        {
            // Arrange: Medium business - 50 local, 30 foreign at £500 average
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 50,
                ForeignSalesCount = 30,
                AverageSaleAmount = 500
            };

            // Act
            var result = _controller.Calculate(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CommissionCalculationResponse>(okResult.Value);

            // Assert
            // Avalpha: (50 × 0.20 × 500) + (30 × 0.35 × 500) = 5000 + 5250 = 10250
            Assert.Equal(10250m, response.AvalphaTechnologiesCommissionAmount);
            // Competitor: (50 × 0.02 × 500) + (30 × 0.0755 × 500) = 500 + 1132.5 = 1632.5
            Assert.Equal(1632.5m, response.CompetitorCommissionAmount);
        }

        #endregion
    }
}
