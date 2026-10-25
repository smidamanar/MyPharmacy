from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

class LoginPage:
    def __init__(self, driver):
        self.driver = driver
        self.url = "https://localhost:7063/login"

        # Locators (Blazor may generate dynamic IDs, using placeholder for stability)
        self.email = (By.CSS_SELECTOR, "input[placeholder='Enter your email']")
        self.password = (By.CSS_SELECTOR, "input[placeholder='Enter your password']")
        self.submit = (By.CSS_SELECTOR, "button[type='submit']")

    def open(self):
        self.driver.get(self.url)

    def login(self, email, password):
        wait = WebDriverWait(self.driver, 10)
        wait.until(EC.visibility_of_element_located(self.email)).send_keys(email)
        wait.until(EC.visibility_of_element_located(self.password)).send_keys(password)
        wait.until(EC.element_to_be_clickable(self.submit)).click()

    def is_loaded(self):
        wait = WebDriverWait(self.driver, 10)
        return wait.until(EC.visibility_of_element_located(self.email)) is not None
