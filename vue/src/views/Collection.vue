<template>
  <div>
    <div id="card-display-area">
      <card-display v-bind:collectionItem = "collectionItem" v-for="collectionItem in collection" v-bind:key="collectionItem.card.id"/>
    </div>
    <footer>
      <p>Standard users can have up to 100 unique cards in their collection.</p>
    </footer>
  </div>
</template>

<script>
import CardDisplay from "../components/CardDisplay.vue";
import CollectionService from "../services/CollectionService.js";

export default {
    name: "collection",
    components: {
        CardDisplay
    },
    created (){
        if(!this.$store.state.isSearchedUser){
          var currentUser = this.$store.state.user.username;
          CollectionService.getCollectionByUser(currentUser).then((response) => {
            this.collection = response.data;
          })
        }
    },
    data () {
        return {
          collection: {}
        }
    }
}
</script>

<style scoped>
div {
  display: flex;
  font-family: sans-serif;
}
</style>