*** Settings ***
Library           SeleniumLibrary
Suite Setup       Open Browser To Login
Suite Teardown    Close Browser

*** Variables ***
${URL}            http://localhost:7063/login
${BROWSER}        chrome
${VALID_USER}     admin@mail.com
${VALID_PASS}     Admin123
${INVALID_PASS}   WrongPassword

*** Keywords ***
Open Browser To Login
    Open Browser    ${URL}    ${BROWSER}
    Maximize Browser Window
    Wait Until Page Contains Element    xpath=//input[@id='Email']    10s

Input Credentials
    [Arguments]    ${username}    ${password}
    Input Text    xpath=//input[@id='Email']    ${username}
    Input Text    xpath=//input[@id='Password']    ${password}

Click Login Button
    Click Button    xpath=//button[@type='submit']
    Wait Until Page Contains Element    xpath=//div[contains(@class,'alert-danger')]    10s

*** Test Cases ***
Login Valide
    Input Credentials    ${VALID_USER}    ${VALID_PASS}
    Click Login Button
    Wait Until Location Contains    /dashboard    10s

Login Invalide
    Input Credentials    ${VALID_USER}    ${INVALID_PASS}
    Click Login Button
    Page Should Contain Element    xpath=//div[contains(@class,'alert-danger')]
