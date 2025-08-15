<script setup lang="ts">
  //import HelloWorld from './components/HelloWorld.vue'
</script>

<template>
  <nav>
    <ul>
      <li><router-link :to="{name:'Login'}">Login</router-link></li>
      <li><router-link :to="{name:'Home'}">Home</router-link></li>
    </ul>
  </nav>

  <router-view/>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
