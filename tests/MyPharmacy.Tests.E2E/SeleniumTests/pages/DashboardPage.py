# pages/DashboardPage.py
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

class DashboardPage:
    def __init__(self, driver, base_url="https://localhost:7063"):
        self.driver = driver
        self.base_url = base_url
        self.wait = WebDriverWait(driver, 10)

    def open(self):
        self.driver.get(f"{self.base_url}/dashboard")

    def wait_for_dashboard(self):
        # Wait for at least one stat card to be visible
        self.wait.until(EC.visibility_of_element_located((By.CSS_SELECTOR, ".dashboard-stat-card")))

    def is_loaded(self):
        try:
            self.wait_for_dashboard()
            return True
        except:
            return False

    def capture_screenshot(self, filename="dashboard.png"):
        self.wait_for_dashboard()
        self.driver.save_screenshot(filename)

    def get_total_patients(self):
        self.wait_for_dashboard()
        elem = self.driver.find_element(By.CSS_SELECTOR, ".dashboard-stat-card .dashboard-number")
        return elem.text
