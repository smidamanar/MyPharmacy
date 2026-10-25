import pytest
from PlaywrightTests.pages.login_page import LoginPage

def test_login_invalid(page):
    login_page = LoginPage(page)
    login_page.goto()
    login_page.login("admin@mail.com", "WrongPassword")

    # Wait for the error alert to appear
    page.wait_for_selector("div.alert-danger", timeout=60000)
    assert page.is_visible("div.alert-danger")
