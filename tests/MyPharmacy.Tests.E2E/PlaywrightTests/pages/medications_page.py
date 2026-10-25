class MedicationsPage:
    def __init__(self, page, base_url="https://localhost:7063"):
        self.page = page
        self.base_url = base_url
        self.page_title = "h3"

    def goto(self):
        self.page.goto(f"{self.base_url}/medications")
        self.page.wait_for_selector(self.page_title, timeout=60000)

    def is_loaded(self):
        return self.page.is_visible(self.page_title)
