from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

class MedicationsPage:
    def __init__(self, driver):
        self.driver = driver
        self.url = "https://localhost:7063/medications"

        # Example: table of medications
        self.table = (By.CSS_SELECTOR, "table")  # adjust selector to actual table

    def open(self):
        self.driver.get(self.url)

    def page_loaded(self):
        wait = WebDriverWait(self.driver, 10)
        return wait.until(EC.visibility_of_element_located(self.table)) is not None
