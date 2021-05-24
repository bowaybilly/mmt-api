# MMT Api specification

## Recommendations

If I had more time the changes I might make;

1. Specifications:  
   I will allow specification to provide for unit testing the code.
2. Implementation:  
   I will use any of the design patterns to implement the way I query data from the Customer Api. I will use the Repository pattern
   as I feel the database is to close to the Api. The layer of abstraction will give me more control in unit testing as wll.

## Before code can be deployed to production environment:

1.  I will remove Authentication token from application.Production.json file and save it production environment variable or an azure vault
2.  I will also remove the hard coded connection string and save it in an azure vault or production environment variable
