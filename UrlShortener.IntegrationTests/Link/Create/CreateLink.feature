Feature: Create Link
    As a user of the UrlShortener API
    I want to create Links
    
Scenario: Success
    Given a url of "https://create-test.com"
    When a create link request is made
    Then the http status code is 201
    And a link exists with url "https://create-test.com"
    
Scenario: Empty String Validation Failure
    Given a url of ""
    When a create link request is made
    Then the http status code is 400
    And the following errors are returned:
        | Field     | Error Message                    |
        | TargetUri | The TargetUri field is required. |

Scenario: Non Url Validation Failure
    Given a url of "not a url"
    When a create link request is made
    Then the http status code is 400
    And the following errors are returned:
        | Field     | Error Message                                                               |
        | TargetUri | The TargetUri field is not a valid fully-qualified http, https, or ftp URL. |