<template>
  <div id="collection-area">
    <div id="card-display-area">
      <card-display v-bind:collectionItem = "collectionItem" v-for="collectionItem in collection" v-bind:key="collectionItem.card.id"/>
    </div>
    <footer>
      <p>Standard users can have up to 100 unique cards in their collection.</p>
      <button id="go-premium">Go Premium!</button>
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
  flex-direction: column;
}
#collection-area {
  height: 100vh;
  width: 100vw;
}
#card-display-area {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}
footer {
  display: flex;
  align-self: center;
  flex-direction: column;
}
button {
  background-color: #3466af;
  color: white;
  border: none;
  border-radius: 5px;
  font-family: 'Pokemon Solid', sans-serif;
  text-align: center;
  text-justify: auto;
  letter-spacing: 1px;
  padding-bottom: 5px;
  display: inline-block;
  cursor: pointer;
  max-width: 400px;
  align-self: center;
  padding-left: 20px;
  padding-right: 20px;
}
</style>