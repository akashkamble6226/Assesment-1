import logo from "./logo.png";
import "./App.css";
import { useState } from "react";

// Constants for API and validation
const API_ENDPOINT = "http://localhost:5111/commision";
const MIN_VALID_VALUE = 0;
const COMMISSION_RATES = {
  avalpha: { local: 20, foreign: 35 },
  competitor: { local: 2, foreign: 7.55 },
};

function App() {
  // Form state: tracks user input for sales calculations
  const [formData, setFormData] = useState({
    localSalesCount: "",
    foreignSalesCount: "",
    averageSaleAmount: "",
  });

  // Results state: stores commission calculations from API
  const [results, setResults] = useState({
    avalphaTechnologiesCommission: 0,
    competitorCommission: 0,
  });

  const [isLoading, setIsLoading] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const validateFormData = () => {
    return (
      parseFloat(formData.localSalesCount) > MIN_VALID_VALUE &&
      parseFloat(formData.foreignSalesCount) > MIN_VALID_VALUE &&
      parseFloat(formData.averageSaleAmount) > MIN_VALID_VALUE
    );
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    // Validate input before making API call
    if (!validateFormData()) {
      alert("Please enter positive values for all fields.");
      setIsLoading(false);
      return;
    }

    try {
      // Make API request with validated sales data
      const response = await fetch(API_ENDPOINT, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          localSalesCount: parseFloat(formData.localSalesCount),
          foreignSalesCount: parseFloat(formData.foreignSalesCount),
          averageSaleAmount: parseFloat(formData.averageSaleAmount),
        }),
      });

      // Check if API response is successful
      if (!response.ok) {
        throw new Error("Failed to fetch commission data");
      }

      const data = await response.json();
      console.log("API Response:", data); // Debug log

      // Update results with formatted commission amounts (2 decimal places)
      setResults({
        avalphaTechnologiesCommission:
          data.avalphaTechnologiesCommissionAmount.toFixed(2),
        competitorCommission: data.competitorCommissionAmount.toFixed(2),
      });
    } catch (error) {
      // Log error for debugging and notify user
      console.error("Error:", error);
      alert("Error calculating commission. Please try again.");
    } finally {
      // Always reset loading state regardless of success or failure
      setIsLoading(false);
    }
  };

  return (
    <div className="App">
      <header className="App-header">
        <div className="logo-container">
          <img
            src={logo}
            className="App-logo"
            alt="Avalpha Technologies Logo"
          />
          <h1 className="company-title">Avalpha Technologies</h1>
          <h2 className="app-subtitle">Commission Calculator</h2>
        </div>
      </header>

      <main className="main-content">
        <div className="calculator-container">
          {/* Sales input form section */}
          <div className="form-section">
            <h3>Sales Information</h3>
            <form onSubmit={handleSubmit} className="calculator-form">
              <div className="form-group">
                <label htmlFor="localSalesCount">Local Sales Count</label>
                <input
                  type="number"
                  id="localSalesCount"
                  name="localSalesCount"
                  value={formData.localSalesCount}
                  onChange={handleInputChange}
                  placeholder="Enter number of local sales"
                  required
                  min="1"
                />
              </div>

              <div className="form-group">
                <label htmlFor="foreignSalesCount">Foreign Sales Count</label>
                <input
                  type="number"
                  id="foreignSalesCount"
                  name="foreignSalesCount"
                  value={formData.foreignSalesCount}
                  onChange={handleInputChange}
                  placeholder="Enter number of foreign sales"
                  required
                  min="1"
                />
              </div>

              <div className="form-group">
                <label htmlFor="averageSaleAmount">
                  Average Sale Amount (£)
                </label>
                <input
                  type="number"
                  step="0.01"
                  id="averageSaleAmount"
                  name="averageSaleAmount"
                  value={formData.averageSaleAmount}
                  onChange={handleInputChange}
                  placeholder="Enter average sale amount"
                  required
                  min="0.01"
                />
              </div>

              <button
                type="submit"
                className={`calculate-btn ${isLoading ? "loading" : ""}`}
                disabled={isLoading}
              >
                {isLoading ? "Calculating..." : "Calculate Commission"}
              </button>
            </form>
          </div>

          {/* Commission results display section */}
          <div className="results-section">
            <h3>Commission Results</h3>
            <div className="results-grid">
              {/* Avalpha Technologies commission card */}
              <div className="result-card avalpha-card">
                <div className="result-header">
                  <h4>Avalpha Technologies</h4>
                  <span className="commission-rates">
                    Local: {COMMISSION_RATES.avalpha.local}% | Foreign:{" "}
                    {COMMISSION_RATES.avalpha.foreign}%
                  </span>
                </div>
                <div className="result-amount">
                  £{results.avalphaTechnologiesCommission}
                </div>
              </div>

              {/* Competitor commission card for comparison */}
              <div className="result-card competitor-card">
                <div className="result-header">
                  <h4>Competitor</h4>
                  <span className="commission-rates">
                    Local: {COMMISSION_RATES.competitor.local}% | Foreign:{" "}
                    {COMMISSION_RATES.competitor.foreign}%
                  </span>
                </div>
                <div className="result-amount">
                  £{results.competitorCommission}
                </div>
              </div>
            </div>

            {/* Display competitive advantage only when results are available */}
            {results.avalphaTechnologiesCommission > 0 && (
              <div className="advantage-indicator">
                <p className="advantage-text">
                  Avalpha Technologies advantage:
                  <strong>
                    {" "}
                    £
                    {(
                      results.avalphaTechnologiesCommission -
                      results.competitorCommission
                    ).toFixed(2)}
                  </strong>
                </p>
              </div>
            )}
          </div>
        </div>
      </main>

      <footer className="App-footer">
        <p>&copy; 2025 Avalpha Technologies. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default App;
