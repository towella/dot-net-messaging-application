<script setup lang="ts">
    import Widget from '../components/Widget.vue'
</script>

<script lang="ts">
    import { defineComponent } from 'vue'

    export default defineComponent({
        data() {
            return {
                volume: 50
            };
        },

        // lifecycle hook (called on mount)
        async mounted() {
            const lastVolume = localStorage.getItem("volume")
            if (lastVolume) {
                const num = Number.parseInt(lastVolume)
                if (num >= 0 && num <= 100) {
                    this.volume = num
                }
            }

        },

        methods: {
            changeVolume() {
                localStorage.setItem("volume", this.volume.toString())
            },
            returnHome() {
                this.$router.push('home');
            },
        }
    });
</script>

<template>
    <div id="settings">
        <div id="header">
            <Button class="back-button" v-on:click="returnHome()"><</Button>
            <h1>Audio Chat</h1>
            <div style="width: 70px;"></div> <!-- This empty div is required for even spacing -->
        </div>

        <Widget id="menu-body">
            <div class="menu-item">
                <label for="volume-slider">Volume: </label>
                <input id="volume-slider" title="volume-slider" type="range" min="0" max="100" v-model="volume" v-on:change="changeVolume" />
                <p>{{ volume }}</p>
            </div>

            <div class="menu-item">
                <label class="desc" for="mic-select">Choose microphone: </label>
                <select id="mic-select" title="mic-select">
                    <option value="bluetooth">Bluetooth microphone</option>
                    <option value="wired">Wired microphone</option>
                </select>
            </div>

            <div class="menu-item">
                <label class="desc" for="noise-cancellation">Noise cancellation: </label>
                <input id="noise-cancellation" title="noise-cancellation" type="checkbox" />
            </div>

            <div id="start-call">
                <button v-on:click="returnHome">Start call!</button>
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

    #menu-body {
        width: 400px;
        height: 80vh;
        padding-left: 30px;
        text-align: left;
    }

    #volume-slider {
        margin: 0 10px;
    }

    #start-call {
        margin-top: 20px;
        margin-left: 35%;
    }

    .menu-item {
        margin-top: 20px;
        display: flex;
        flex-direction: row;
        align-items: center;
    }

    .desc {
        margin-right: 10px;
    }
</style>