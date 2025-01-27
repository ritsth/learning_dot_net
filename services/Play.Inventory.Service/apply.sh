#!/bin/bash

#code : ./apply.sh

# Navigate to the k8s directory
cd k8s

# Apply all YAML files
kubectl apply -f playinventoryservice-ingress.yaml
kubectl apply -f playinventoryservice-secret.yaml
kubectl apply -f playinventoryservice-configmap.yaml

cd ./mongo
# kubectl apply -f mongo2-development.yaml
kubectl apply -f .  

cd ../playinventoryservice
kubectl apply -f playinventoryservice-deployment.yaml
kubectl apply -f playinventoryservice-service.yaml

echo "All manifests applied successfully."

cd ../../../Play.Inventory.Service
docker build -t ritsth/playinventoryservice:latest .
# docker push ritsth/playinventoryservice:latest
# kubectl rollout restart deployment playinventoryservice-deployment

echo "docker build and pushed to docker hub successfully."