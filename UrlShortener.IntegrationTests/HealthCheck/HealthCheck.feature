Feature: HealthCheck
	As a user of the UrlShortener API
	I want to verify the health status of the API
	So that I can check if the API is usable

Scenario: Simple health check
	When a health check request is made
	Then the http status code is 200
	And a healthy response is returned