echo "Setting up AI Resume Generator development environment..."

# Check if required tools are installed
check_tool() {
    if ! command -v $1 &> /dev/null; then
        echo "❌ $1 is not installed. Please install it first."
        exit 1
    else
        echo "✅ $1 is installed"
    fi
}

echo "Checking required tools..."
check_tool "dotnet"
check_tool "node"
check_tool "docker"
check_tool "git"

# Create directory structure
echo "Creating project structure..."
mkdir -p src/backend/{ResumeGenerator.API,ResumeGenerator.Core,ResumeGenerator.Infrastructure,ResumeGenerator.Tests}
mkdir -p src/frontend/{src,public,tests}
mkdir -p infrastructure/{docker,azure,scripts}
mkdir -p docs
mkdir -p .github/workflows

# Setup backend
echo "Setting up backend..."
cd src/backend

# Create solution and projects
dotnet new sln -n ResumeGenerator
dotnet new webapi -n ResumeGenerator.API
dotnet new classlib -n ResumeGenerator.Core
dotnet new classlib -n ResumeGenerator.Infrastructure
dotnet new xunit -n ResumeGenerator.Tests

# Add projects to solution
dotnet sln add ResumeGenerator.API/ResumeGenerator.API.csproj
dotnet sln add ResumeGenerator.Core/ResumeGenerator.Core.csproj
dotnet sln add ResumeGenerator.Infrastructure/ResumeGenerator.Infrastructure.csproj
dotnet sln add ResumeGenerator.Tests/ResumeGenerator.Tests.csproj

# Add project references
cd ResumeGenerator.API
dotnet add reference ../ResumeGenerator.Core/ResumeGenerator.Core.csproj
dotnet add reference ../ResumeGenerator.Infrastructure/ResumeGenerator.Infrastructure.csproj

cd ../ResumeGenerator.Infrastructure
dotnet add reference ../ResumeGenerator.Core/ResumeGenerator.Core.csproj

cd ../ResumeGenerator.Tests
dotnet add reference ../ResumeGenerator.API/ResumeGenerator.API.csproj
dotnet add reference ../ResumeGenerator.Core/ResumeGenerator.Core.csproj

# Add NuGet packages
cd ../ResumeGenerator.API
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.SignalR
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.ML
dotnet add package Swashbuckle.AspNetCore

cd ../ResumeGenerator.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

cd ../ResumeGenerator.Tests
dotnet add package Moq
dotnet add package Microsoft.AspNetCore.Mvc.Testing

cd ../../..

# Setup frontend
echo "Setting up frontend..."
cd src/frontend

# Initialize React project with Vite
npm create vite@latest . -- --template react-ts
npm install

# Install additional dependencies
npm install @microsoft/signalr @tanstack/react-query @radix-ui/react-avatar @radix-ui/react-dialog @radix-ui/react-dropdown-menu @radix-ui/react-label @radix-ui/react-select @radix-ui/react-slot @radix-ui/react-toast class-variance-authority clsx lucide-react react-hook-form react-router-dom sonner tailwind-merge tailwindcss-animate zustand

# Install dev dependencies
npm install -D @playwright/test @types/node autoprefixer postcss tailwindcss @typescript-eslint/eslint-plugin @typescript-eslint/parser eslint-plugin-react-hooks eslint-plugin-react-refresh jsdom vitest

# Initialize Tailwind CSS
npx tailwindcss init -p

cd ../..

# Create environment files
echo "Creating environment files..."
cat > .env.example << EOF
# Backend Environment Variables
ConnectionStrings__DefaultConnection=Server=localhost;Database=ResumeGeneratorDB;Trusted_Connection=true;
OpenAI__ApiKey=your-openai-api-key-here
Azure__ClientId=your-azure-client-id
Azure__ClientSecret=your-azure-client-secret
JWT__SecretKey=your-jwt-secret-key-here

# Frontend Environment Variables
VITE_API_BASE_URL=http://localhost:5000
VITE_SIGNALR_HUB_URL=http://localhost:5000/resumeHub
EOF

# Copy to actual .env file
cp .env.example .env

# Create Docker network
echo "Creating Docker network..."
docker network create resumegen-network 2>/dev/null || true

# Build and start services
echo "Starting services with Docker Compose..."
docker-compose up -d db

# Wait for database to be ready
echo "Waiting for database to be ready..."
sleep 30

# Run database migrations
echo "Running database migrations..."
cd src/backend
dotnet ef database update --project ResumeGenerator.API

cd ../..

echo "✅ Development environment setup complete!"
echo ""
echo "Next steps:"
echo "1. Update the .env file with your actual API keys"
echo "2. Run 'docker-compose up' to start all services"
echo "3. Access the application at:"
echo "   - Frontend: http://localhost:3000"
echo "   - Backend API: http://localhost:5000"
echo "   - API Documentation: http://localhost:5000/swagger"
echo ""
echo "For development:"
echo "- Backend: cd src/backend && dotnet run --project ResumeGenerator.API"
echo "- Frontend: cd src/frontend && npm run dev"
