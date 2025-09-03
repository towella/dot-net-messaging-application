<script setup lang="ts">
    import ChatSelectBox from "../components/ChatSelectBox.vue"
    import MessageBubble from "../components/MessageBubble.vue"
</script>

<script lang="ts">
    import { defineComponent } from 'vue'
    import * as types from '../types.ts'
    import signalrService from '../signalrService'

    export default defineComponent({
        data() {
            return {
                chatListTabSelected: "dm",
                chats: [] as types.Chat[],
                selectedChatIndex: 0,
                id: -1,
                selectedImageFile: null as File | null,
            };
        },
        computed: {
            chatHeading(): string {
                const chat = this.chats[this.selectedChatIndex];
                if (this.chatListTabSelected === 'dm') {
                    return 'Chat Name';
                } 
                else {
                    return chat.name ?? 'Chat Name';
                }
            }
        },


        // lifecycle hook (called on mount)
        async mounted() {
            // get user details
            const response = await fetch('https://localhost:7157/api/controllers/details', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ emailOrUsername: this.$route.params.username})
            })
            .then(r => r.json())
            this.id = response.id;

            await signalrService.startConnection("https://localhost:7157/chatHub");

            signalrService.onMessageEdited((messageId, newContent) => {
                for (const chat of this.chats) {
                    const msg = chat.messages.find(m => m.id === messageId);
                    if (msg) msg.body = newContent;
                }
            });

            signalrService.onMessageDeleted((messageId) => {
                for (const chat of this.chats) {
                    const idx = chat.messages.findIndex(m => m.id === messageId);
                    if (idx !== -1) chat.messages.splice(idx, 1);
                }
            });

            signalrService.onChatDeleted((chatId) => {
                const idx = this.chats.findIndex(c => c.chatId === chatId);
                if (idx !== -1) this.chats.splice(idx, 1);
            });

            if (signalrService.connection) {
                signalrService.connection.on("ReceiveChats", (chats: Array<types.Chat>) => {
                    this.chats = chats;
                    this.selectedChatIndex = 0;
                });
                signalrService.connection.on("ReceiveMessage", async (chats) => {
                    this.chats = chats;
                    console.log("Received DMs:", chats);
                    for (const chat of this.chats) {
                        await signalrService.connection?.invoke("JoinChatGroup", chat.chatId);
                    }
                });
            }
        },

        methods: {
            async changeChat(chatId: number) {
                this.selectedChatIndex = this.chats.findIndex(c => c.chatId === chatId);
                const chat = this.chats[this.selectedChatIndex];
                console.log(chat)
                var newChat = await signalrService.changeChat({ chatId: chat.chatId });
                console.log("new chat", newChat)
                chat.messages = newChat.messages;
                console.log(chat)
            },

            handleImageSelection(event: Event) {
                const input = event.target as HTMLInputElement;
                const file = input.files?.[0];
                if (!file) return;

                this.selectedImageFile = file;
            },

            async sendMessage() {
            let imageUrl: string | null = null;
            const messageInput = document.getElementById("message-input") as HTMLTextAreaElement | null;
            const messageText = messageInput?.value ?? "";

            if (messageInput && messageText !== "") {
                    const chat = this.chats[this.selectedChatIndex];
                    console.log("Selected image file:", this.selectedImageFile);
                    if (this.selectedImageFile) {
                        const formData = new FormData();
                        formData.append("image", this.selectedImageFile);
                        formData.append("chatId", this.chats[this.selectedChatIndex].chatId.toString());
                        formData.append("senderId", this.id.toString());
                        formData.append("message", messageText);
                        messageInput.value = ""; // Clear input

                        try {
                        const response = await fetch("https://localhost:7157/api/images/upload", {
                            method: "POST",
                            body: formData,
                        });

                        console.log("response", response)

                        if (!response.ok) {
                            const errorText = await response.text();
                            console.error("Image upload failed:", errorText);
                            return;
                        }

                        const result = await response.json();
                        imageUrl = result.imageUrl;
                        } catch (err) {
                        console.error("Unexpected error during image upload:", err);
                        return;
                        }

                        this.selectedImageFile = null; // Clear after upload
                    }

                const newMessage: types.Message = {
                    id: null,
                    body: messageText,
                    senderUser: this.$route.params.username as string,
                    authorId: this.id,
                    chatId: this.chats[this.selectedChatIndex].chatId,
                    imageUrl: imageUrl,
                };
                console.log("ahfuiaio")
                console.log(newMessage)

                await signalrService.sendMessage({
                    senderId: this.id,
                    chatId: this.chats[this.selectedChatIndex].chatId,
                    message: messageText,
                    imageUrl: imageUrl,
                })
                chat.messages.push(newMessage);
                console.log(newMessage)

                messageInput.value = ""; // Clear input
            };
            },
            
            async editMessage(messageId: number, newContent: string) {
                await signalrService.editMessage({ messageId, newMessage: newContent });
            },

            async deleteMessage(messageId: number) {
                await signalrService.deleteMessage({ messageId });
            },

            async createChat(participantUsers: string[], chatName: string) {
                await signalrService.createChat({ creatorUser: this.$route.params.username as string, participantUsers, chatName });
            },

            async deleteChat(chatId: number) {
                await signalrService.deleteChat({ chatId });
            },

            async switchTab(tab: string) {
                this.chatListTabSelected = tab;
                if (tab === "dm") {
                    this.chats = await this.getDirectMessages(this.$route.params.username as string);
                } else if (tab === "gc") {
                    this.chats = await this.getGroupChats(this.$route.params.username as string);
                }

                console.log("switched tabs", this.chats);
            },

            async getDirectMessages(username: string): Promise<types.Chat[]> {
                return await signalrService.getDirectMessages(username);
            },

            async getGroupChats(username: string): Promise<types.Chat[]> {
                return await signalrService.getGroupChats(username);
            },

            createNewChat() {
                const creatorUser = this.$route.params.username as string;
                var userNames: Array<string> = [];
                var username: string | null = prompt("Create chat with: (leave empty to create) ", "");
                while (username != null && username != "") {
                    userNames.push(username);
                    username = prompt("Create chat with: (leave empty to create) ", "");
                }

                if (userNames.length === 0) {
                    alert("No users added. Chat not created.");
                    return;
                }

                if (userNames.length === 1) {
                    // Direct Message
                    const request = {
                        creatorUser: creatorUser,
                        participantUsers: [userNames[0]],
                        chatName: userNames[0]
                    };
                    this.createChat(request.participantUsers, request.chatName);
                    this.switchTab("dm");
                } else {
                    // Group Chat
                    const chatName = prompt("Enter a name for the group chat:", "My Group Chat");
                    const request = {
                        creatorId: creatorUser,
                        participantUsers: userNames,
                        chatName: chatName || "My Group Chat"
                    };
                    this.createChat(request.participantUsers, request.chatName);
                    this.switchTab("gc");
                }

                // create a chat with userNames in it
            },

            triggerImageUpload() {
                const input = document.getElementById("image-input") as HTMLInputElement;
                input?.click();
            },
        }
    });
