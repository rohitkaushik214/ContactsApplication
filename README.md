# ContactsApplication
Following technologies and frameworks were used in the project:
- WebApi2
- .Net framework 4.5.2
- Autofac for IOC container
- log4net for logging
- Entity framework 6 as ORM
- Microsoft.Owin.Security.Oauth for authentication


Endpoints:
	 
#1. For registering a new user:

	POST: {BASE_URL}/api/Account/Register
	Sample request body: 
	{
		"UserName": "",
		"Password": "",
		"Name": ""
	}
	
#2. For generating a token:

	POST: {BASE_URL}/ token   
	Request body should contain following parameters as x-www-form-urlencoded
	 - grant_type: password
	 - username: {user_name}
	 - password: {password} 
	 
#3. For adding a new contact:

	POST: {BASE_URL}/api/Contacts/AddContact
	Sample request body: 
	{
		"Email": "",
		"FirstName": "",
		"LastName": "",
		"PhoneNumber":""
	}

#4. For getting a contact with ID:

	GET: {BASE_URL}/api/Contacts/GetContact/{id}
	
	
#5. For getting all contacts: (Max. 200 contacts will be returned and if there are more contacts the reponse will contain a "hasMore" flag set to true and a offset which you can send in the next request)

	GET: {BASE_URL}/api/Contacts/GetAllContacts/{offset?}
	
	
#6. For marking a contact as inactive:

	GET: {BASE_URL}/api/Contacts/Inactive/{contactId}
	
#7. For marking a contact as active:

	GET: {BASE_URL}/api/Contacts/Active/{contactId}
	
#8. For editing a contact:

	POST: {BASE_URL}/api/Contacts/edit/{contactId}
	Sample request body: 
	{
		"Email": "",
		"FirstName": "",
		"LastName": "",
		"PhoneNumber":""
	}
	
**Note: All the calls except register call requires token to be passed along in the headers as follows:**

Authorization - Bearer {token}


**Project structure:**

#1. ContactsApplication:

Contains a Startup.cs file as I have used OWIN for hosting. I have configured IOC container(Autofac) for injecting dependencies. Also, oAUth is configured here.

**/Controllers** contains controllers used in the application.

**/Models** contains the request models used in the application

**/Infrastructure** contains the authentication implementation and a log injection module via log4net library.


#2. ContactApplication.Domain:

Contains the .edmx  file created via entity framework code first approach.

#3. ContactsApplication.Repository:

Contains the DTOs and repositories used in the application with their interfaces.






	
	
