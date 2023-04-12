<template>
  <div class="home">
    <div>
      <h1>Home</h1>
    </div>
    <div>
      <router-link @click="this.$store.state.isSearchedUser = true" v-for="collection in publicCollections" v-bind:key="collection" :to="{ name: 'collection', params: {username: `${collection.username}`} }">{{ collection.username }}</router-link>
    </div>
  </div>
</template>

<script>
import CollectionService from "../services/CollectionService.js";

export default {
  name: "home",
  data() {
    return {
      publicCollections: []
    }
  },
  methods: {
    getPublicCollectionUsers() {
      CollectionService.getPublicCollectionUsers().then((response) => {
        this.publicCollections = response.data;
      });
    },
    switchSearchedUser() {
      this.$store.commit('MAKE_SEARCHED_USER_TRUE');
    }
  },
  created() {
    this.getPublicCollectionUsers();
  }
};
</script>

<style>
.home {
  margin-top: 20px;
}
h1 {
  text-align: center;
}
p {
  text-align: center;
}
</style>