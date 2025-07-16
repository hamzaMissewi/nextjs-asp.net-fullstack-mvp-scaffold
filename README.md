# AI-Powered Resume Generator MVP

A full-stack web application that leverages AI to help users create professional resumes with real-time collaboration features.

## 🚀 Features

- **AI-Powered Resume Generation**: GPT-based resume content generation and optimization
- **Real-time Collaboration**: Live editing and feedback using SignalR
- **Smart Templates**: ML.NET-powered template recommendations
- **User Authentication**: Secure JWT-based authentication with OAuth2 support
- **Responsive Design**: Modern React/TypeScript frontend with Vite
- **Cloud-Ready**: Containerized deployment on Azure

## 🏗️ Architecture

\`\`\`mermaid
graph TB
    subgraph "Frontend (React + TypeScript)"
        A[Web App] --> B[Vite Dev Server]
        A --> C[SignalR Client]
    end
    
    subgraph "Backend (ASP.NET Core)"
        D[Web API] --> E[SignalR Hub]
        D --> F[Authentication Service]
        D --> G[AI Service]
        D --> H[Resume Service]
    end
    
    subgraph "External Services"
        I[OpenAI API]
        J[Azure AD]
    end
    
    subgraph "Data Layer"
        K[Entity Framework Core]
        L[SQL Server/PostgreSQL]
    end
    
    subgraph "Infrastructure"
        M[Docker Compose]
        N[Azure App Service]
        O[Azure Functions]
    end
    
    A --> D
    C --> E
    G --> I
    F --> J
    H --> K
    K --> L
    D --> M
    M --> N
    G --> O
\`\`\`

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core 8.0 Web API
- **Real-time**: SignalR for live collaboration
- **AI/ML**: ML.NET + OpenAI GPT integration
- **Authentication**: ASP.NET Identity + JWT + OAuth2
- **Database**: Entity Framework Core with SQL Server/PostgreSQL
- **Testing**: xUnit, Moq, TestContainers

### Frontend
- **Framework**: React 18 with TypeScript
- **Build Tool**: Vite for fast development
- **State Management**: Zustand/Redux Toolkit
- **UI Library**: Tailwind CSS + shadcn/ui
- **Testing**: Vitest, React Testing Library, Playwright

### DevOps & Infrastructure
- **Containerization**: Docker + Docker Compose
- **CI/CD**: GitHub Actions
- **Cloud**: Azure App Service, Azure Functions, Azure Container Registry
- **Monitoring**: Application Insights

## 📁 Project Structure

\`\`\`
ai-resume-generator/
├── src/
│   ├── backend/
│   │   ├── ResumeGenerator.API/
│   │   ├── ResumeGenerator.Core/
│   │   ├── ResumeGenerator.Infrastructure/
│   │   └── ResumeGenerator.Tests/
│   └── frontend/
│       ├── src/
│       ├── public/
│       └── tests/
├── infrastructure/
│   ├── docker/
│   ├── azure/
│   └── scripts/
├── docs/
└── .github/workflows/
\`\`\`

## 🚦 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+
- Docker Desktop
- SQL Server/PostgreSQL

### Quick Start

1. **Clone the repository**
   \`\`\`bash
   git clone <repository-url>
   cd ai-resume-generator
   \`\`\`

2. **Start with Docker Compose**
   \`\`\`bash
   docker-compose up -d
   \`\`\`

3. **Access the application**
   - Frontend: http://localhost:3000
   - API: http://localhost:5000
   - API Documentation: http://localhost:5000/swagger

### Development Setup

1. **Backend Setup**
   \`\`\`bash
   cd src/backend
   dotnet restore
   dotnet run --project ResumeGenerator.API
   \`\`\`

2. **Frontend Setup**
   \`\`\`bash
   cd src/frontend
   npm install
   npm run dev
   \`\`\`

## 🔧 Configuration

### Environment Variables

#### Backend (.env)
\`\`\`
ConnectionStrings__DefaultConnection=Server=localhost;Database=ResumeGeneratorDB;Trusted_Connection=true;
OpenAI__ApiKey=your-openai-api-key
Azure__ClientId=your-azure-client-id
Azure__ClientSecret=your-azure-client-secret
JWT__SecretKey=your-jwt-secret-key
\`\`\`

#### Frontend (.env)
\`\`\`
VITE_API_BASE_URL=http://localhost:5000
VITE_SIGNALR_HUB_URL=http://localhost:5000/resumeHub
\`\`\`

## 🧪 Testing

### Backend Tests
\`\`\`bash
cd src/backend
dotnet test
\`\`\`

### Frontend Tests
\`\`\`bash
cd src/frontend
npm run test
npm run test:e2e
\`\`\`

## 🚀 Deployment

### Azure Deployment
\`\`\`bash
# Build and push containers
docker build -t resumegenerator-api ./src/backend
docker build -t resumegenerator-web ./src/frontend

# Deploy to Azure
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name resumegenerator-api --deployment-container-image-name resumegenerator-api
\`\`\`

## 📚 API Documentation

API documentation is available at \`/swagger\` when running the backend service.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
