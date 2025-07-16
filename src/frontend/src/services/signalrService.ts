import { type HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr"

class SignalRService {
  private connection: HubConnection | null = null

  async startConnection(token: string): Promise<void> {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_SIGNALR_HUB_URL}`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()

    try {
      await this.connection.start()
      console.log("SignalR connection started")
    } catch (error) {
      console.error("Error starting SignalR connection:", error)
    }
  }

  async joinResumeGroup(resumeId: string): Promise<void> {
    if (this.connection) {
      await this.connection.invoke("JoinResumeGroup", resumeId)
    }
  }

  async leaveResumeGroup(resumeId: string): Promise<void> {
    if (this.connection) {
      await this.connection.invoke("LeaveResumeGroup", resumeId)
    }
  }

  async updateResumeContent(resumeId: string, content: string, section: string): Promise<void> {
    if (this.connection) {
      await this.connection.invoke("UpdateResumeContent", resumeId, content, section)
    }
  }

  async sendTypingIndicator(resumeId: string, section: string, isTyping: boolean): Promise<void> {
    if (this.connection) {
      await this.connection.invoke("SendTypingIndicator", resumeId, section, isTyping)
    }
  }

  onResumeContentUpdated(callback: (data: any) => void): void {
    if (this.connection) {
      this.connection.on("ResumeContentUpdated", callback)
    }
  }

  onTypingIndicator(callback: (data: any) => void): void {
    if (this.connection) {
      this.connection.on("TypingIndicator", callback)
    }
  }

  async stopConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.stop()
      this.connection = null
    }
  }
}

export const signalRService = new SignalRService()
