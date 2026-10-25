import time
import pytest
from SeleniumTests.pages.LoginPage import LoginPage
from SeleniumTests.pages.DashboardPage import DashboardPage
from SeleniumTests.pages.MedicationsPage import MedicationsPage

def test_capture_all_pages(driver, screenshot):
    # Login
    login = LoginPage(driver)
    login.open()
    assert login.is_loaded(), "Login page did not load"
    login.login("admin@mail.com", "Admin123")
    time.sleep(2)

    # Dashboard
    dashboard = DashboardPage(driver)
    assert dashboard.is_loaded(), "Dashboard did not load"
    time.sleep(1)

    # Medications
    meds = MedicationsPage(driver)
    meds.open()
    assert meds.page_loaded(), "Medications page did not load"
    time.sleep(1)
