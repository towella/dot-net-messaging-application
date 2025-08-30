<script setup lang="ts">
    import Widget from '../components/Widget.vue'
</script>

<script lang="ts">
  import { defineComponent } from 'vue'

  export default defineComponent({
    data() {
      return {
      }
    },
    props: {
      sender: String,
      body: String,
      // true: other client sent, false: this client sent
      externalMessage: Boolean,
    }
  });
</script>

<template>
  <div id="message-bubble">
    <div id="chat-select-box-text">
        <p id="sender">{{ sender }}</p>
        <Widget id="message-body-bubble" :class="{'internal-message': !externalMessage, 'external-message': externalMessage}">
            <p id="message-body-text">{{ body }}</p>
        </Widget>
    </div>
  </div>
</template>

<style scoped>
  #message-bubble {
    transition: margin 0.1s;
  }

  #message-bubble:hover {
    transition: margin 0.2s;
    margin: 12px;
  }

  #message-body-bubble {
    min-height: 0;
    margin-top: 0;
    width: auto;
  }

  #message-body-bubble:hover {
    /* Override to prevent default widget hover behaviour */
    margin: 10px;
    margin-top: 0;
  }

  .internal-message {
      background-color: var(--standoutColour);
  }

  .external-message {
      background-color: var(--secondaryStandoutColour);
  }

  #sender {
    margin-top: 10px;
    margin-left: 15px;
  }

  #message-body-text {
      display: flex;
      text-align: left;
      white-space: wrap;
  }

  p {
      padding: 0;
      margin: 5px;
  }
</style>