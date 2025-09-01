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
                email: '',
                pronouns: '',
                errorMessage: ''
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {

        },

        methods: {
            async create() {
                if (!this.username || !this.password || !this.email || !this.pronouns) {
                    this.errorMessage = 'One or more fields were blank. All fields are required.'
                    return
                }

                const validateExp = /[^a-zA-z0-9_.-]+/

                if (validateExp.test(this.username) || validateExp.test(this.password)) {
                    this.errorMessage = 'Username or password contains invalid characters.'
                    return
                }
                // validate input
                // create account

                const response = await fetch('https://localhost:7157/api/controllers/addUser', {
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(
                        {
                            username: this.username,
                            password: this.password,
                            email: this.email,
                            pronouns: this.pronouns
                        })
                })

                if (response.status === 200) {
                    this.$router.push('user-id-example-2/home');
                }
                else {
                    this.errorMessage = await response.text()
                }
            }
        }
    });
</script>

<template>
    <div id="new-account">
        <Widget id="new-account-widget" title="New Account">
            <div class="new-account-input">
                <label for="email-input">Email: </label>
                <input v-model="email" id="email-input" placeholder="Email..."/>
            </div>
            <div class="new-account-input">
                <label for="pronouns-input">Pronouns: </label>
                <input v-model="pronouns" id="pronouns-input" placeholder="they/them/theirs..."/>
            </div>
            <div class="new-account-input">
                <label for="username-input">Username: </label>
                <input v-model="username" id="username-input" placeholder="Username..."/>
            </div>
            <div class="new-account-input">
                <label for="password-input">Password: </label>
                <input v-model="password" id="password-input" placeholder="Password..."/>
            </div>
            <button id="create-button" v-on:click="create()">Create</button>
            <p id="error-message" v-show="errorMessage">{{ errorMessage }}</p>
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

    #error-message {
        color: red;
        font-size: 15px;
    }
</style>