<template>
  <div class="home">
    <div id = "testing">
      <search-filters/>
    </div>
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
import SearchFilters from "../components/SearchFilters.vue";
export default {
  name: "home",
  components: {
    SearchFilters
  },
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

<style scoped>
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