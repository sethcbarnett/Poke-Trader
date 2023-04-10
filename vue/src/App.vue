<template>
  <div id="app">
    <div id="nav">
      <router-link v-bind:to="{ name: 'home' }">Home</router-link>&nbsp;|&nbsp;
      <router-link v-bind:to="{ name: 'logout' }" v-if="$store.state.token != ''">Logout</router-link>
    </div>
    <router-view />
  </div>
</template>

<style>
/* TODO: Style the application */
#top {
  background-color: #ee1515;
  color: #222224;
}
#middle {
  background-color: #222224;
  color: white;
}
#bottom {
  background-color: #f0f0f0;
  color: #222224;
}
</style>