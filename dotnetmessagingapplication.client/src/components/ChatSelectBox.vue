<script lang="ts">
  import { defineComponent } from 'vue'

  export default defineComponent({
    data() {
      return {
      }
    },
    props: {
      chatName: String,
      lastMessage: String,
    }
  });
</script>

<template>
  <div class="chat-select-box">
    <div id="chat-select-box-text">
        <h3>{{ chatName }}</h3>
        <p>{{ lastMessage }}</p>
    </div>
    <button>More actions</button>
  </div>
</template>

<style scoped>
  .chat-select-box {
    display: flex;
    justify-content: space-between;
    margin: 10px;
    padding: 10px;
    background-color: var(--darkColour);
    backdrop-filter: blur(10px);
    border: var(--widgetBorder) 2px solid;
    border-radius: 14px;
    box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
    transition: margin 0.1s;
  }

  .chat-select-box:hover {
    margin: 0px;
    transition: margin 0.2s;
  }

  #chat-select-box-text {
    display: flex;
    flex-direction: column;
    text-align: left;
  }

  h3 {
    font-size: 20px;
    padding: 0;
    margin: 5px;
  }

  p {
    padding: 0;
    margin: 5px;
  }
</style>