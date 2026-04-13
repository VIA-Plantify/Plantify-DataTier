# Plantify-DataTier

# To set up a dev container:

## [Set up dev container documentation](https://github.com/VIA-Plantify/Plantify-DataTier/blob/development/setup_dev.md)

# Set up the database:

## [Set up or update the database using migrations in EFC for container database](https://github.com/VIA-Plantify/Plantify-DataTier/blob/development/database_setup_or_update.md)


### If you want to connect to the database there are 3 cases:

<h1>The postgres container must be running</h1>


<h3>From native machine</h3>
```
jdbc:postgresql://localhost:55432/plantify?password=plantifydev&user=dev 
```
<h3>From container</h3>

```
jdbc:postgresql://host.docker.internal:55432/plantify?user=dev&password=plantifydev
```

<h3> In production containers</h3>>

```
jdbc:postgresql://postgres:5432/plantify?password=plantifydev&user=dev
```