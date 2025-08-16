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

        }
    });
</script>

<template>
    <div id="login">
        <Widget id="login-widget" title="Log In">
            <div class="login-input">
                <label for="username-input">Username: </label>
                <input id="username-input" placeholder="Username..."/>
            </div>
            <div class="login-input">
                <label for="password-input">Password: </label>
                <input id="password-input" placeholder="Password..."/>
            </div>
            <button id="login-button">Log in</button>
        </Widget>
    </div>
</template>

<style scoped>
    #login {
        margin: auto;
        display: flex;
    }

    #login-widget {
        width: 300px;
        margin: auto;
        margin-top: 15%;
        align-content: center;
    }

    .login-input {
        margin: 5px;
    }

    #login-button {
        margin-top: 10px;
    }
</style>