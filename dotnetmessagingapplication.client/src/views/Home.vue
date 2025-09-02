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
                chats: [{id: 32, name: "da boyz", messages: [{authorId: 45, authorName: "Name", body: "message!!"}]},
                        {id: 60, name: "pijins", messages: []}
                ] as Array<types.Chat>,
                selectedChatIndex: 0,
                id: -1,
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {
            const response = await fetch('https://localhost:7157/api/controllers/details', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ emailOrUsername: this.$route.params.username})
            })
            .then(r => r.json())
            this.id = response.id;
            // Start SignalR connection (replace with your backend SignalR hub URL)
            await signalrService.startConnection("https://localhost:7157/chatHub");

            // Listen for incoming messages
            signalrService.onMessageReceived((message: types.Message) => {
                // Find the chat by message.chatId and push the message
                const chat = this.chats.find(c => c.id === message.chatId);
                if (chat) {
                    chat.messages.push(message);
                }
            });
        },

        methods: {
            switchTab(tab: string) {
                this.chatListTabSelected = tab;
                // repopulate chats list with API call
                this.chats = [];
            },

            async changeChat(chatId: string) {

            },

            async sendMessage() {
                let messageInput: HTMLTextAreaElement | null = document.getElementById("message-input") as HTMLTextAreaElement | null;
                let messageText: string = messageInput?.value ?? "";
                if (messageInput && messageText != "") {
                    this.chats[0].messages.push({
                        authorId: this.id,
                        authorName: this.$route.params.username,
                        body: messageText,
                        chatId: chat.id // Add chatId property if not present in Message type
                    };
                    // Send via SignalR
                    await signalrService.sendMessage(chat.id, message);
                    // Optionally, add to local state for instant feedback
                    chat.messages.push(message);
                    messageInput.value = "";
                }
            }
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
                <button style="padding: 3px;"><img src="../assets/icons/message.png" style="width: 30px;" /></button>
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
                <ChatSelectBox :chat-name="chat.name" :last-message="chat.messages[chat.messages.length-1]?.body ?? 'No messages yet...'" v-on:click="changeChat(chat.id)"></ChatSelectBox>
            </div>
            <p v-else>No chats, time to start a conversation!</p>
        </div>

        <div id="chat-window-container">
            <div id="chat-window-header">
                <h2 id="chat-heading">Chat Name</h2>
                <div id="call-buttons">
                    <RouterLink :to="{name: 'Login'}"><img src="../assets/icons/call.png" style="width: 40px;"/></RouterLink>
                    <RouterLink :to="{name: 'Login'}"><img src="../assets/icons/video.png" style="width: 40px;"/></RouterLink>
                </div>
            </div>

            <div id="chat-window">
                <MessageBubble v-if="chats[selectedChatIndex]?.messages.length > 0" 
                    v-for="m in chats[selectedChatIndex]?.messages"
                    :sender="m.authorName" :body="m.body" :external-message="m.authorName != $route.params.username"></MessageBubble>
                <p v-else style="align-self: center;">No one has said anything yet. Start the conversation!</p>
            </div>

            <div id="input-bar">
                <textarea id="message-input" type="text" placeholder="Message the chat..."></textarea>
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