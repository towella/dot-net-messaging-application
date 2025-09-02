import { HubConnectionBuilder, HubConnection, LogLevel } from "@microsoft/signalr";

class SignalRService {
    public connection: HubConnection | null = null;

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

    async joinChatGroup(chatId: number) {
        await this.connection?.invoke("JoinChatGroup", chatId);
    }

    async sendMessage(request: { senderId: number, chatId: number, message: string }) {
        await this.connection?.invoke("SendMessage", request);
    }

    // Message actions
    async editMessage(request: { messageId: number, newMessage: string }) {
        await this.connection?.invoke("EditMessage", request);
    }
    async deleteMessage(request: { messageId: number }) {
        await this.connection?.invoke("DeleteMessage", request);
    }

    // Chat actions
    async createGroupChat(request: { creatorId: number, participantIds: number[], chatName: string }) {
        await this.connection?.invoke("CreateGroupChat", request);
    }
    async deleteChat(request: { chatId: number }) {
        await this.connection?.invoke("DeleteChat", request);
    }

    // Event listeners
    onMessageEdited(callback: (messageId: number, newContent: string) => void) {
        this.connection?.on("MessageEdited", callback);
    }
    onMessageDeleted(callback: (messageId: number) => void) {
        this.connection?.on("MessageDeleted", callback);
    }
    onChatCreated(callback: (chatName: string) => void) {
        this.connection?.on("ChatCreated", callback);
    }
    onChatDeleted(callback: (chatId: number) => void) {
        this.connection?.on("ChatDeleted", callback);
    }

    stopConnection() {
        this.connection?.stop();
    }
}

export default new SignalRService();
