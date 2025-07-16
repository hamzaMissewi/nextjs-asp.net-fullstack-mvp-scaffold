# Architecture Documentation

## System Overview

The AI-Powered Resume Generator is a full-stack web application built with modern technologies to provide users with intelligent resume creation and optimization capabilities.

## Architecture Patterns

### Backend Architecture
- **Clean Architecture**: Separation of concerns with Core, Infrastructure, and API layers
- **CQRS Pattern**: Command Query Responsibility Segregation for complex operations
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling and testability

### Frontend Architecture
- **Component-Based Architecture**: Reusable React components
- **State Management**: Zustand for global state, React Query for server state
- **Feature-Based Structure**: Organized by features rather than file types

## Technology Stack

### Backend (.NET 8)
\`\`\`
┌─────────────────────────────────────────┐
│              Presentation Layer          │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │ Controllers │  │    SignalR Hubs     ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│               Core Layer                 │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │  Services   │  │     Interfaces      ││
│  └─────────────┘  └─────────────────────┘│
│  ┌─────────────┐  ┌─────────────────────┐│
│  │   Entities  │  │       DTOs          ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│           Infrastructure Layer           │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │ Repositories│  │    External APIs    ││
│  └─────────────┘  └─────────────────────┘│
│  ┌─────────────┐  ┌─────────────────────┐│
│  │   DbContext │  │    ML Services      ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
\`\`\`

### Frontend (React + TypeScript)
\`\`\`
┌─────────────────────────────────────────┐
│               Pages Layer                │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │  Dashboard  │  │   Resume Editor     ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│             Components Layer             │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │ UI Components│  │ Feature Components ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
┌─────────────────────────────────────────┐
│              Services Layer              │
│  ┌─────────────┐  ┌─────────────────────┐│
│  │ API Client  │  │   SignalR Client    ││
│  └─────────────┘  └─────────────────────┘│
└─────────────────────────────────────────┘
\`\`\`

## Data Flow

### Resume Creation Flow
1. User initiates resume creation
2. Frontend sends request to API
3. API creates resume record in database
4. AI service generates initial content
5. Real-time updates via SignalR
6. Frontend updates UI with new content

### Real-time Collaboration Flow
1. User joins resume editing session
2. SignalR connection established
3. User joins resume-specific group
4. Content changes broadcast to group members
5. Optimistic UI updates on frontend

## Security Architecture

### Authentication & Authorization
- **JWT Tokens**: Stateless authentication
- **ASP.NET Identity**: User management
- **OAuth2**: Third-party authentication
- **Role-based Access**: Fine-grained permissions

### Data Protection
- **HTTPS**: Encrypted communication
- **Input Validation**: XSS and injection prevention
- **Rate Limiting**: API abuse prevention
- **CORS**: Cross-origin request control

## Scalability Considerations

### Horizontal Scaling
- **Stateless API**: Easy to scale across multiple instances
- **SignalR Backplane**: Redis for multi-instance real-time communication
- **Database Sharding**: User-based data partitioning

### Performance Optimization
- **Caching**: Redis for frequently accessed data
- **CDN**: Static asset delivery
- **Database Indexing**: Optimized queries
- **Lazy Loading**: On-demand resource loading

## Deployment Architecture

### Azure Cloud Infrastructure
\`\`\`
┌─────────────────────────────────────────┐
│            Azure Front Door             │
│         (Load Balancer + CDN)           │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│          Azure Static Web Apps          │
│            (Frontend Hosting)           │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│           Azure App Service             │
│            (Backend API)                │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│          Azure SQL Database             │
│            (Data Storage)               │
└─────────────────────────────────────────┘
\`\`\`

### Container Orchestration
- **Docker Compose**: Local development
- **Azure Container Instances**: Production deployment
- **Azure Container Registry**: Image storage

## Monitoring & Observability

### Application Monitoring
- **Application Insights**: Performance and error tracking
- **Health Checks**: Service availability monitoring
- **Structured Logging**: Centralized log management

### Metrics & Alerts
- **Custom Metrics**: Business-specific KPIs
- **Performance Counters**: System resource monitoring
- **Alert Rules**: Proactive issue detection

## API Design

### RESTful Endpoints
\`\`\`
GET    /api/resumes              # Get user resumes
POST   /api/resumes              # Create new resume
GET    /api/resumes/{id}         # Get specific resume
PUT    /api/resumes/{id}         # Update resume
DELETE /api/resumes/{id}         # Delete resume
POST   /api/resumes/{id}/generate # Generate AI content
POST   /api/resumes/{id}/optimize # Optimize resume
\`\`\`

### SignalR Hubs
\`\`\`
/resumeHub                       # Real-time collaboration hub
  - JoinResumeGroup(resumeId)
  - LeaveResumeGroup(resumeId)
  - UpdateResumeContent(...)
  - SendTypingIndicator(...)
\`\`\`

## Database Schema

### Core Entities
- **Users**: Authentication and profile data
- **Resumes**: Resume content and metadata
- **Templates**: Predefined resume layouts
- **UserProfiles**: Extended user information

### Relationships
- User → Resumes (One-to-Many)
- Resume → Template (Many-to-One)
- User → UserProfile (One-to-One)

## AI Integration

### OpenAI Integration
- **Content Generation**: GPT-based resume writing
- **Content Optimization**: Improvement suggestions
- **Skill Extraction**: Job description analysis

### ML.NET Integration
- **Template Recommendation**: User preference learning
- **Content Scoring**: Resume quality assessment
- **Trend Analysis**: Industry-specific insights

## Error Handling

### Backend Error Handling
- **Global Exception Middleware**: Centralized error processing
- **Custom Exceptions**: Domain-specific error types
- **Structured Error Responses**: Consistent API error format

### Frontend Error Handling
- **Error Boundaries**: React component error isolation
- **Toast Notifications**: User-friendly error messages
- **Retry Logic**: Automatic recovery for transient failures

## Testing Strategy

### Backend Testing
- **Unit Tests**: Individual component testing
- **Integration Tests**: API endpoint testing
- **Performance Tests**: Load and stress testing

### Frontend Testing
- **Unit Tests**: Component and utility testing
- **E2E Tests**: User workflow testing
- **Visual Regression**: UI consistency testing

## Development Workflow

### Git Workflow
- **Feature Branches**: Isolated development
- **Pull Requests**: Code review process
- **Automated Testing**: CI/CD pipeline integration

### Code Quality
- **ESLint/Prettier**: Frontend code formatting
- **EditorConfig**: Consistent coding standards
- **SonarQube**: Code quality analysis
\`\`\`

```md project="AI-Powered Resume Generator MVP" file="docs/API_SPECIFICATION.md" type="markdown"
# API Specification

