import time
import pytest
from SeleniumTests.pages.LoginPage import LoginPage

def test_login(driver, screenshot):
    login = LoginPage(driver)
    login.open()
    assert login.is_loaded(), "Login page did not load"

    # Perform login
    login.login("admin@mail.com", "Admin123")

    # Wait to visually capture Dashboard redirect
    time.sleep(2)
