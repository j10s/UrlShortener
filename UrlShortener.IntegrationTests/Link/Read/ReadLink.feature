Feature: Get Link
As a user of the UrlShortener API
I want to get Links
    
Scenario: Not Found
    When a get link request is made with stub "doesnotexist"
    Then the http status code is 404