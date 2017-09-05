# identity-provider-example

### This repository demostrates setting up a simple authorization server with users, scopes, and clients stored in memory that will provide a JWT token upon sending the correct credentials. Included in the solution is a Resource.api project that can be used to consume the generated token. 

## Recommended tools:
* OpenSSL (http://slproweb.com/products/Win32OpenSSL.html)
* Postman (Google app) for api and token testing

## Setup
* Create a public/private key pair using OpenSSL (helper instructions: https://developer.xero.com/documentation/api-guides/create-publicprivate-key) Steps shown below
    * openssl genrsa -out [Private_Key_Name].pem 2048
    * openssl req -new -x509 -key [Private_Key_Name].pem -out [Public_Key_Name].cer -days [Number_Of_Valid_Days]
    * openssl pkcs12 -export -out [PFX_Key_Name].pfx -inkey [Private_Key_Name].pem -in [Public_Key_Name].cer
* Set the SecurityKeyPath app setting in the IDP project to the full path for the .pfx file
* Set the SecurityKeyPassword app setting in the IDP project to the password created during the pkcs12 export step
* Set the Authority app setting in the Resource.api project to the specific localhost and port running the IDP application
* Using Postman, navigate to the IDP localhost:<port>/connect/token
* Copy the token returned and use it as the Authorization when requesting a url from the Resource.api
   
## Postman Setup
To properly utilize Postman to retrieve a valid token, the following x-www-form-urlencoded values will need to be set:
* client_id: this will match the client id specified in InMemoryManager.GetClients()
* client_secret: this will match one or more of the ClientSecrets in InMemoryManager.GetClients()
* scope: typically openid, but will match one of the scopes defined in InMemoryManager.GetScopes()
* grant_type: password
* username: must match one of the defined InMemoryUser usernames in InMemoryManager.GetUsers()
* password: must match one of the defined InMemoryUser passwords in InMemoryManager.GetUsers()
