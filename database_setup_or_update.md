# This guide is made for JetBrains Rider use EFC extension

## Important!!

Make sure you are in **SOLUTION** view and the postgres container is running,
or if the database is in the Azure cloud make sure you  have got the env api key 
and the database is running.

To find the container documentation go to:
[Set up dev container documentation](https://github.com/VIA-Plantify/Plantify-DataTier/blob/development/setup_dev.md)

## Creating a migration

**Right click** on efc and go to Add Migration

Step 1:
Select EFC as the Migrations project

Step 2:
Select GrpcService as the startup project

Step 3: 
Name the migration

Step 4:
Click **Okay**

## Update database

**Right click** on efc and go to Update database...

Step 1:
Select EFC as the Migrations project

Step 2:
Select GrpcService as the startup project

Step 3:
Select the migration you want to update the database with

Step 4:
Click **Okay**


