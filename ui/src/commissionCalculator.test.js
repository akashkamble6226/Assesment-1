import {
  calculateCommissions,
  formatLargeNumber,
  COMMISSION_RATES_EXPORT,
} from "./commissionCalculator";

describe("Commission Calculator - Core Logic Tests", () => {
  describe("calculateCommissions", () => {
    describe("Basic Calculation Tests", () => {
      test("should calculate commissions correctly for basic inputs", () => {
        // Arrange: Setup test data
        const localSalesCount = 10;
        const foreignSalesCount = 5;
        const averageSaleAmount = 100;

        // Act: Call the function
        const result = calculateCommissions(
          localSalesCount,
          foreignSalesCount,
          averageSaleAmount
        );

        // Assert: Verify calculations
        // Avalpha: (10 * 0.20 * 100) + (5 * 0.35 * 100) = 200 + 175 = 375
        expect(result.avalphaTechnologiesCommissionAmount).toBe(375);

        // Competitor: (10 * 0.02 * 100) + (5 * 0.0755 * 100) = 20 + 37.75 = 57.75
        expect(result.competitorCommissionAmount).toBeCloseTo(57.75, 2);
      });

      test("should return 0 commission when sales count is 0", () => {
        // Test edge case: no sales
        const result = calculateCommissions(0, 0, 100);

        expect(result.avalphaTechnologiesCommissionAmount).toBe(0);
        expect(result.competitorCommissionAmount).toBe(0);
      });

      test("should handle decimal average sale amounts", () => {
        // Test with realistic decimal values
        const result = calculateCommissions(10, 5, 50.5);

        // Avalpha: (10 * 0.20 * 50.5) + (5 * 0.35 * 50.5) = 101 + 88.375 = 189.375
        expect(result.avalphaTechnologiesCommissionAmount).toBeCloseTo(
          189.375,
          2
        );

        // Competitor: (10 * 0.02 * 50.5) + (5 * 0.0755 * 50.5) = 10.1 + 19.01375 = 29.11375
        expect(result.competitorCommissionAmount).toBeCloseTo(29.11375, 2);
      });
    });

    describe("Large Number Calculations", () => {
      test("should correctly calculate commissions for large sales volumes", () => {
        // Test with large numbers to ensure no precision loss
        const result = calculateCommissions(1000, 500, 1000);

        // Avalpha: (1000 * 0.20 * 1000) + (500 * 0.35 * 1000) = 200000 + 175000 = 375000
        expect(result.avalphaTechnologiesCommissionAmount).toBe(375000);

        // Competitor: (1000 * 0.02 * 1000) + (500 * 0.0755 * 1000) = 20000 + 37750 = 57750
        expect(result.competitorCommissionAmount).toBe(57750);
      });

      test("should handle very large numbers without overflow", () => {
        // Test with very large sales amounts
        const result = calculateCommissions(10000, 5000, 10000);

        expect(result.avalphaTechnologiesCommissionAmount).toBe(37500000);
        expect(result.competitorCommissionAmount).toBe(5775000);
      });
    });

    describe("Avalpha vs Competitor Comparison", () => {
      test("Avalpha should always have higher commission for mixed sales", () => {
        // Test that Avalpha rates are better than competitor
        const result = calculateCommissions(100, 100, 500);

        expect(result.avalphaTechnologiesCommissionAmount).toBeGreaterThan(
          result.competitorCommissionAmount
        );
      });

      test("should correctly calculate the advantage gap", () => {
        // Verify the competitive advantage calculation
        const result = calculateCommissions(50, 50, 100);

        const advantage =
          result.avalphaTechnologiesCommissionAmount -
          result.competitorCommissionAmount;

        // Avalpha: (50 * 0.20 * 100) + (50 * 0.35 * 100) = 1000 + 1750 = 2750
        // Competitor: (50 * 0.02 * 100) + (50 * 0.0755 * 100) = 100 + 377.5 = 477.5
        // Advantage: 2750 - 477.5 = 2272.5
        expect(advantage).toBeCloseTo(2272.5, 1);
      });
    });

    describe("Input Validation", () => {
      test("should throw error for negative local sales count", () => {
        expect(() => {
          calculateCommissions(-1, 5, 100);
        }).toThrow("All inputs must be non-negative");
      });

      test("should throw error for negative foreign sales count", () => {
        expect(() => {
          calculateCommissions(5, -1, 100);
        }).toThrow("All inputs must be non-negative");
      });

      test("should throw error for negative average sale amount", () => {
        expect(() => {
          calculateCommissions(5, 5, -100);
        }).toThrow("All inputs must be non-negative");
      });

      test("should throw error for non-numeric inputs", () => {
        expect(() => {
          calculateCommissions("10", 5, 100);
        }).toThrow("All inputs must be numbers");

        expect(() => {
          calculateCommissions(10, "5", 100);
        }).toThrow("All inputs must be numbers");

        expect(() => {
          calculateCommissions(10, 5, "100");
        }).toThrow("All inputs must be numbers");
      });
    });

    describe("Real-world Scenarios", () => {
      test("small business scenario", () => {
        // Small business: 5 local sales, 2 foreign sales, £200 average
        const result = calculateCommissions(5, 2, 200);

        // Avalpha: (5 * 0.20 * 200) + (2 * 0.35 * 200) = 200 + 140 = 340
        expect(result.avalphaTechnologiesCommissionAmount).toBe(340);

        // Competitor: (5 * 0.02 * 200) + (2 * 0.0755 * 200) = 20 + 30.2 = 50.2
        expect(result.competitorCommissionAmount).toBeCloseTo(50.2, 1);
      });

      test("medium business scenario", () => {
        // Medium business: 50 local sales, 30 foreign sales, £500 average
        const result = calculateCommissions(50, 30, 500);

        // Avalpha: (50 * 0.20 * 500) + (30 * 0.35 * 500) = 5000 + 5250 = 10250
        expect(result.avalphaTechnologiesCommissionAmount).toBe(10250);

        // Competitor: (50 * 0.02 * 500) + (30 * 0.0755 * 500) = 500 + 1132.5 = 1632.5
        expect(result.competitorCommissionAmount).toBeCloseTo(1632.5, 1);
      });

      test("high-volume business scenario", () => {
        // High-volume business: 500 local sales, 300 foreign sales, £1000 average
        const result = calculateCommissions(500, 300, 1000);

        // Avalpha: (500 * 0.20 * 1000) + (300 * 0.35 * 1000) = 100000 + 105000 = 205000
        expect(result.avalphaTechnologiesCommissionAmount).toBe(205000);

        // Competitor: (500 * 0.02 * 1000) + (300 * 0.0755 * 1000) = 10000 + 22650 = 32650
        expect(result.competitorCommissionAmount).toBe(32650);
      });
    });
  });

  describe("formatLargeNumber", () => {
    describe("Thousands (K) Formatting", () => {
      test("should format numbers >= 1000 as thousands", () => {
        expect(formatLargeNumber(1500)).toBe("1.50K");
        expect(formatLargeNumber(5000)).toBe("5.00K");
        expect(formatLargeNumber(999999)).toBe("1000.00K");
      });

      test("should handle edge case at 1000", () => {
        expect(formatLargeNumber(1000)).toBe("1.00K");
      });
    });

    describe("Millions (M) Formatting", () => {
      test("should format numbers >= 1000000 as millions", () => {
        expect(formatLargeNumber(1000000)).toBe("1.00M");
        expect(formatLargeNumber(5500000)).toBe("5.50M");
        expect(formatLargeNumber(999999999)).toBe("1000.00M");
      });
    });

    describe("Billions (B) Formatting", () => {
      test("should format numbers >= 1000000000 as billions", () => {
        expect(formatLargeNumber(1000000000)).toBe("1.00B");
        expect(formatLargeNumber(2500000000)).toBe("2.50B");
      });
    });

    describe("Trillions (T) Formatting", () => {
      test("should format numbers >= 1000000000000 as trillions", () => {
        expect(formatLargeNumber(1000000000000)).toBe("1.00T");
        expect(formatLargeNumber(5555000000000)).toBe("5.56T");
      });
    });

    describe("Small Numbers Formatting", () => {
      test("should format numbers < 1000 without abbreviation", () => {
        expect(formatLargeNumber(100)).toBe("100.00");
        expect(formatLargeNumber(999.99)).toBe("999.99");
        expect(formatLargeNumber(0)).toBe("0.00");
      });

      test("should format very small numbers correctly", () => {
        expect(formatLargeNumber(0.5)).toBe("0.50");
        expect(formatLargeNumber(10.25)).toBe("10.25");
      });
    });

    describe("Negative Numbers Formatting", () => {
      test("should preserve negative sign in abbreviated format", () => {
        expect(formatLargeNumber(-1500)).toBe("-1.50K");
        expect(formatLargeNumber(-2000000)).toBe("-2.00M");
        expect(formatLargeNumber(-3000000000)).toBe("-3.00B");
      });

      test("should preserve negative sign for small numbers", () => {
        expect(formatLargeNumber(-100)).toBe("-100.00");
        expect(formatLargeNumber(-500.5)).toBe("-500.50");
      });
    });

    describe("Decimal Precision", () => {
      test("should maintain 2 decimal places in all formats", () => {
        expect(formatLargeNumber(1234567)).toBe("1.23M");
        expect(formatLargeNumber(9876543210)).toBe("9.88B");
        expect(formatLargeNumber(1111111111111)).toBe("1.11T");
      });
    });
  });

  describe("Commission Rates Verification", () => {
    test("should have correct Avalpha commission rates", () => {
      expect(COMMISSION_RATES_EXPORT.avalpha.local).toBe(0.2);
      expect(COMMISSION_RATES_EXPORT.avalpha.foreign).toBe(0.35);
    });

    test("should have correct competitor commission rates", () => {
      expect(COMMISSION_RATES_EXPORT.competitor.local).toBe(0.02);
      expect(COMMISSION_RATES_EXPORT.competitor.foreign).toBe(0.0755);
    });

    test("Avalpha rates should be higher than competitor rates", () => {
      expect(COMMISSION_RATES_EXPORT.avalpha.local).toBeGreaterThan(
        COMMISSION_RATES_EXPORT.competitor.local
      );
      expect(COMMISSION_RATES_EXPORT.avalpha.foreign).toBeGreaterThan(
        COMMISSION_RATES_EXPORT.competitor.foreign
      );
    });
  });
});
