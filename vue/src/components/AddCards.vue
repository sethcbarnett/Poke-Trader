<template>
  <div id = "container">
    <search-filters :searchType="this.searchType" :isSearching="this.isSearching" @setIsSearching="(isSearching) => this.isSearching = isSearching"/>
    <div v-if="$store.state.resultsExist" v-show="this.isSearching" id="searched-cards-area">
      <searched-card-display v-bind:searchedCard = "searchedCard" v-for="searchedCard in $store.state.searchedCardResult" v-bind:key="searchedCard.id"/>
    </div>
    <div id = "no results found" v-if="!$store.state.resultsExist">
          <h1>No pokemon here...</h1>
          <p>Try reworking your search or checking your spelling</p>
    </div>
  </div>
</template>

<script>
import SearchedCardDisplay from "./SearchedCardDisplay.vue";
import SearchFilters from "./SearchFilters.vue";
export default {
    name: "add-cards",
    data() {
        return {
            searchString: "",
            searchType: "apicall",
            resultsExist: true,
            isSearching: false
        }
    },
    components: {
        SearchedCardDisplay,
        SearchFilters
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
    justify-content:center;
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
    font-size: 11px
}
</style>