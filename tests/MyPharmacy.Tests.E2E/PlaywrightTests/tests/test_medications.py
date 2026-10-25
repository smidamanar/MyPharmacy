import pytest
from PlaywrightTests.pages.medications_page import MedicationsPage

def test_medications(page, screenshot):
    meds = MedicationsPage(page)
    meds.goto()
    screenshot("medications")
    assert meds.is_loaded()
