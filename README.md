# RolesWithoutIdentity
----------------------------------------------------------------------------------------------------------------------------------------
Summary:

	In ASP.NET Core MVC, authorization is the process of ensuring that a user has the necessary permissions
		to perform a specific action.

	The framework provides a lot of built-in functionality for authorization, such as policies and requirements,
		but sometimes you need to implement custom authorization logic that goes beyond the built-in functionality.

	This is where the custom authorization handler comes in.

	In this demo, the CustomAuthorization class is a custom authorization handler that extends
		the AuthorizationHandler class and implements the HandleRequirementAsync method.

	This method contains the custom authorization logic that determines whether the user has the required roles
		to perform the requested action.

	The HandleRequirementAsync method receives two parameters:
		the context parameter, which contains information about the user and the requested action,
		and the requirement parameter, which contains the required roles.

	The method first checks whether the user has a specific claim
		(in this case, a claim of type ClaimTypes.Name and an issuer of "AD AUTHORITY") that identifies the user.
	If the user doesn't have this claim, the method returns Task.FromResult(0).

	If the user has the required claim, the method extracts the user's username from the claim,
		and then checks whether the user has any of the required roles.

	The method loops through the roles and checks whether the user has a role that matches the role name.

	If the user has a matching role, the method calls the Succeed method on the context object
		to indicate that the authorization was successful, and then returns Task.FromResult(0).

	If the user doesn't have any of the required roles, the method simply returns Task.FromResult(0)
		without calling Fail or Succeed on the context object.
	This means that the authorization has failed, and the user will be redirected to the Error action
		in the HomeController class.

	Overall, the CustomAuthorization class provides a way to implement custom authorization logic in ASP.NET Core MVC,
		and it can be used to enforce any type of authorization policy that
		can't be achieved with the built-in authorization features of the framework.

----------------------------------------------------------------------------------------------------------------------------------------

Required files:

	CustomAuthorization.cs
	RoleRequirement.cs

Required modifications:

	Program.cs:
		builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

		builder.Services.AddAuthorization(opt =>
		{
			opt.AddPolicy("Admin", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN" })));
			opt.AddPolicy("Poweruser", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN", "POWERUSER" })));
			opt.AddPolicy("User", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN", "POWERUSER", "USER" })));
		});

		//needs to be scoped, otherwise DI of DbContext doesn't work in customauthorization
		builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorization>();

		app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

	HomeController.cs:
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                var viewName = statusCode.ToString();
                return View("~/Views/Error/" + viewName + ".cshtml");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

----------------------------------------------------------------------------------------------------------------------------------------
	
Suggestions:

	Name error views by their status code (i.e. "401.cshtml", "500.cshtml") and place them
		in Views/Error

A working implementation of this demo can be found in \\mkdev01\repo.git\ps\IncomingShipmentMonitor

----------------------------------------------------------------------------------------------------------------------------------------