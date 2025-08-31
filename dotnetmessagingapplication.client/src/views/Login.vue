<script setup lang="ts">
    // import neccessary components
    import Widget from "../components/Widget.vue"
</script>

<script lang="ts">
    import { defineComponent } from 'vue'

    export default defineComponent({
        data() {
            return {
                username: '',
                password: '',
                loggedin: false,
                errorMessage: ''
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {

        },

        methods: {
            async login() {
                if (!this.username || !this.password) {
                    this.errorMessage = 'Username/email or password is blank.';
                    return;
                }

                const response = await fetch('https://localhost:7157/api/controllers/login', {
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ emailOrUsername: this.username, password: this.password })
                })
                .then(r => r.json())

                if (response.success === true) {
                    this.$router.push('user-id-example/home');
                }
                else {
                    this.errorMessage = 'Incorrect login details, please try again!'
                }
            }
        }
    });
</script>

<template>
    <div id="login">
        <Widget id="login-widget" title="Log In">
            <div class="login-input">
                <label for="username-email-input">Username/Email: </label>
                <input id="username-email-input" v-model="username" placeholder="Username/Email..."/>
            </div>
            <div class="login-input">
                <label for="password-input">Password: </label>
                <input id="password-input" v-model="password" placeholder="Password..."/>
            </div>
            <button id="login-button" v-on:click="login()">Log in</button>
            <p id="error-message" v-show="errorMessage">{{ errorMessage }}</p>
            <RouterLink id="create-account-link" :to="{name: 'NewAccount'}">Create a new account</RouterLink>
        </Widget>
    </div>
</template>

<style scoped>
    #login {
        margin: auto;
        display: flex;
    }

    #login-widget {
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

    #login-widget:hover {
        background-color: var(--secondaryDarkColour);
        padding: 15px;
        transition: background-color 0.2s;
        transition: padding 0.2s;
    }

    .login-input {
        margin: 5px;
    }

    #login-button {
        margin-top: 10px;
    }

    #create-account-link {
        margin-top: 10px;
    }

    #error-message {
        color: red;
        font-size: 15px;
    }
</style>