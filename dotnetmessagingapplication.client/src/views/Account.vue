<script setup lang="ts">
    import Widget from '../components/Widget.vue';
</script>

<script lang="ts">
    import { defineComponent } from 'vue'

    export default defineComponent({
        data() {
            return {
                errorMessage: '',
                newUsername: '',
                newEmail: '',
                newPhone: '',
                newPronouns: '',
                newBio: ''
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {
            const response = await fetch('https://localhost:7157/api/controllers/details', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ emailOrUsername: this.$route.params.username})
            })
            .then(r => r.json())

            const profileImage = document.getElementById('profile-picture')! as HTMLImageElement;
            profileImage.src = "https://i.insider.com/602ee9ced3ad27001837f2ac?width=700";

            this.newUsername = response.username;
            this.newEmail = response.email;
            this.newPhone = response.phone;
            this.newPronouns = response.pronouns;
            this.newBio = response.bio;
        },

        methods: {
            returnHome() {
                this.$router.push('home');
            },

            async saveChanges() {                   
                if (!this.newUsername || !this.newEmail || !this.newPhone || !this.newPronouns || !this.newBio) {
                    this.errorMessage = 'All fields must be filled to save';
                } else {
                    this.errorMessage = '';
                    const response = await fetch('https://localhost:7157/api/controllers/updateUser', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({ 
                            oldUsername: this.$route.params.username,
                            username: this.newUsername,
                            email: this.newEmail,
                            phone: this.newPhone,
                            pronouns: this.newPronouns,
                            bio: this.newBio,
                        })
                    })

                    if (response.status === 200) {
                        this.returnHome();
                    } else {
                        this.errorMessage = await response.text()
                    }
                }
            },
        }
    });
</script>

<template>
    <div id="account">
        <div id="header">
            <Button class="back-button" v-on:click="returnHome()"><</Button>
            <h1 style="padding-left: 10px;">Account</h1>
            <div style="width: 70px;"></div>
        </div>

        <Widget id="account-body">
            <div class="account-item" style="display: flex; justify-content: center; padding-right: 10px;">
                <img id="profile-picture"></img>
            </div>

            <div class="account-item">
                <label for="username">Username: </label>
                <input v-model="newUsername" id="username" placeholder="Username..."/>
            </div>

            <div class="account-item">
                <label for="email">Email: </label>
                <input v-model="newEmail" id="email" type="email" placeholder="Email..."/>
            </div>

            <div class="account-item">
                <label for="phone">Phone: </label>
                <input v-model="newPhone" id="phone" placeholder="Phone..."/>
            </div>

            <div class="account-item">
                <label for="pronouns">Pronouns: </label>
                <input v-model="newPronouns" id="pronouns" placeholder="They/Them/Theirs..."/>
            </div>

            <div class="account-item">
                <p for="bio" style="margin-bottom:0;">Bio</p>
                <textarea v-model="newBio" id="bio" placeholder="Bio..."></textarea>
            </div>

            <div class="account-item">
                <button v-on:click="saveChanges()" style="margin: 0;">Save Changes</button>
                <p id="errorMessage" :innerText="errorMessage"></p>
            </div>
        </Widget>
    </div>
</template>

<style scoped>
    #account {
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

    #account-body {
        width: 400px;
        height: 80vh;
        padding-left: 30px;
        margin: 20px;
        text-align: left;
    }

    .account-item {
        margin-top: 20px;
    }

    #profile-picture {
        width: 100px;
        height: 100px;
        border-radius: 50%;
        border: none;
        transition: border 0.2s;
    }

    #profile-picture:hover {
        border: white 3px solid;
        transition: border 0.2s;
    }

    #bio {
        width: 94%;
    }

    #errorMessage {
        color: var(--destructiveColour);
    }
</style>
