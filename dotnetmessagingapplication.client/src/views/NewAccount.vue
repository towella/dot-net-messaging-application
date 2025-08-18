<script setup lang="ts">
    // import neccessary components
    import Widget from "../components/Widget.vue"
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
            create() {
                // validate input
                // create account
                this.$router.push('home');
            }
        }
    });
</script>

<template>
    <div id="new-account">
        <Widget id="new-account-widget" title="New Account">
            <div class="new-account-input">
                <label for="email-input">Email: </label>
                <input id="email-input" placeholder="Email..."/>
            </div>
            <div class="new-account-input">
                <label for="pronouns-input">Pronouns: </label>
                <input id="email-input" placeholder="they/them/theirs..."/>
            </div>
            <div class="new-account-input">
                <label for="username-input">Username: </label>
                <input id="username-input" placeholder="Username..."/>
            </div>
            <div class="new-account-input">
                <label for="password-input">Password: </label>
                <input id="password-input" placeholder="Password..."/>
            </div>
            <button id="create-button" v-on:click="create()">Create</button>
            <RouterLink id="login-link" :to="{name: 'Login'}">Back to login</RouterLink>
        </Widget>
    </div>
</template>

<style scoped>
    #new-account {
        margin: auto;
        display: flex;
    }

    #new-account-widget {
        display: flex;
        flex-direction: column;
        width: 350px;
        margin: auto;
        margin-top: 15%;
        align-content: center;
        align-items: center;
        transition: background-color 0.1s;
        transition: padding 0.1s;
    }

    #new-account-widget:hover {
        background-color: var(--secondaryDarkColour);
        padding: 15px;
        transition: background-color 0.2s;
        transition: padding 0.2s;
    }

    .new-account-input {
        margin: 5px;
    }

    #create-button {
        margin-top: 10px;
    }

    #login-link {
        margin-top: 10px;
    }
</style>