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
            <Button v-on:click="returnHome()"><</Button>
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
    }
</style>