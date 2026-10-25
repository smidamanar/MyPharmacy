*** Settings ***
Library    SeleniumLibrary

*** Keywords ***
Ajouter Nouveau Patient
    [Arguments]    ${fullname}  ${nationalid}  ${birthdate}  ${address}
    Click Button    css=button.btn-primary
    Input Text      css=input[placeholder="Full Name"]    ${fullname}
    Input Text      css=input[placeholder="National ID"]  ${nationalid}
    Input Text      css=input[type="date"]                ${birthdate}
    Input Text      css=input[placeholder="Address"]     ${address}
    Click Button    css=button.btn-success
    Wait Until Page Contains    ${fullname}

Verifier Patient Dans Liste
    [Arguments]    ${fullname}
    Wait Until Page Contains    ${fullname}
