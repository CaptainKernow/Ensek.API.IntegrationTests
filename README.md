Requested Scenarios covered:

"Reset the test data" - I would expect this to normally done as part of a [Before Run] or similar, however it is encorporated into the "Bought quatities of fuel can then be returned" scenario as a Given/prereq. See below for reset issue.
"Buy a quantity of each fuel" - Again there maybe call to add groups of test data as part of prerequirement step so here I have bought 3 types fuel in a single step (the order PUT being not implemented) during the "Bought quatities of fuel can then be returned" scenario, then verfied each order that was made.
"Verify each order from the previous step" - I didn't have enough time to pull the values from the message string in the buy response, but I would recommend that the relavent data be returned as their own properties and value alongside the message. I have left some comment in the code around this, but this is also fulfilled by the "Bought quatities of fuel can then be returned" Scenario.
"Confirm how many orders were created before today" - I wasn't completely sure how to put this in proper test context however I did cover it separately in the "Orders made before todays date are returned" Scenario

I initially outlined some, (but not every) other test that I thought it would be practical to do, however I feel I run out of time. There is one additional test in the form of "Ordering more fuel than is available returns an error". This not only tests that Nuclear Fuel can't be purchased past a quantity of 0, but highlights the fact the /energy response probably should be returned in an [ ] array, but is not.


List of issues found

PUT /ENSEK/buy/{id}/{quantity}
Some energy have the quantities and the remaining units the wrong way around in response message, but values themselves seem correct
Cost is being calculated incorrectly according to what is represented in the /energy RS (per 10 units, not per 1 unit, oil is doubled?)
Use of a set number of decimal places/sig fig for the price given in the response, should be used/should match energy response
Able to order from a negative number of units remaining for OIL. Nuclear (0 units) prevents purchase so you would expect a negative value to be the same, or it to be prevented from getting to the point where a negative is returned at all
Ordering a negative amount gives a 404, I would probably expect a 400
Values in the message string could stand to be returned as their own properties and value alongside the message

GET /ENSEK/energy
This would be eeasier to porcess is the energy types are returned in an [ ] array response?
Values don't match the returned calculated cost in the buy message, see above

POST /ENSEK/login
I was able to successfully post additional unrelated properties and values alongside the UN and PW as long as they were valid. in the past I have been used to some sort of validation on payload objects

DELETE /ENSEK/orders/{orderId}
Currently giving a 500 response

PUT /ENSEK/orders/{orderId}
Currently giving a 500 response

GET /ENSEK/orders/{orderId}
Currently giving a 500 response

POST /ENSEK/reset
retunrs a 401 despite valid auth token








