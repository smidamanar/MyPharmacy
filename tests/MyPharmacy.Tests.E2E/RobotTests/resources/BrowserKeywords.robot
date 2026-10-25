*** Settings ***
Library           SeleniumLibrary

*** Keywords ***
Ajouter Nouveau Patient
    [Arguments]    ${fullname}  ${nationalid}  ${birthdate}  ${address}
    Click Button    button:contains("Add New Patient")
    Input Text      input[placeholder="Full Name"]    ${fullname}
    Input Text      input[placeholder="National ID"]  ${nationalid}
    Input Text      input[type="date"]                ${birthdate}
    Input Text      input[placeholder="Address"]     ${address}
    Click Button    button:contains("Save")
    Wait Until Page Contains    ${fullname}
