Thanks for applying for this project, which is to write some of the base functionality for a .net core 2 web api.
What I’m looking for is as below, and should use the best practices of attribute routing, named actions and correspond to the correct HTTP verbs.

ApplicationUser Model:
•	Extend the IdentityUser class so can add additional properties to the user model

Account Controller (making use of the Identity package):
•	Create a user
•	Reset a password
•	Remove a user account
•	Change email address (which should send out a warning)
•	Update other account details such as name
•	Add/remove a role from a user

Token Controller (making use of the Identity package):
•	Issue a JWT Token based upon user/password
•	Issue a JWT Token based upon a refresh token
•	Revoke a JWT token
•	JWT Token to include relevant claims

Startup:
•	Setup the JWT Authentication
•	Use the IdentityCore rather than Identity so as cookies are not set
•	Dependency injection correctly used
•	Configuration and secrets used to setup parameters

Logging should also be used, with sensitive data not logged.

This should be structured neatly, with comments and unit tests within a working web api solution using seeded data and the in-memory database. 
Another controller can be used for testing to show that bearer tokens are working as expected.
The HTTP response codes should be in line with the actions and results, e.g. user created 203 (with a route to the resource and the json representation).

Reference reading:
•	https://stackoverflow.com/questions/46323844/net-core-2-0-web-api-using-jwt-adding-identity-breaks-the-jwt-authentication
•	https://github.com/shawnwildermuth/DualAuthCore
•	http://www.c-sharpcorner.com/article/handle-refresh-token-using-asp-net-core-2-0-and-json-web-token/