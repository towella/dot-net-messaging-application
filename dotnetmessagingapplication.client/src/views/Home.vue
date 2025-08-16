<script setup lang="ts">
</script>

<script lang="ts">
    import { defineComponent } from 'vue'

    export default defineComponent({
        data() {
            return {

            };
        },

        // lifecycle hook (called on mount)
        async mounted() {

        },

        methods: {

        }
    });
</script>

<template>
    <div id="header">
        <h1>kaboom</h1>
    </div>
    <div id="body-content">
        <div id="chat-list">
            <h1>chats</h1>
        </div>
        <div id="chat-window-container">
            <div id="chat-window-header">
                <h1>Chat Name</h1>
            </div>
            <div id="chat-window">
                <h1>chat window</h1>
            </div>
        </div>
    </div>
</template>

<style scoped>
    @import '../assets/main.css';

    #header {
        background-color: var(--secondaryStandoutColour);
        width: 100%;
        height: 10vh;
    }

    #body-content {
        display: flex;
        width: 100%;
        height: 85vh;
    }

    #chat-list {
        background-color: var(--darkColour);
        width: 20%;
    }

    #chat-window-container {
        display: flex;
        flex-direction: column;
        align-items: stretch;
        width: 80%;
        height: 100%;
    }

    #chat-window-header {
        background-color: var(--secondaryDarkColour);
        width: 100%;
    }

    #chat-window {
        width: 100%;
        height: 100%;
    }
</style>