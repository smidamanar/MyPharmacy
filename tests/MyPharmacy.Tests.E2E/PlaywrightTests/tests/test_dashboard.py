import pytest
from PlaywrightTests.pages.dashboard_page import DashboardPage

def test_dashboard(page, screenshot):
    dashboard = DashboardPage(page)
    dashboard.goto()
    screenshot("dashboard")
    assert dashboard.is_loaded()
