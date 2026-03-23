Feature: Update Link
As a user of the UrlShortener API
I want to update Links
    
Scenario: Success
    Given a link exists with url "https://update-test.com"
    When an update link request is made with url "https://update-test2.com"
    Then the http status code is 200
    And the link has url "https://update-test2.com"
    
Scenario: Not Found
    When an update link request is made with stub "doesnotexist" and url "https://not-found.com"
    Then the http status code is 404
    
Scenario: Empty String Validation Failure
    Given a link exists with url "https://update-test3.com"
    When an update link request is made with url ""
    Then the http status code is 400
    And the following errors are returned:
      | Field     | Error Message                    |
      | TargetUri | The TargetUri field is required. |

Scenario: Non Url Validation Failure
    Given a link exists with url "https://update-test4.com"
    When an update link request is made with url "not a url"
    Then the http status code is 400
    And the following errors are returned:
      | Field     | Error Message                                                               |
      | TargetUri | The TargetUri field is not a valid fully-qualified http, https, or ftp URL. |