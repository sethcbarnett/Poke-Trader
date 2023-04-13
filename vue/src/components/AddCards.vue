<template>
  <div id = "container">
    <form id = "search-form" @submit.prevent ="submitSearch" >
        <input v-model = "searchString" id = "search-bar" type = "text" placeholder="Search for Pokemon to catch by name..."/>
        <input id = "search-button" type ="submit" value="Search"/>
    </form>
    <div id="searched-cards-area">
      <searched-card-display v-bind:searchedCard = "searchedCard" v-for="searchedCard in $store.state.searchedCardResult" v-bind:key="searchedCard.id"/>
    </div>
  </div>
</template>

<script>
import SearchService from "../services/SearchService.js";
import SearchedCardDisplay from "./SearchedCardDisplay.vue"
export default {
    name: "add-cards",
    data() {
        return {
            searchString: ""
        }
    },
    methods: {
        submitSearch() {
            SearchService.getCardsBySearch(`name:${this.searchString}`).then((response) => {
                this.$store.commit('SET_SEARCHED_CARDS', response.data);
            });
        }
    },
    components: {
        SearchedCardDisplay
    },
    created() {
        
    }
}
</script>

<style>
#container {
    display: flex;
}
#searched-cards-area{
    display:flex;
    flex-wrap: wrap;
}
#search-bar {
    height: 30px;
    border-radius: 5px;
    width: 364px;
    margin-right: 10px;
    margin-left: 10px;
}
#search-form {
    align-self: center;
}
#search-button {
    background-color: #ffcb05;
    color: #3466af;
    border-color: #3466af;
    border-radius: 5px;
    height: 36px;
    width: 75px;
    cursor: pointer;
}
</style>