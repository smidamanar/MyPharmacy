*** Settings ***
Library           SeleniumLibrary
Suite Setup       Open Browser To Patients
Suite Teardown    Close Browser

*** Variables ***
${URL}            http://localhost:7063/patients
${BROWSER}        chrome
${VALID_USER}     admin@mail.com
${VALID_PASS}     Admin123
${NEW_PATIENT_NAME}     Test Patient
${NEW_PATIENT_ID}       12345678
${NEW_PATIENT_BIRTH}    2000-01-01
${NEW_PATIENT_ADDR}     123 Rue Test

*** Keywords ***
Open Browser To Patients
    Open Browser    http://localhost:7063/login    ${BROWSER}
    Maximize Browser Window
    Wait Until Page Contains Element    xpath=//input[@id='Email']    10s
    Input Text    xpath=//input[@id='Email']    ${VALID_USER}
    Input Text    xpath=//input[@id='Password']    ${VALID_PASS}
    Click Button    xpath=//button[@type='submit']
    Wait Until Location Contains    /dashboard    10s
    Go To    ${URL}
    Wait Until Page Contains Element    xpath=//button[contains(.,'Add New Patient')]    10s

Add Patient
    Click Button    xpath=//button[contains(.,'Add New Patient')]
    Wait Until Page Contains Element    xpath=//input[@id='FullName']    10s
    Input Text    xpath=//input[@id='FullName']     ${NEW_PATIENT_NAME}
    Input Text    xpath=//input[@id='NationalId']   ${NEW_PATIENT_ID}
    Input Text    xpath=//input[@BirthDate']       ${NEW_PATIENT_BIRTH}
    Input Text    xpath=//input[@Address']         ${NEW_PATIENT_ADDR}
    Click Button    xpath=//button[contains(.,'Save')]
    Wait Until Page Contains    ${NEW_PATIENT_NAME}    10s

*** Test Cases ***
Ajouter Patient Et Vérifier Liste
    Add Patient
    Page Should Contain    ${NEW_PATIENT_NAME}
