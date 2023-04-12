<template>
  <div class="home">
    <div>
      <h1>Home</h1>
    </div>
    <div>
      <router-link v-for="collection in publicCollections" v-bind:key="collection.id" :to="{ name: 'collection', params: {username: `${collection.username}`} }" >
        <span @click="switchSearchedUser(collection.username)">{{ collection.username }}</span></router-link>
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
      console.log(this.publicCollections);
    },
    switchSearchedUser(username) {
      this.$store.commit('SET_CURRENT_COLLECTION', username);
      this.$store.commit('SET_CURRENT_COLLECTION_OBJECT');
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