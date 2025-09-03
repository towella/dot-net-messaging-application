<script setup lang="ts">
    import Widget from '../components/Widget.vue'
    defineProps<{
        sender: string,
        body: string,
        imageUrl?: string | null,
        externalMessage?: boolean
    }>();
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
      imageUrl: { type: String, required: false },
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
              <img v-if="imageUrl" :src="imageUrl" class="message-image" style="max-width: 200px; max-height: 200px; margin-top: 8px;" />
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

  .message-image {
      max-width: 100%;
      height: auto;
      display: block;
      margin-top: 5px;
      border-radius: 8px;
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