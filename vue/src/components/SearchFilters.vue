<template>
  <form id="search-form">
    <div id="search-name" @submit.prevent="submitSearch">
        <button id = "show-hide-filters" @click.prevent="showHideFilters">{{(filterVisibility ? "Hide  Filters" : "Show Filters")}}</button>
      <input
        v-model="searchString"
        id="search-bar"
        type="text"
        placeholder="Search for Pokemon to catch by name..."
      />
      <input id="search-button" type="submit" value="Search" />
    </div>
    <div id="search-filters" v-show="filterVisibility">
      <div id="price-filter">
        <p>Filter results by price:</p>
        <div id="min-max-input">
          <label for="min-price" name="min-price">Min: $</label>
          <input id="min-price" name="min-price" type="text" />
          <label for="max-price" name="max-price">Max: $</label>
          <input id="max-price" type="text" name="max-price" />
        </div>
      </div>
      <div id="rarity-filter">
        <p>Filter results by rarity:</p>
        <div id="rarity-checkboxes">
          <div id="common" class="rarity">
            <p>COMMON</p>
            <div class="symbol-and-checkbox">
              <p>●</p>
              <input type="checkbox" checked />
            </div>
          </div>
          <div id="uncommon" class="rarity">
            <p>UNCOMMON</p>
            <div class="symbol-and-checkbox">
              <p>◆</p>
              <input type="checkbox" checked />
            </div>
          </div>
          <div id="rare" class="rarity">
            <p>RARE</p>
            <div class="symbol-and-checkbox">
              <p>🟊</p>
              <input type="checkbox" checked />
            </div>
          </div>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
export default {
  name: "search-filters",
  data() {
      return {
          filterVisibility: false
      }
  },
  methods: {
      showHideFilters() {
          this.filterVisibility = !this.filterVisibility;
      }
  }
};
</script>

<style>
button {
    width: 60px;
    
}
#show-hide-filters {
background-color: #ffcb05;
    color: #3466af;
    border-left: solid 2px #3466af;  
    border-top: solid 2px #3466af; 
    border-right: solid 2px black; 
    border-bottom: solid 2px black; 
    border-radius: 5px;
     
            
            
            

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
#min-max-input label{
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
</style>