docker build -t ritsth/playinventoryservice:latest .
docker push ritsth/playinventoryservice:latest
kubectl rollout restart deployment playinventoryservice-deployment