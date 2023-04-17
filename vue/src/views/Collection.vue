<template>
  <div id="collection-area">
    <search-filters :searchType="searchType" :isSearching="this.isSearching" @setIsSearching="(isSearching) => this.isSearching = isSearching"/>
    <div class="options">
      <collection-stats />
      <premium-button v-if="$store.state.isPremium == false && $store.state.isLoginUser" />
    </div>
    <div id="card-display-area" v-show="this.isSearching">
      <card-display 
        v-bind:collectionItem="collectionItem"
        v-for="collectionItem in $store.state.filteredCollection"
        v-bind:key="collectionItem.card.id"
        @set-quantity ="(quantity) => setQuantity(collectionItem, quantity)"
        @set-quantity-for-trade ="(quantity) => setQuantityForTrade(collectionItem, quantity)"
        @set-grade="(grade) => setGrade(collectionItem, grade)"
      />
    </div>
    <div id="card-display-area" v-show="!this.isSearching">
      <card-display
        v-bind:collectionItem="collectionItem"
        v-for="collectionItem in $store.state.currentCollectionObject"
        v-bind:key="collectionItem.card.id"
        @set-quantity ="(quantity) => setQuantity(collectionItem, quantity)"
        @set-quantity-for-trade ="(quantity) => setQuantityForTrade(collectionItem, quantity)"
        @set-grade="(grade) => setGrade(collectionItem, grade)"
      />
    </div>
    <div id="collection-and-search-options" v-if="$store.state.isLoginUser">
      <p>Collection Visibility</p>
      <div class="switch-container">
        <p>Private</p>
        <label class="switch">
          <input
            @change="toggleVisibility"
            type="checkbox"
            v-model="this.$store.state.user.isPublic"
          />
          <span class="slider round"></span>
        </label>
        <p>Public</p>
      </div>
      <add-cards/>
      <div id="spacer" />
    </div>
  </div>
</template>

<script>
import CardDisplay from "../components/CardDisplay.vue";
import AddCards from "../components/AddCards.vue";
import authService from "../services/AuthService.js";
import CollectionStats from "../components/CollectionStats.vue";
import PremiumButton from "../components/PremiumButton.vue";
import SearchFilters from '../components/SearchFilters.vue';
import CollectionService from '../services/CollectionService.js';
export default {
  name: "collection",
  components: {
    CardDisplay,
    AddCards,
    CollectionStats,
    PremiumButton,
    SearchFilters
  },
  data() {
    return {
      user: {
        username: "",
        isPublic: this.$store.state.user.username,
      },
      collectionItem: {},
      searchType: "filterCollection",
      isLoginUser: null,
      isSearching: false
    };
  },
  methods: {
    redirectToPremium() {
      this.$store.commit('TOGGLE_SEARCHING_OFF');
      this.$router.push({ name: "premium" });
    },
    checkForPremium() {
      if (this.$store.state.user.isPremium == true) {
        this.$store.commit("SET_PREMIUM");
      }
    },
    checkForPublic() {
      if (this.$store.state.user.isPublic == true) {
        this.$store.commit("TOGGLE_VISIBILITY");
      }
    },
    toggleVisibility() {
      authService
        .toggleVisibility(this.$store.state.user.username)
        .then((response) => {
          if (response.status == 200) {
            this.$store.commit("SWITCH_PUBLIC");
          } else {
            console.log("NO SIR");
          }
        });
    },
    setQuantity(collectionItem, quantity)
    {
      if (Number.isFinite(quantity))
      {
        collectionItem.quantity = quantity;
        if (quantity <= collectionItem.quantityForTrade)
        { 
          collectionItem.quantityForTrade = quantity;
        }
        let username = this.$store.state.user.username;
        CollectionService.updateCard(collectionItem, username)
      }
      else 
      {
        alert("Please input a number");
      }
    },
    setQuantityForTrade(collectionItem, quantity)
    {
      if (Number.isFinite(quantity) && quantity <= collectionItem.quantity)
      {
        collectionItem.quantityForTrade = quantity;
        let username = this.$store.state.user.username;
        CollectionService.updateCard(collectionItem, username)
      }
      else if (Number.isFinite(quantity))
      {
        alert("You don't have that many in your collection")
      }
      else 
      {
        alert("Please input a number");
      }
    },
    setGrade(collectionItem, grade)
    {
      console.log(grade);
      collectionItem.grade = grade;
      let username = this.$store.state.user.username;
      CollectionService.updateCard(collectionItem, username).then((response) => {
        console.log(response);
      }).catch((error) => {
        console.log(error);
      });
    }
  },
  created() {
    this.checkForPremium();
    this.checkForPublic();
  },
};
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
  overflow: auto;
}
#card-display-area {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
}

button {
  background-color: #3466af;
  color: white;
  border: none;
  border-radius: 5px;
  font-family: "Pokemon Solid", sans-serif;
  line-height: 1.5em;
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
#collection-and-search-options > p {
  font-size: 14px;
  margin-bottom: 0px;
}
.switch {
  display: inline-block;
  position: relative;

  width: 60px;
  height: 34px;
}
.switch-container {
  display: flex;
  justify-content: space-around;
  align-items: center;
  text-align: center;
  flex-direction: row;
  padding-bottom: 10px;
}
.switch-container p {
  font-size: 12px;
  padding: 8px;
}
.switch input {
  opacity: 0;
  width: 0;
  height: 0;
}
.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: 0.4s;
  transition: 0.4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 26px;
  width: 26px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  -webkit-transition: 0.4s;
  transition: 0.4s;
}

input:checked + .slider {
  background-color: #3466af;
}

input:focus + .slider {
  box-shadow: 0 0 1px #3466af;
}

input:checked + .slider:before {
  -webkit-transform: translateX(26px);
  -ms-transform: translateX(26px);
  transform: translateX(26px);
}

/* Rounded sliders */
.slider.round {
  border-radius: 34px;
}

.slider.round:before {
  border-radius: 50%;
}
#spacer {
  margin: 20px;
}
.options {
  background-color: rgb(207, 200, 177);
  border: 2px solid black;
  border-radius: 20px;
  padding: 20px;
  display: flex;

  margin: 20px;
  padding-bottom: 5px;
  padding-top: 5px;
  justify-content: space-between;
  flex-direction: row;
}
collection-stats {
  flex-basis: 33%;
}
#collection-and-search-options {
  display: flex;
  align-items: center;
}
</style>