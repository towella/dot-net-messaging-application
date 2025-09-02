import { HubConnectionBuilder, HubConnection, LogLevel } from "@microsoft/signalr";

class SignalRService {
    private connection: HubConnection | null = null;

    async startConnection(hubUrl: string) {
        this.connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        await this.connection.start();
    }

    onMessageReceived(callback: (message: any) => void) {
        this.connection?.on("ReceiveMessage", callback);
    }

    async sendMessage(chatId: string, message: any) {
        await this.connection?.invoke("SendMessage", chatId, message);
    }

    stopConnection() {
        this.connection?.stop();
    }
}

export default new SignalRService();
