<template>
  <div class="home">
    <div id="testing">
      <search-filters />
    </div>
    <div>
      <h1>Home</h1>
    </div>
    <div >
      <router-link
        v-for="collection in publicCollections"
        v-bind:key="collection.id"
        :to="{
          name: 'collection',
          params: { username: `${collection.username}` },
        }"
      >
        <span id="public-user" @click="switchSearchedUser(collection.username)">{{
          collection.username
        }}</span></router-link
      >
    </div>
  </div>
</template>

<script>
import CollectionService from "../services/CollectionService.js";
import SearchFilters from "../components/SearchFilters.vue";
export default {
  name: "home",
  components: {
    SearchFilters,
  },
  data() {
    return {
      publicCollections: [],
    };
  },
  methods: {
    getPublicCollectionUsers() {
      CollectionService.getPublicCollectionUsers().then((response) => {
        this.publicCollections = response.data;
      });
      console.log(this.publicCollections);
    },
    switchSearchedUser(username) {
      this.$store.commit("SET_CURRENT_COLLECTION", username);
      this.$store.commit("SET_CURRENT_COLLECTION_OBJECT");
    },
  },
  created() {
    this.getPublicCollectionUsers();
  },
};
</script>

<style scoped>
.home {
  height: 100vh;
  width: 100vw;
  position: relative;
}
.home::before {
  content: "";
  background-image: url("../assets/pokeball wallpaper.jpg");
  background-size: cover;
  position: absolute;
  top: 0px;
  right: 0px;
  bottom: 0px;
  left: 0px;
  opacity: .45;
}
.testing{
  position: relative;

}
.public-user {
  position: relative;
}

h1 {
  text-align: center;
  position: relative;
}
p {
  text-align: center;
  
}
</style>