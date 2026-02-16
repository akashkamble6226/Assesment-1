// Commission calculation logic extracted for testability and reusability
// This module handles all commission calculations with clear, documented logic

/**
 * Commission rates for Avalpha Technologies and Competitor
 * Based on sales type: Local vs Foreign
 */
const COMMISSION_RATES = {
  avalpha: {
    local: 0.20,      // 20% for local sales
    foreign: 0.35,    // 35% for foreign sales
  },
  competitor: {
    local: 0.02,      // 2% for local sales
    foreign: 0.0755,  // 7.55% for foreign sales
  },
};

/**
 * Calculate commission for a specific sales type
 * @param {number} salesCount - Number of sales
 * @param {number} averageSaleAmount - Average value per sale
 * @param {number} rate - Commission rate (as decimal, e.g., 0.20 for 20%)
 * @returns {number} Total commission for this sales type
 */
const calculateCommissionForSalesType = (salesCount, averageSaleAmount, rate) => {
  return rate * salesCount * averageSaleAmount;
};

/**
 * Calculate total commission for a company
 * @param {number} localSalesCount - Number of local sales
 * @param {number} foreignSalesCount - Number of foreign sales
 * @param {number} averageSaleAmount - Average value per sale
 * @param {object} rates - Commission rates object with local and foreign properties
 * @returns {number} Total commission
 */
const calculateTotalCommission = (
  localSalesCount,
  foreignSalesCount,
  averageSaleAmount,
  rates
) => {
  const localCommission = calculateCommissionForSalesType(
    localSalesCount,
    averageSaleAmount,
    rates.local
  );
  
  const foreignCommission = calculateCommissionForSalesType(
    foreignSalesCount,
    averageSaleAmount,
    rates.foreign
  );
  
  return localCommission + foreignCommission;
};

/**
 * Calculate commissions for both Avalpha and Competitor
 * @param {number} localSalesCount - Number of local sales
 * @param {number} foreignSalesCount - Number of foreign sales
 * @param {number} averageSaleAmount - Average value per sale
 * @returns {object} Object containing avalphaTechnologies and competitor commission amounts
 */
export const calculateCommissions = (
  localSalesCount,
  foreignSalesCount,
  averageSaleAmount
) => {
  // Validate inputs
  if (
    typeof localSalesCount !== "number" ||
    typeof foreignSalesCount !== "number" ||
    typeof averageSaleAmount !== "number"
  ) {
    throw new Error("All inputs must be numbers");
  }

  if (
    localSalesCount < 0 ||
    foreignSalesCount < 0 ||
    averageSaleAmount < 0
  ) {
    throw new Error("All inputs must be non-negative");
  }

  const avalphaTechnologiesCommission = calculateTotalCommission(
    localSalesCount,
    foreignSalesCount,
    averageSaleAmount,
    COMMISSION_RATES.avalpha
  );

  const competitorCommission = calculateTotalCommission(
    localSalesCount,
    foreignSalesCount,
    averageSaleAmount,
    COMMISSION_RATES.competitor
  );

  return {
    avalphaTechnologiesCommissionAmount: avalphaTechnologiesCommission,
    competitorCommissionAmount: competitorCommission,
  };
};

/**
 * Format large numbers with abbreviations (K, M, B, T)
 * Improves readability when dealing with very large commission values
 * @param {number} num - Number to format
 * @returns {string} Formatted number with abbreviation
 */
export const formatLargeNumber = (num) => {
  const parsedNum = parseFloat(num);
  const absNum = Math.abs(parsedNum);

  // 1e12 = 1,000,000,000,000 (1 trillion) → abbreviates as T
  if (absNum >= 1e12) {
    return (parsedNum / 1e12).toFixed(2) + "T";
  }
  // 1e9 = 1,000,000,000 (1 billion) → abbreviates as B
  else if (absNum >= 1e9) {
    return (parsedNum / 1e9).toFixed(2) + "B";
  }
  // 1e6 = 1,000,000 (1 million) → abbreviates as M
  else if (absNum >= 1e6) {
    return (parsedNum / 1e6).toFixed(2) + "M";
  }
  // 1e3 = 1,000 (1 thousand) → abbreviates as K
  else if (absNum >= 1e3) {
    return (parsedNum / 1e3).toFixed(2) + "K";
  }
  // Return as is with 2 decimal places for numbers less than 1000
  return parsedNum.toFixed(2);
};

export const COMMISSION_RATES_EXPORT = COMMISSION_RATES;
