import pytest
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
import os

@pytest.fixture(scope="session")
def driver():
    # Configure Chrome options
    chrome_options = Options()
    chrome_options.add_argument("--start-maximized")
    chrome_options.add_argument("--ignore-certificate-errors")
    chrome_options.add_argument("--disable-extensions")
    chrome_options.add_experimental_option("excludeSwitches", ["enable-logging"])

    # Path to chromedriver if needed, else ensure chromedriver is in PATH
    service = Service()

    driver = webdriver.Chrome(service=service, options=chrome_options)
    yield driver
    driver.quit()

@pytest.fixture(scope="function")
def screenshot(request, driver):
    """Automatically take screenshot if test fails."""
    yield
    test_name = request.node.name
    screenshots_dir = os.path.join(os.getcwd(), "screenshots")
    os.makedirs(screenshots_dir, exist_ok=True)
    screenshot_file = os.path.join(screenshots_dir, f"{test_name}.png")
    driver.save_screenshot(screenshot_file)
