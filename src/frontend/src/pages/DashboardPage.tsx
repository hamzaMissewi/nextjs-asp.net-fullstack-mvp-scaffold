import type React from "react"
import { useQuery } from "@tanstack/react-query"
import { Plus, FileText, Calendar, TrendingUp } from "lucide-react"
import { Button } from "../components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../components/ui/card"
import { resumeService } from "../services/resumeService"
import { useNavigate } from "react-router-dom"
import { LoadingSpinner } from "../components/ui/LoadingSpinner"

export const DashboardPage: React.FC = () => {
  const navigate = useNavigate()

  const { data: resumes, isLoading } = useQuery({
    queryKey: ["resumes"],
    queryFn: resumeService.getResumes,
  })

  const handleCreateResume = async () => {
    try {
      const newResume = await resumeService.createResume({
        title: "New Resume",
        content: "",
        templateId: 1,
      })
      navigate(`/resume/${newResume.id}`)
    } catch (error) {
      console.error("Failed to create resume:", error)
    }
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-[400px]">
        <LoadingSpinner size="lg" />
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
          <p className="text-muted-foreground">Manage your resumes and track your job application progress</p>
        </div>
        <Button onClick={handleCreateResume} className="flex items-center gap-2">
          <Plus className="h-4 w-4" />
          Create Resume
        </Button>
      </div>

      {/* Stats Cards */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Resumes</CardTitle>
            <FileText className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{resumes?.length || 0}</div>
            <p className="text-xs text-muted-foreground">+2 from last month</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Applications Sent</CardTitle>
            <Calendar className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">12</div>
            <p className="text-xs text-muted-foreground">+4 from last week</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Response Rate</CardTitle>
            <TrendingUp className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">25%</div>
            <p className="text-xs text-muted-foreground">+5% from last month</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">AI Optimizations</CardTitle>
            <TrendingUp className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">8</div>
            <p className="text-xs text-muted-foreground">This month</p>
          </CardContent>
        </Card>
      </div>

      {/* Recent Resumes */}
      <Card>
        <CardHeader>
          <CardTitle>Recent Resumes</CardTitle>
          <CardDescription>Your recently created and modified resumes</CardDescription>
        </CardHeader>
        <CardContent>
          {resumes && resumes.length > 0 ? (
            <div className="space-y-4">
              {resumes.map((resume) => (
                <div
                  key={resume.id}
                  className="flex items-center justify-between p-4 border rounded-lg hover:bg-muted/50 cursor-pointer"
                  onClick={() => navigate(`/resume/${resume.id}`)}
                >
                  <div className="flex items-center space-x-4">
                    <div className="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center">
                      <FileText className="h-5 w-5 text-primary" />
                    </div>
                    <div>
                      <h3 className="font-medium">{resume.title}</h3>
                      <p className="text-sm text-muted-foreground">
                        Last modified {new Date(resume.updatedAt).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                  <Button variant="ghost" size="sm">
                    Edit
                  </Button>
                </div>
              ))}
            </div>
          ) : (
            <div className="text-center py-8">
              <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
              <h3 className="text-lg font-medium mb-2">No resumes yet</h3>
              <p className="text-muted-foreground mb-4">Create your first resume to get started</p>
              <Button onClick={handleCreateResume}>
                <Plus className="h-4 w-4 mr-2" />
                Create Resume
              </Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
