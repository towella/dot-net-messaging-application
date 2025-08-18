<script setup lang="ts">
    // import neccessary components
    import Widget from "../components/Widget.vue"
</script>

<template>
    <div id="not-found">
        <Widget id="not-found-widget" title="Error 404">
            <p>Sorry, we couldn't find your page :(</p>
            <p>Please try a different url</p>
        </Widget>
    </div>
</template>

<style scoped>
    #not-found {
        margin: auto;
        display: flex;
    }

    #not-found-widget {
        width: 300px;
        margin: auto;
        margin-top: 15%;
        align-content: center;
        border-color: var(--destructiveColour);
        transition: background-color 0.1s;
        transition: padding 0.1s;
    }

    #not-found-widget:hover {
        background-color: var(--secondaryDarkColour);
        padding: 15px;
        transition: background-color 0.2s;
        transition: padding 0.2s;
    }
</style>