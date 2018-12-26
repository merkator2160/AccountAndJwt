This project demonstrates implementation of base functionality .NET Core 2 WebAPI + JWT Token authorization.
I’m presenting the best practices of attribute routing, named actions, which corresponds to the correct HTTP verbs.

ApplicationUser Model:
*	Extended the IdentityUser class, which can store additional properties of the user model.

Account Controller:
*	Create a user
*	Reset a password
*	Remove a user account
*	Change email address (able to send out a warning)
*	Update other account details such as name
*	Add/remove a role from a user

Token Controller:
*	Issue a JWT Token based upon user/password
*	Issue a JWT Token based upon a refresh token
*	Revoke a JWT token
*	JWT Token to include relevant claims

Startup class:
*	Configuration the JWT Authentication
*	Used the IdentityCore, rather than Identity, so cookies are not set up
*	Dependency injection used
*	Configuration and secrets are used to setup initially parameters

Logging also used, but sensitive data are excluded from logs.

Application structured neatly based on classical 3rd layer architecture, with public methods annotation and self documenting API by Swagger. Unit tests samples are provided, but not with full coverage. Integration test engine also implemented, based on in-memory database and data seeding independently for each test. Integration tests scripts shows that that bearer tokens are working as expected and ValueCondroller properly provide all base CRUD operations with Value entity.
The HTTP response codes are in line with the actions and results, e.g. user created 203 (with a route to the resource and JSON representation).

Based on the materials below:
*	https://stackoverflow.com/questions/46323844/net-core-2-0-web-api-using-jwt-adding-identity-breaks-the-jwt-authentication
*	https://github.com/shawnwildermuth/DualAuthCore
*	http://www.c-sharpcorner.com/article/handle-refresh-token-using-asp-net-core-2-0-and-json-web-token/