<template>
  <div>
    <div id="card-display-area">
      <card-display v-for="card in collection" v-bind:key="card.id"/>
      
    </div>
  </div>
</template>

<script>
import CardDisplay from "../components/CardDisplay.vue";

export default {
    name: "collection",
    components: {
        CardDisplay
    },
    created (){
        if(!this.$store.state.isSearchedUser){
        var currentUser = this.$store.state.user.username;
        this.collection = this.$route.getCollectionByUser(currentUser)
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