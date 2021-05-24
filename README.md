# MMT Api specification

## Recommendations

If I had more time the changes I might make;

1. ## Specifications:  
    Api should be protected by either a Json web token JWT or two factor authentication to prevent unathourised access.
    Specification should explicitly require handling errors gracefully and return standard error codes.
2. ## Implementation:  
   Implementation of api should be fully restful i.e Use nouns instead of verbs in endpoint paths, allow for filtering and etc.
   Api should also allow for version. This allows the api to evolve with the business.

## Before code can be deployed to production environment:

1.  I will remove Authentication token from appsettings.Production.json file and save it production environment variable or an azure vault
2.  I will also remove the hard coded connection string and save it in an azure vault or production environment variable