## Base URL
\`\`\`
Development: http://localhost:5000
Production: https://api.resumegenerator.com
\`\`\`

## Authentication

All API endpoints require authentication via JWT token in the Authorization header:
\`\`\`
Authorization: Bearer <jwt_token>
\`\`\`

## Common Response Format

### Success Response
\`\`\`json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully"
}
\`\`\`

### Error Response
\`\`\`json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable error message",
    "details": { ... }
  }
}
\`\`\`

## Authentication Endpoints

### POST /api/auth/login
Login with email and password.

**Request Body:**
\`\`\`json
{
  "email": "user@example.com",
  "password": "password123"
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "user-id",
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe"
    }
  }
}
\`\`\`

### POST /api/auth/register
Register a new user account.

**Request Body:**
\`\`\`json
{
  "email": "user@example.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe"
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "user-id",
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe"
    }
  }
}
\`\`\`

## Resume Endpoints

### GET /api/resumes
Get all resumes for the authenticated user.

**Response:**
\`\`\`json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "Software Engineer Resume",
      "content": "Resume content...",
      "templateId": 1,
      "createdAt": "2024-01-15T10:30:00Z",
      "updatedAt": "2024-01-16T14:20:00Z"
    }
  ]
}
\`\`\`

### GET /api/resumes/{id}
Get a specific resume by ID.

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "id": 1,
    "title": "Software Engineer Resume",
    "content": "Detailed resume content...",
    "templateId": 1,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-16T14:20:00Z"
  }
}
\`\`\`

### POST /api/resumes
Create a new resume.

**Request Body:**
\`\`\`json
{
  "title": "New Resume",
  "content": "Initial content",
  "templateId": 1
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "id": 2,
    "title": "New Resume",
    "content": "Initial content",
    "templateId": 1,
    "createdAt": "2024-01-17T09:15:00Z",
    "updatedAt": "2024-01-17T09:15:00Z"
  }
}
\`\`\`

### PUT /api/resumes/{id}
Update an existing resume.

**Request Body:**
\`\`\`json
{
  "title": "Updated Resume Title",
  "content": "Updated resume content..."
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "message": "Resume updated successfully"
}
\`\`\`

### DELETE /api/resumes/{id}
Delete a resume.

**Response:**
\`\`\`json
{
  "success": true,
  "message": "Resume deleted successfully"
}
\`\`\`

## AI-Powered Features

### POST /api/resumes/{id}/generate-content
Generate AI-powered resume content.

**Request Body:**
\`\`\`json
{
  "jobDescription": "Software Engineer position requiring React, Node.js...",
  "skills": ["React", "Node.js", "TypeScript", "AWS"],
  "experience": "5 years of full-stack development experience..."
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "content": "Generated professional resume content based on job requirements..."
  }
}
\`\`\`

### POST /api/resumes/{id}/optimize
Optimize existing resume content using AI.

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "optimizedContent": "Improved resume content with better formatting and stronger language..."
  }
}
\`\`\`

### POST /api/ai/suggest-skills
Get skill suggestions based on job description.

**Request Body:**
\`\`\`json
{
  "jobDescription": "Looking for a full-stack developer with experience in modern web technologies..."
}
\`\`\`

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "skills": [
      "JavaScript",
      "React",
      "Node.js",
      "TypeScript",
      "MongoDB",
      "Express.js",
      "Git",
      "Agile",
      "REST APIs",
      "GraphQL"
    ]
  }
}
\`\`\`

## Template Endpoints

### GET /api/templates
Get all available resume templates.

**Response:**
\`\`\`json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Modern Professional",
      "description": "Clean and modern design suitable for tech roles",
      "previewUrl": "/templates/modern-professional-preview.png",
      "isActive": true
    }
  ]
}
\`\`\`

## User Profile Endpoints

### GET /api/profile
Get user profile information.

**Response:**
\`\`\`json
{
  "success": true,
  "data": {
    "id": "user-id",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phone": "+1234567890",
    "location": "San Francisco, CA",
    "linkedInUrl": "https://linkedin.com/in/johndoe"
  }
}
\`\`\`

### PUT /api/profile
Update user profile information.

**Request Body:**
\`\`\`json
{
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "location": "San Francisco, CA",
  "linkedInUrl": "https://linkedin.com/in/johndoe"
}
\`\`\`

## Error Codes

| Code | Description |
|------|-------------|
| AUTH_001 | Invalid credentials |
| AUTH_002 | Token expired |
| AUTH_003 | Insufficient permissions |
| RESUME_001 | Resume not found |
| RESUME_002 | Invalid resume data |
| AI_001 | AI service unavailable |
| AI_002 | Content generation failed |
| TEMPLATE_001 | Template not found |
| USER_001 | User not found |
| VALIDATION_001 | Invalid input data |

## Rate Limiting

API endpoints are rate limited to prevent abuse:

- **Authentication endpoints**: 5 requests per minute
- **Resume CRUD operations**: 100 requests per hour
- **AI-powered features**: 20 requests per hour
- **Other endpoints**: 1000 requests per hour

Rate limit headers are included in responses:
\`\`\`
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1642694400
\`\`\`

## WebSocket Events (SignalR)

### Connection
Connect to the SignalR hub at \`/resumeHub\` with JWT token.

### Events

#### JoinResumeGroup
Join a resume editing session for real-time collaboration.
\`\`\`javascript
connection.invoke("JoinResumeGroup", "resume-id");
\`\`\`

#### UpdateResumeContent
Broadcast content changes to other users.
\`\`\`javascript
connection.invoke("UpdateResumeContent", "resume-id", "new content", "section-name");
\`\`\`

#### ResumeContentUpdated (Receive)
Receive content updates from other users.
\`\`\`javascript
connection.on("ResumeContentUpdated", (data) => {
  // data: { resumeId, content, section, updatedBy, timestamp }
});
\`\`\`

#### TypingIndicator
Show/hide typing indicators.
\`\`\`javascript
connection.invoke("SendTypingIndicator", "resume-id", "section-name", true);
\`\`\`

## SDK Examples

### JavaScript/TypeScript
\`\`\`typescript
import { ResumeGeneratorAPI } from '@resumegen/api-client';

const api = new ResumeGeneratorAPI({
  baseURL: 'https://api.resumegenerator.com',
  token: 'your-jwt-token'
});

// Get all resumes
const resumes = await api.resumes.getAll();

// Create new resume
const newResume = await api.resumes.create({
  title: 'Software Engineer Resume',
  templateId: 1
});

// Generate AI content
const aiContent = await api.ai.generateContent(resumeId, {
  jobDescription: 'Software Engineer role...',
  skills: ['React', 'Node.js'],
  experience: '5 years experience...'
});
\`\`\`

### cURL Examples
\`\`\`bash
# Login
curl -X POST https://api.resumegenerator.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'

# Get resumes
curl -X GET https://api.resumegenerator.com/api/resumes \
  -H "Authorization: Bearer your-jwt-token"

# Generate AI content
curl -X POST https://api.resumegenerator.com/api/resumes/1/generate-content \
  -H "Authorization: Bearer your-jwt-token" \
  -H "Content-Type: application/json" \
  -d '{"jobDescription":"Software Engineer...","skills":["React","Node.js"]}'
\`\`\`
