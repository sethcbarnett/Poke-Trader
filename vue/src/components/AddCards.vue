<template>
  <div>
    <form id = "search-form" @submit.prevent ="submitSearch" >
        <input v-model = "searchString" id = "search-bar" type = "text" placeholder="Search for Pokemon to catch..."/>
        <input type ="submit" value="Search"/>
    </form>
    <div id="searched-cards-area">
      <searched-card-display v-bind:searchedCard = "searchedCard" v-for="searchedCard in cardSearchResult" v-bind:key="searchedCard.id"/>
    </div>
  </div>
</template>

<script>
import SearchService from "../services/SearchService.js";
//import SearchedCardDisplay from "./SearchedCardDisplay.vue"
export default {
    name: "add-cards",
    data() {
        return {
            cardSearchResult: {},
            searchString: ""
        }
    },
    methods: {
        submitSearch() {
            SearchService.getCardsBySearch(this.searchString).then((response) => {
                this.cardSearchResult = response.data;
            });
        }
    },
    components: {
        //SearchedCardDisplay
    }

}
</script>

<style>

</style>