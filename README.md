## Project Name
JSON Web Tokens (JWT) Authentication System in .Net Core 6.

## Project Description
The JWT token authentication system in .NET Core 6 is a secure and reliable way to authenticate users for web-based applications. JWT, or JSON Web Tokens, is a popular industry-standard for securely transmitting information between parties as a JSON object. 

This authentication system uses JWT tokens to validate user identity and provide access to resources within the application. It supports both user authentication and authorization, allowing developers to implement fine-grained access control to various parts of the application.

The system is built on top of .NET Core 6, a fast and modular platform that allows for easy scaling and deployment. The project also incorporates industry-standard libraries for authentication, such as Microsoft Identity, to ensure maximum security and compatibility with existing systems.

Some of the features of the JWT token authentication system in .NET Core 6 include:

- Secure and reliable authentication and authorization of users.
- Easy integration with existing web applications built on .NET Core 6.
- Support username and password authentication.
- Support for fine-grained access control to resources within the application.
- Simple and flexible API for authentication and token generation.

Overall, the JWT token authentication system in .NET Core 6 is a robust and flexible solution for authentication and authorization in web-based applications. It provides a secure and reliable way to protect user data and resources while also being easy to integrate and customize for specific use cases.

## Getting Started

- Clone the Repository
- In “appsetting.json” File, 
![image](https://user-images.githubusercontent.com/98965203/233771473-093e4477-17c7-4cbf-a294-211b654f1a94.png)

 
  -	Change the Server_Name.
  -	Change the Database_Name.
  -	Change the AccessTokenSecret
  - Change the RefreshTokenSecret

Note:- Do the other changes if required.

- Change/Add the Roles according to the project needs in Helper Class within Helper Folder.
![image](https://user-images.githubusercontent.com/98965203/233771745-46412577-6fc5-41ec-b280-c2cba3c6aa0a.png)

 
-Change/Add the Claims Passed with the token as per the Project needs in “AccessTokenGenerator” class within “TokenGenerators” folder in Services Folder.

![image](https://user-images.githubusercontent.com/98965203/233771755-1d12a67e-2355-47f2-88e6-d6d80e43d371.png)


- Open Package manager console and run the command “update-database”.
## Usage
-	Create Initial Roles  
  ![image](https://user-images.githubusercontent.com/98965203/233771774-51c7a857-f84b-4691-9ddf-ba52229cf2c6.png)

-	Register User
  ![image](https://user-images.githubusercontent.com/98965203/233771782-728a145b-64bd-4630-bddc-e47a964538a1.png)

-	Login
  ![image](https://user-images.githubusercontent.com/98965203/233771784-3d04032f-8341-46cf-9377-c33e7cc6a503.png)

- Logout
 ![image](https://user-images.githubusercontent.com/98965203/233771793-274721ae-f697-4d44-93e4-0d11964a3aee.png)


## Technologies Used

- .Net Core 6
- SQL Server 


## Conclusion

This Repository can act like a plug and play system, you just need to perform the steps mentioned in the “Getting Stated” Section and you JWT Token Authentication System will ready.
