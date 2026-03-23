Feature: Delete Link
As a user of the UrlShortener API
I want to delete Links
    
Scenario: Success
    Given a link exists with url "https://delete-test.com"
    When a delete link request is made
    Then the http status code is 200
    And the link does not exist
    
Scenario: Not Found
    When a delete link request is made with stub "doesnotexist"
    Then the http status code is 404