<script setup lang="ts">
    import ChatSelectBox from "../components/ChatSelectBox.vue"
</script>

<script lang="ts">
    import { defineComponent } from 'vue'
    import * as types from '../types.ts'

    export default defineComponent({
        data() {
            return {
                chatListTabSelected: "dm" as String,
                chats: [{id: "id", name: "da boyz", lastMessage: "noice"},
                        {id: "asd", name: "pijins", lastMessage: "this is the last message"}
                ] as Array<types.Chat>,
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {

        },

        methods: {
            async switchTab(tab: String) {
                this.chatListTabSelected = tab;
                // repopulate chats list with API call
                this.chats = [];
            },

            async changeChat(chatId: String) {

            },

            openPage(pageName: String) {
                this.$router.push(pageName);
            },
        }
    });
</script>

<template>
    <div id="header">
        <img id="app-icon" src="https://1000logos.net/wp-content/uploads/2021/06/Discord-logo.png"></img>
        <div id="settings-buttons">
            <!-- <input type="image" src="https://www.iconpacks.net/icons/2/free-settings-icon-3110-thumb.png"></input> -->
            <button v-on:click="openPage('settings')">Settings</button>
            <button v-on:click="openPage('account')">Account</button>
        </div>
    </div>

    <div id="body-content">
        <div id="chat-list">
            <div id="chat-list-actions">
                <input placeholder="Search chats..."></input>
                <button>New Chat</button>
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
                <ChatSelectBox :chat-name="chat.name" :last-message="chat.lastMessage" v-on:click="changeChat(chat.id)"></ChatSelectBox>
            </div>
            <p v-else>No chats, time to start a conversation!</p>
        </div>

        <div id="chat-window-container">
            <div id="chat-window-header">
                <h2 id="chat-heading">Chat Name</h2>
                <div id="call-buttons">
                    <button>Call</button>
                    <button>Video call</button>
                </div>
            </div>

            <div id="chat-window">

            </div>

            <div id="input-bar">
                <textarea id="message-input" type="text" placeholder="Message the chat..."></textarea>
                <button id="send-button">Send</button>
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
        background-color: var(--secondaryDarkColour);
        width: 100%;
    }

    #chat-heading {
        margin-left: 3%;
    }

    #call-buttons {
        margin-top: 3px;
        margin-bottom: 3px;
        margin-right: 3%
    }

    #chat-window {
        width: 100%;
        height: 100%;
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
        min-height: 2rem;
    }

    #send-button {
        height: fit-content;
        margin: 0;
        margin-left: 10px;
    }
</style>