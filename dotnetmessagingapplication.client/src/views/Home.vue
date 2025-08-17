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
                chats: [{id: "id", name: "da boyz", lastMessage: "noice"}] as Array<types.Chat>,
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {

        },

        methods: {
            async switchTab(tab: String) {
                this.chatListTabSelected = tab;
                // repopulate chats list with API call
            }
        }
    });
</script>

<template>
    <div id="header">
        <img id="app-icon" src="https://1000logos.net/wp-content/uploads/2021/06/Discord-logo.png"></img>
        <div id="settings-buttons">
            <!-- <input type="image" src="https://www.iconpacks.net/icons/2/free-settings-icon-3110-thumb.png"></input> -->
            <button>Settings</button>
            <button>Account</button>
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
                <ChatSelectBox :chat-name="chat.name" :last-message="chat.lastMessage"></ChatSelectBox>
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
                <h1>chat window</h1>
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
</style>