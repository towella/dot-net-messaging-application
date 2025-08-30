<script lang="ts">
  import { defineComponent } from 'vue'

  export default defineComponent({
    data() {
      return {
      }
    },
    props: {
      title: String,
    }
  });
</script>

<template>
  <div class="widget">
    <h2>{{ title }}</h2>
    <slot></slot>
  </div>
</template>

<style scoped>
  .widget {
      width: 100%;
      height: auto;
      margin: 10px;
      padding: 10px;
      background-color: var(--widgetBackground);
      backdrop-filter: blur(10px);
      border: var(--widgetBorder) 2px solid;
      border-radius: 20px;
      display: inline-block;
      text-align: center;
      min-height: 160px;
      box-shadow: 0 4px 20px 8px var(--widgetShadow) inset;
      transition: margin 0.1s;
  }

  .widget:hover {
    margin: 0px;
    transition: margin 0.2s;
  }

  h2 {
    font-size: 26px;
  }
</style>