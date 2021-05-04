This project contains the ubercontext database schema which has the responsibility to create and modify the database
for the backend application.

The main(backend) db context is contained within the **UberContext.cs** class in the **DataModel** folder.

CMS db context is exception here since we used ASPIdentity within the CMS project and it has lot of abstract
concepts which was difficult to change and make its db context non-initializer, but the theory here is that
we should have only one db initializer which is the UberContext