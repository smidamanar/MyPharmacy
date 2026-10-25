import time
import pytest
from SeleniumTests.pages.MedicationsPage import MedicationsPage

def test_medications(driver, screenshot):
    meds = MedicationsPage(driver)
    meds.open()
    assert meds.page_loaded(), "Medications page did not load"
    time.sleep(1)  # allow visual capture
