<template>
  <div id="app">
    <link href="https://fonts.cdnfonts.com/css/pokemon-solid" rel="stylesheet">
    <div id="nav">
      <router-link class="nav-link" v-bind:to="{ name: 'home' }">Home</router-link>
      <router-link class="nav-link" v-bind:to="{name: 'collection', params: {username: `${this.$store.state.user.username}`}}" @click.native="SetCollectionToCurrentUser" v-if="$store.state.token != ''">&nbsp;|&nbsp;My Collection</router-link>
      <router-link class="nav-link" v-bind:to="{ name: 'trade' }" @click.native ="ClearTrade" v-if="$store.state.token != ''">&nbsp;|&nbsp;Trade</router-link>
      <router-link class="nav-link" v-bind:to="{ name: 'logout' }" v-if="$store.state.token != ''">&nbsp;|&nbsp;Logout</router-link>
      <router-link class="nav-link" v-bind:to="{ name: 'login' }" v-if="$store.state.token == ''">&nbsp;|&nbsp;Login</router-link>
    </div>
    <div id = "nav-spacer">
    </div>
    <router-view />
  </div>
</template>

<script>
export default {
  methods: {
    SetCollectionToCurrentUser(){
      this.$store.commit('SET_CURRENT_COLLECTION', this.$store.state.user.username);
      this.$store.commit('SET_CURRENT_COLLECTION_OBJECT')
      let payload = {name:"", minPrice:0, maxPrice:200000, rarity:["common", "uncommon", "rare"]};
      this.$store.commit('SET_FILTERED_COLLECTION_OBJ', payload);
      this.$store.commit('CHECK_IF_IS_LOGIN_USER');
    },
    ClearTrade() {
      this.$store.commit("CLEAR_TRADE");
    }
  }
}
</script>


<style>
@import url('https://fonts.cdnfonts.com/css/pokemon-solid');
  #app {
    height: 100vh;
    display: flex;
    flex-direction: column;
    font-family: 'Pokemon Solid', sans-serif;
    letter-spacing: 3px;
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
    text-decoration: none;
  }
  #nav-spacer {
    margin: 34px;
  }



</style>
