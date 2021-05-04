The **UberContextShould** test class is responsible for **creating the backend database**.
You have to run the **BuildModel** test which is defined within the mentioned test class to create the database.

Database name and connection string area also defined within the **App.config** file in the root of this project,
And they should be same with the backend project config*(WebApplication.API)*

**Attention**: you have to create the database in the MSSQL management studio before running this test otherwise
the test will fail.