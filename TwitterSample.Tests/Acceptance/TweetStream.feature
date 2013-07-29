Feature: TweetStream
	As a user
	I want to see my tweet stream
	So i can keep up to date with my tweets

@integration
Scenario: Can call twitter to retrieve tweets
	Given I have an API key for twitter
	When I call the service with the following Twitter Ids
	| Id            |
	| pay_by_phone  |
	| PayByPhone    |
	| PayByPhone_Uk |
	Then tweets should be returned