</script>

<template>
    <div id="header">
        <RouterLink :to="{name: 'Login'}" style="padding-left: 40px;"><img id="app-icon" style="width: 200px; margin: 0px;" src="../assets/logo.png"></img></RouterLink>
        <div id="settings-buttons">
            <!-- <input type="image" src="https://www.iconpacks.net/icons/2/free-settings-icon-3110-thumb.png"></input> -->
            <RouterLink :to="{name: 'Settings'}"><img src="../assets/icons/settings.png" style="width: 40px;" /></RouterLink>
            <RouterLink :to="{name: 'Account'}"><img id="profile-picture" src="https://i.insider.com/602ee9ced3ad27001837f2ac?width=700"></img></RouterLink>
        </div>
    </div>

    <div id="body-content">
        <div id="chat-list">
            <div id="chat-list-actions">
                <div style="display: flex; align-items: center;">
                    <label for="chat-search"><img src="../assets/icons/search.png" style="width: 20px; padding-top: 5px;" /></label>
                    <input id="chat-search" placeholder="Search chats..." />
                </div>
                <button v-on:click="createNewChat()" style="padding: 3px;"><img src="../assets/icons/message.png" style="width: 30px;" /></button>
            </div>

            <div id="chat-list-tabs">
                <div class="chat-list-tab" :class="{'chat-list-tab-selected': chatListTabSelected =='dm'}" id="dm-chat-list-tab" v-on:click="switchTab('dm')">
                    <h2>DM</h2>
                </div>
                <div class="chat-list-tab" :class="{'chat-list-tab-selected': chatListTabSelected == 'gc'}" id="gc-chat-list-tab" v-on:click="switchTab('gc')">
                    <h2>GC</h2>
                </div>
            </div>

            <div v-if="chats.length > 0" v-for="chat in chats">
                <ChatSelectBox :chat-name="chat.name" :last-message="chat.messages[chat.messages.length-1]?.body ?? 'No messages yet...'" v-on:click="changeChat(chat.chatId)"></ChatSelectBox>
            </div>
            <p v-else>No chats, time to start a conversation!</p>
        </div>

        <div id="chat-window-container">
            <div id="chat-window-header">
                <h2 id="chat-heading">{{ chatHeading }}</h2>
                <div id="call-buttons">
                    <RouterLink :to="{name: 'AudioChat'}"><img src="../assets/icons/call.png" style="width: 40px;"/></RouterLink>
                    <RouterLink :to="{name: 'VideoChat'}"><img src="../assets/icons/video.png" style="width: 40px;"/></RouterLink>
                </div>
            </div>

            <div id="chat-window">
                <MessageBubble v-if="chats[selectedChatIndex]?.messages.length > 0" 
                    v-for="m in chats[selectedChatIndex]?.messages"
                    :sender="m.senderUser" :body="m.body" :image-url="m.imageUrl" :external-message="m.senderUser != $route.params.username"></MessageBubble>
                <p v-else style="align-self: center;">No one has said anything yet. Start the conversation!</p>
            </div>

            <div id="input-bar">
                <textarea id="message-input" type="text" placeholder="Message the chat..."></textarea>
                <input type="file" id="image-input" accept="image/*" style="display: none;" @change="sendMessage()" />
                <button @click="triggerImageUpload" style="padding:0; width: 75px;">
                    <img src="../assets/icons/image.png" style="height: 50px;" />
                </button>
                <button id="send-button" v-on:click="sendMessage()">Send</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
    @import '../assets/main.css';

    h1, h2 {
        margin: 10px;
    }

    #header {
        display: flex;
        justify-content: space-between;
        background-color: var(--secondaryStandoutColour);
        width: 100%;
        height: 10vh;
    }

    #app-icon {
        margin-left: 3%;
    }

    #settings-buttons {
        display: flex;
        align-items: center;
        margin-right: 2%;
    }

    #profile-picture {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        border: white 3px solid;
    }

    #body-content {
        display: flex;
        width: 100%;
        height: 85vh;
    }

    #chat-list {
        background-color: var(--darkColour);
        width: 20%;
        text-align: center;
    }

    #chat-list-actions {
        display: flex;
        justify-content: space-evenly;
        align-items: center;
    }

    #chat-list-tabs {
        display: flex;
        justify-content: space-evenly;
    }

    .chat-list-tab {
        width: 100%;
        border-left: 1px solid var(--darkestColour);
        border-right: 1px solid var(--darkestColour);
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
        text-align: center;
    }

    .chat-list-tab:hover {
        background-color: var(--secondaryDarkColour);
    }

    .chat-list-tab-selected {
        background-color: var(--darkestColour);
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2) inset;
    }

    .chat-list-tab-selected:hover {
        background-color: var(--darkestColour);
    }

    #chat-window-container {
        display: flex;
        flex-direction: column;
        align-items: stretch;
        width: 80%;
        height: 100%;
    }

    #chat-window-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        background-color: var(--secondaryDarkColour);
        width: 100%;
    }

    #chat-heading {
        margin-left: 3%;
    }

    #call-buttons {
        display: flex;
        align-content: center;
        margin-top: 3px;
        margin-bottom: 3px;
        margin-right: 3%
    }

    #chat-window {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 100%;
        overflow-x: hidden;
        overflow-y: auto;
    }

    #input-bar {
        display: flex;
        justify-content: center;
        align-items: flex-end;
        width: 100%;
        padding: 10px;
    }

    #message-input {
        width: 80%;
        font-size: 20px;
        overflow-x: hidden;
        resize: none;
        field-sizing: content;
        max-height: 12rem;
        min-height: 25px;
    }

    #send-button {
        height: fit-content;
        margin: 0;
        margin-left: 10px;
    }
</style>