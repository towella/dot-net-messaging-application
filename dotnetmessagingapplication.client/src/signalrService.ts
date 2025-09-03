import { HubConnectionBuilder, HubConnection, LogLevel } from "@microsoft/signalr";
import type { Chat } from "./types";

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

    async sendMessage(request: { senderId: number, chatId: number, message: string, imageUrl?: string | null }) {
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
    async createChat(request: { creatorUser: string, participantUsers: string[], chatName: string }) {
        await this.connection?.invoke("CreateChat", request);
    }
    async deleteChat(request: { chatId: number }) {
        await this.connection?.invoke("DeleteChat", request);
    }

    async changeChat(request: { chatId: number }): Promise<Chat[]> {
        return await this.connection?.invoke("ChangeChat", request) ?? [];
    }

    async getDirectMessages(username: string): Promise<Chat[]> {
        return await this.connection?.invoke("GetDirectMessages", username) ?? [];
    }

    async getGroupChats(username: string): Promise<Chat[]> {
        return await this.connection?.invoke("GetGroupChats", username) ?? [];
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
