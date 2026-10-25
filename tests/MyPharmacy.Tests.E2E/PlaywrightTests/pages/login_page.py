from playwright.sync_api import Page

class LoginPage:
    def __init__(self, page: Page, base_url: str = "https://localhost:7063"):
        self.page = page
        self.base_url = base_url
        # Selectors using placeholders
        self.email_input = "input[placeholder='Enter your email']"
        self.password_input = "input[placeholder='Enter your password']"
        self.login_button = "button:has-text('Login')"

    def goto(self):
        self.page.goto(f"{self.base_url}/login")
        # Wait for the email input to be visible
        self.page.wait_for_selector(self.email_input, timeout=60000)

    def login(self, email: str, password: str):
        self.page.fill(self.email_input, email)
        self.page.fill(self.password_input, password)
        self.page.click(self.login_button)
        # Wait for either dashboard redirect or error alert
        self.page.wait_for_load_state("networkidle")
