<template>
  <form id="search-form">
    <div id="search-name" v-on:submit.prevent="submitSearch">
      <button id="clear-search" v-show="$store.state.isSearching" @click.prevent="clearSearch">Clear Search
      </button>
      <button id="show-hide-filters" @click.prevent="showHideFilters">
        {{ filterVisibility ? "Hide Filters" : "Show Filters" }}
      </button>
      <input
        v-model="nameSearch"
        id="search-bar"
        type="text"
        placeholder="Search for Pokemon to catch by name..."
      />
      <input id="search-button" type="submit" @click.prevent="submitSearch" value="Search" />
    </div>
    <div id="search-filters" v-show="filterVisibility">
      <div id="price-filter">
        <p>Filter results by price:</p>
        <div id="min-max-input">
          <label for="min-price" name="min-price">Min: $</label>
          <input id="min-price" name="min-price" type="text" v-model="minPrice"/>
          <label for="max-price" name="max-price">Max: $</label>
          <input id="max-price" type="text" name="max-price" v-model="maxPrice"/>
        </div>
      </div>
      <div id="rarity-filter">
        <p>Filter results by rarity:</p>
        <div id="rarity-checkboxes">
          <div id="common" class="rarity">
            <p>COMMON</p>
            <div class="symbol-and-checkbox">
              <p>●</p>
              <input type="checkbox" checked v-model="rarities" value="common"/>
            </div>
          </div>
          <div id="uncommon" class="rarity">
            <p>UNCOMMON</p>
            <div class="symbol-and-checkbox">
              <p>◆</p>
              <input type="checkbox" checked v-model="rarities" value="uncommon"/>
            </div>
          </div>
          <div id="rare" class="rarity">
            <p>RARE</p>
            <div class="symbol-and-checkbox">
              <p>🟊</p>
              <input type="checkbox" checked v-model="rarities" value="rare"/>
            </div>
          </div>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
import SearchService from "../services/SearchService.js";
export default {
  name: "search-filters",
  props: ['searchType'],
  data() {
    return {
    isSearching: false,
    filterVisibility: false,
      nameSearch: "",
      minPrice: 0,
      maxPrice: 200000,
      rarities: ["common", "uncommon", "rare"],
    };
  },
  computed: {
      getNameFilterString() {
          if (this.nameSearch.length > 0)
          {
            return `name:*${this.nameSearch}* `;
          }
          else return "";
        },
        getPriceFilterString() {
            return `(tcgplayer.prices.normal.mid:[${this.minPrice} TO ${this.maxPrice}] OR tcgplayer.prices.holofoil.mid:[${this.minPrice} TO ${this.maxPrice}] OR cardmarket.prices.averageSellPrice:[${this.minPrice} TO ${this.maxPrice}]) `
        },
        getRarityFilterString() {
            var rarityFilterString = "";
            rarityFilterString += "("
            if (this.rarities.includes('common'))
            {
                rarityFilterString += "rarity:Common ";
            }
            if (this.rarities.includes('uncommon'))
            {
                if (this.rarities.includes('common'))
                {
                    rarityFilterString += 'OR '
                }
                rarityFilterString += "rarity:Uncommon ";
            }
            if (this.rarities.includes('rare'))
            {
                if (this.rarities.includes('common') || this.rarities.includes('uncommon'))
                {
                    rarityFilterString += 'OR '
                }
                rarityFilterString += `rarity:*r* OR rarity:*l* OR rarity:*v*`;
            }
            rarityFilterString += ")"
            return rarityFilterString;
        },
        getCompleteFilterString() {
            var completeFilterString = this.getNameFilterString + this.getPriceFilterString + this.getRarityFilterString;
            return completeFilterString;
        }
  },
  methods: {
    showHideFilters() {
      this.filterVisibility = !this.filterVisibility;
    },
    submitSearch() {
      if (this.searchType == 'apicall'){
        this.submitSearchToApi();
        this.$store.commit('TOGGLE_SEARCHING_ON');
      }
      else if (this.searchType == 'filterCollection'){
        this.filterCollection();
      }
    },
    submitSearchToApi() {
        SearchService.getCardsBySearch(`${this.getCompleteFilterString}`).then((response) => {
                this.$store.commit('SET_SEARCHED_CARDS', response.data);
            });
    },
    clearSearch() {
      this.$store.commit('TOGGLE_SEARCHING_OFF');
    filterCollection() {
      this.$store.commit('SET_FILTERED_COLLECTION_OBJ', {name:this.nameSearch, minPrice:this.minPrice, maxPrice:this.maxPrice, rarity:this.rarities});
    }
  },
};
</script>

<style>
button {
    width: 60px;
    
}
#show-hide-filters {
background-color: #ffcb05;
    background-color: #ffcb05;
    color: #3466af;
    border: solid #3466af 2px;
    border-radius: 5px;
    height: 36px;
    width: 75px;
    cursor: pointer;
    font-family: sans-serif;
    font-size: 11px;
}

#search-name {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

#search-form {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  margin: 5px;
}

#search-filters {
  font: sans-serif;
  display: flex;
  flex-direction: row;
}
#search-filter > div {
  margin-right: 0px;
}
/* Rarity Filter Styling */
#rarity-filter {
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  border: solid black 2px;
  border-radius: 5px;
  width: 255px;
  height: 80px;
  margin-left: 10px;
}
#rarity-filter > p {
  margin-bottom: 0px;
  margin-top: 3px;
  font-size: 15px;
}
#rarity-checkboxes {
  display: flex;
  flex-direction: row;
}
#rarity-checkboxes > div {
  margin: 4px;
}
#rarity-checkboxes > div > p {
  font-size: 10px;
  margin-bottom: 0px;
  text-decoration: underline;
}
#common.rarity {
  margin: 0px;
}
#uncommon.rarity {
  margin: 0px;
}
#rare.rarity {
  margin: 0px;
}
.rarity {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0px;
  padding: 2px;
  padding-left: 4px;
  padding-right: 4px;
}
.symbol-and-checkbox {
  display: flex;
  flex-direction: row;
  height: 35px;
  align-items: center;
  margin: 0px;
}
#common .symbol-and-checkbox p {
  font-size: 30px;
  margin: 0px;
  margin-bottom: 3px;
}
#uncommon .symbol-and-checkbox p {
  font-size: 22px;
  margin: 0px;
  margin-top: 2px;
}
#rare .symbol-and-checkbox p {
  font-size: 25px;
  margin: 0px;
  margin-bottom: 3px;
}

/* Price Filter Styling */
#price-filter {
  display: flex;
  flex-direction: column;
  border: solid black 2px;
  border-radius: 5px;
  width: 255px;
  height: 80px;
  align-items: center;
}
#price-filter p {
  margin-top: 8px;
  margin-bottom: 8px;
}
#search-filters {
  font-family: sans-serif;
  font-weight: 700;
}
#min-max-input {
  display: flex;
}
#min-max-input label {
  font-size: 15px;
}
#min-max-input input {
  width: 60px;
  margin-bottom: 8px;
}
#min-max-input label {
  margin-left: 5px;
}
#max-price {
  margin-right: 5px;
}
#clear-search {
  margin-right: 5px;
}
</style>