import { render, screen, fireEvent } from "@testing-library/react";
import App from "./App";

test("renders commission calculator heading", () => {
  render(<App />);
  expect(screen.getByText("Commission Calculator")).toBeInTheDocument();
});

test("renders all form input fields", () => {
  render(<App />);
  expect(screen.getByLabelText(/Local Sales Count/i)).toBeInTheDocument();
  expect(screen.getByLabelText(/Foreign Sales Count/i)).toBeInTheDocument();
  expect(screen.getByLabelText(/Average Sale Amount/i)).toBeInTheDocument();
});

test("renders calculate commission button", () => {
  render(<App />);
  expect(
    screen.getByRole("button", { name: /Calculate Commission/i }),
  ).toBeInTheDocument();
});
