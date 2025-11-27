# Learning .NET Microservices 🚀

A hands-on project exploring **.NET**, **Docker**, **RabbitMQ**, and distributed microservice architecture. This repo includes backend services, shared contracts, and a simple frontend to tie everything together.

---

## 📁 Project Structure

```bash
├── Contracts/Play.Catalog.Contracts # Shared message contracts for services
├── services/ # .NET microservices
├── frontend/ # Simple UI to interact with services
│
├── apply.sh # Deployment / automation script
├── Play.Catalog.sln # Solution file
└── README.md
```


---

## 🔧 Tech Stack

- **C# / .NET 8**
- **Docker** for containerizing services  
- **RabbitMQ** for message-based communication  
- **REST APIs**  
- **Frontend** built with basic web tools  

---

## ⭐ Features

- Microservice design with clear separation of concerns  
- Event-driven communication through RabbitMQ  
- Docker-ready deployment workflow  
- Shared contract library to keep messages consistent  

---

## ▶️ Getting Started

1. Clone the repository  
   ```bash
   git clone https://github.com/ritsth/learning_dot_net.git
   ```
2. Run the setup script
   ```bash
    ./apply.sh
   ```
3. Spin up Docker services
   ```bash
    docker-compose up --build
   ```

