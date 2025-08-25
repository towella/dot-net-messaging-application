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
            returnHome() {
                this.$router.push('home');
            }
        }
    });
</script>

<template>
    <div id="account">
        <div id="header">
            <Button class="back-button" v-on:click="returnHome()"><</Button>
            <h1>Account</h1>
            <div></div>
        </div>
    </div>
</template>

<style scoped>
    #account {
        width: 100%;
        background-color: var(--darkColour);
    }

    #header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-bottom: var(--darkestColour) 1px solid;
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
        margin: 10px;
    }
</style>