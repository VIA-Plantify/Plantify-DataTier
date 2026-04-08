# This guide is made for JetBrains Rider use EFC extension

## Important!!

<img width="776" height="295" alt="image" src="https://github.com/user-attachments/assets/ea822608-1fc5-47d5-872d-fd4feb6cea29" />

Make sure you are in **SOLUTION** view and the postgres container is running,
or if the database is in the Azure cloud make sure you  have got the env api key 
and the database is running.

To find the container documentation go to:
[Set up dev container documentation](https://github.com/VIA-Plantify/Plantify-DataTier/blob/development/setup_dev.md)

## Creating a migration

**Right click** on efc and go to Add Migration

<img width="850" height="741" alt="image" src="https://github.com/user-attachments/assets/548cb760-7694-44e4-bf3c-877a7f1a0665" />

<img width="871" height="855" alt="image" src="https://github.com/user-attachments/assets/f1e7f246-de19-4ff4-b346-4904dd24dd62" />


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

<img width="776" height="439" alt="image" src="https://github.com/user-attachments/assets/7916f02f-25a3-4478-a62d-6a9de722c766" />


Step 1:
Select EFC as the Migrations project

Step 2:
Select GrpcService as the startup project

Step 3:
Select the migration you want to update the database with

Step 4:
Click **Okay**

<img width="857" height="897" alt="image" src="https://github.com/user-attachments/assets/0820b51a-051d-48c0-aed9-f8332a7127a3" />

