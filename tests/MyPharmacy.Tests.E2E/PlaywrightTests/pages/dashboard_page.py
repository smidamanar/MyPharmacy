class DashboardPage:
    def __init__(self, page, base_url="https://localhost:7063"):
        self.page = page
        self.base_url = base_url
        self.dashboard_header = "h2.display-5"

    def goto(self):
        self.page.goto(f"{self.base_url}/dashboard")
        self.page.wait_for_selector(self.dashboard_header, timeout=60000)

    def is_loaded(self):
        return self.page.is_visible(self.dashboard_header)
