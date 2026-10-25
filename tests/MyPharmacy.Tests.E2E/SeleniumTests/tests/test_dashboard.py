import time
import pytest
from SeleniumTests.pages.DashboardPage import DashboardPage

def test_dashboard(driver, screenshot):
    dashboard = DashboardPage(driver)
    dashboard.open()
    assert dashboard.is_loaded(), "Dashboard page did not load"
    time.sleep(1)  # allow visual capture
