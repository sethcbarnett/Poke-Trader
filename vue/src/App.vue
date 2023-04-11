<template>
  <div id="app">
    <link href="https://fonts.cdnfonts.com/css/pokemon-solid" rel="stylesheet">
    <div id="nav">
      <router-link class="nav-link" v-bind:to="{ name: 'home' }">Home</router-link>&nbsp;|&nbsp;
      <router-link class="nav-link" v-bind:to="{ name: 'logout' }" v-if="$store.state.token != ''">Logout</router-link>
    </div>
    <router-view />
  </div>
</template>

// <script>
// import { defineComponent } from '@vue/composition-api'

// export default defineComponent({
//   setup() {
    
//   },
// })
// </script>

<style>
@import url('https://fonts.cdnfonts.com/css/pokemon-solid');
  #app {
    display: flex;
    justify-content: center;
    font-family: 'Pokemon Solid', sans-serif;
    overflow: auto;
  }
  #nav {
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #3466af;
    color: #ffcb05;
    padding: none;
    position: fixed;
    width: 100vw;
    top: 0;
    font-size: 1.5em;
  }
  .nav-link {
    color: #ffcb05;
  }
  


</style>
