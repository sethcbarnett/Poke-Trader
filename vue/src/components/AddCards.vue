<template>
  <div id = "container">
    <search-filters/>
    <div id="searched-cards-area">
      <searched-card-display v-bind:searchedCard = "searchedCard" v-for="searchedCard in $store.state.searchedCardResult" v-bind:key="searchedCard.id"/>
    </div>
  </div>
</template>

<script>
import SearchService from "../services/SearchService.js";
import SearchedCardDisplay from "./SearchedCardDisplay.vue"
import SearchFilters from "./SearchFilters.vue"
export default {
    name: "add-cards",
    data() {
        return {
            searchString: ""
        }
    },
    methods: {
        submitSearch() {
            SearchService.getCardsBySearch(`name:${this.getCompleteFilterString}`).then((response) => {
                this.$store.commit('SET_SEARCHED_CARDS', response.data);
            });
        }
    },
    components: {
        SearchedCardDisplay,
        SearchFilters
    },
    computed: {
        // getNameFilterString() {
        //     return `name: ${SearchFilters.nameSearch} `;
        // },
        // getPriceFilterString() {
        //     return `tcgplayer.prices.normal.mid:[${SearchFilters.minPrice} TO ${SearchFilters.maxPrice}] `
        // },
        // getRarityFilterString() {
        //     var rarityFilterString = "";
        //     if (SearchFilters.rarities.includes('common'))
        //     {
        //         rarityFilterString += "rarities:Common ";
        //     }
        //     else if (SearchFilters.rarities.includes('uncommon'))
        //     {
        //         if (SearchFilters.rarities.includes('common'))
        //         {
        //             rarityFilterString += 'OR '
        //         }
        //         rarityFilterString += "rarities:Uncommon ";
        //     }
        //     else
        //     {
        //         if (SearchFilters.rarities.includes('common') || SearchFilters.rarities.includes('uncommon'))
        //         {
        //             rarityFilterString += 'OR '
        //         }
        //         for (var rarity in SearchService.apiSearchRares)
        //         {
        //             rarityFilterString += `rarities:${rarity} OR `;
        //         }
        //     }
        //     rarityFilterString = rarityFilterString.substring(0, rarityFilterString.length-2);
        //     return rarityFilterString;
        // },
        // getCompleteFilterString() {
        //     var completeFilterString = this.getNameFilterString + this.getPriceFilterString + this.getRarityFilterString;
        //     console.log(completeFilterString);
        //     return completeFilterString;
        // }
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