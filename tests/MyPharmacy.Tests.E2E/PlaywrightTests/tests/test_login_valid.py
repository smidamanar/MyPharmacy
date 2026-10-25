import pytest
from PlaywrightTests.pages.login_page import LoginPage

def test_login_valid(page):
    login_page = LoginPage(page)
    login_page.goto()
    login_page.login("admin@mail.com", "Admin123")

    # Wait until the URL contains /dashboard
    page.wait_for_url("**/dashboard", timeout=60000)
    assert "/dashboard" in page.url
