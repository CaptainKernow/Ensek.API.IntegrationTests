Feature: ApiTests

All test currently in one feature for viewability, but would usually split out into more defined feature file 

#@Login
#Scenario: A Bearer Access Token can be obtained 
#	Given I am not logged in
#	When I post a login request as a 'test' user
#	Then I recieve a Bearer Access Token

@Orders
Scenario: Bought quatities of fuel can then be returned
	Given the data has been reset
	When I order <quantity> of <fuelType> fuel
	Then a successful response response shows fuel remaing, quatity bought and ID
	And that order can then be returned
	| quantity | fuelType		|
	| 10       | electricity	|
	| 20       | gas			|
	| 40       | oil			|

Scenario: Orders made before todays date are returned
	Given the data has been reset
	When I order '10' units of 'gas' fuel
	Then that order is returned

Scenario: Individual fuel orders can be deleted
	Given I have placed an order for '10' units of 'gas'
	When I delete that order
	Then the order is removed from the system

Scenario: Ordering more fuel than is available retunrs an error
	Given the returned available quantity of 'nuclear' fuel is '0'
	When I try to order '20' units of 'nuclear' fuel
	Then I recieve an error message

Scenario: Ordering fuel with a negative quantity returns an error
	Given the returned quatity of 'gas' fuel is positive
	When I try to order '-10' units of 'gas' fuel
	Then I recieve an error message

