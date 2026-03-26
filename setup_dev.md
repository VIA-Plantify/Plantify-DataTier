# This tutorial is for Jetbrains Rider

## Update Rider to the latest version

## Creating a dev container

Open **Settings -> File -> Remote Development**

On the left click devcontainers and create new if it doesn t already exist.

Select **FROM VCS PROJECT** and put in the following link:
https://github.com/VIA-Plantify/Plantify-DataTier.git

select **development or the branch currently under development** and continue, the ide should setup a remote container

## Refreshing Grpc Container 
inside the devcontainer run: ``scripts/refresh-grpc.sh``

outside container it gets a bit more complicated,
I know only for linux but it should work in WSL:

run: ``chmod +x scripts/refresh-grpc.sh`` to make it executable

afterwards run: ``./scripts/refresh-grpc.sh``

