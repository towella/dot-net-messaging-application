<script setup lang="ts">
    import Widget from '../components/Widget.vue'
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
            returnHome() {
                this.$router.push('home');
            },

            saveChanges() {
                this.returnHome()
            },
        }
    });
</script>

<template>
    <div id="settings">
        <div id="header">
            <button class="back-button" v-on:click="returnHome()"><</button>
            <h1>Settings</h1>
            <div style="width: 70px;"></div> <!-- This empty div is required for even spacing -->
        </div>

        <Widget id="settings-body">
            <div class="settings-item">
                <label for="send-email-notifs">Send email notifications: </label>
                <input type="checkbox" id="send-email-notifs" />
            </div>

            <div class="settings-item">
                <label for="privacy-level">Privacy level: </label>
                <select id="privacy-level">
                    <option value="private">Private</option>
                    <option value="public">Public</option>
                </select>
            </div>

            <div class="settings-item">
                <label for="use=time-limit">Time limit: </label>
                <input type="checkbox" id="use-time-limit" />
            </div>
            <div style="display: flex; align-items: center; padding-left: 20px;">
                <input type="number" min="0" max="24" />
                <p> hrs </p>
                <input type="number" min="0" max="60" />
                <p> mins </p>
            </div>

            <div class="settings-item">
                <label for="theme">Theme: </label>
                <select id="theme">
                    <option value="dark">Dark</option>
                    <option value="light">Light</option>
                </select>
            </div>

            <div class="account-item">
                <button v-on:click="saveChanges()" style="margin: 0; margin-top: 10px;">Save Changes</button>
            </div>
        </Widget>
    </div>
</template>

<style scoped>

    #settings {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        padding-bottom: 20px;
        background-color: var(--darkColour);
    }

    #header {
        display: flex;
        width: 100%;
        justify-content: space-between;
        align-items: center;
        border-bottom: var(--darkestColour) 1px solid;
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
        margin: 10px;
    }

    #settings-body {
        width: 400px;
        height: 80vh;
        padding-left: 30px;
        margin: 20px;
        text-align: left;
    }

    .settings-item {
        margin-top: 20px;
    }
</style>