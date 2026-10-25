import pytest
from playwright.sync_api import sync_playwright

@pytest.fixture(scope="session")
def browser():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=False)
        yield browser
        browser.close()

@pytest.fixture
def page(browser):
    context = browser.new_context()
    page = context.new_page()
    yield page
    context.close()

@pytest.fixture
def screenshot(page):
    def _screenshot(name):
        import os
        if not os.path.exists("screenshots"):
            os.makedirs("screenshots")
        page.screenshot(path=f"screenshots/{name}.png")
    return _screenshot
