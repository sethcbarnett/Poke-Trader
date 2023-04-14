<template>
  <div class="home">
    <div id="testing">
      <search-filters />
    </div>
    <div>
      <h1>Home</h1>
    </div>
    <div id="content-container">
      <div id="public-users">
        <router-link v-for="collection in publicCollections" 
                v-bind:key="collection.id" 
                :to="{
                  name: 'collection',
                  params: { username: `${collection.username}` },
                }">
                <span id="public-user" @click="switchSearchedUser(collection.username)">
                  {{ collection.username }}</span>
        </router-link>
      </div>
      <div id="pokemon-picture">
          <img :src="imgSrc" @click="generateImg"/>
      </div>
      <!-- <router-link
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
      > -->
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
      imgSrc: '',
      randomNumber: ''
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
      let payload = {name:"", minPrice:0, maxPrice:200000, rarity:["common", "uncommon", "rare"]};
      this.$store.commit('SET_FILTERED_COLLECTION_OBJ', payload);
    },
    generateImg() {
      this.randomNumber = Math.random()*10;
      this.imgSrc = 'https://lorempokemon.fakerapi.it/pokemon/200/' + this.randomNumber;
    }
  },
  created() {
    this.getPublicCollectionUsers();
    this.generateImg();
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
#content-container {
  display: flex;
  height: 500px;
  justify-content: space-around;
}
#public-users {
  display: flex;
  flex-direction: column;
  flex-basis: 50%;
  align-items: center;
  justify-content: center;
  border-radius: 100px;
  color: #3466af;
  background-color: #ffcb05;
  border-width: 20px;
  border-style: solid;
  border-color: #3466af;
  font-size: 1.25em;
  padding-bottom: 10px;
  line-height: 0.55em;
  justify-content: space-evenly;
  text-align: center;
  line-height: 25px;
  position: relative;
  margin-right: 5px;
  margin-left: 5px;
  max-width: 500px;
  max-height: 500px;
}
#pokemon-picture {
  display: flex;
  flex-direction: column;
  flex-basis: 50%;
  align-items: center;
  justify-content: center;
  border-radius: 100px;
  max-width: 500px;
  max-height: 500px;
  color: #3466af;
  background-color: #ffcb05;
  border-width: 20px;
  border-style: solid;
  border-color: #3466af;
  font-size: 1.25em;
  padding-bottom: 10px;
  line-height: 0.55em;
  justify-content: space-evenly;
  text-align: center;
  line-height: 25px;
  position: relative;
  margin-left: 5px;
  margin-right: 5px;
}
img {
  height: 550px;
  width: 550px;
}
</style>