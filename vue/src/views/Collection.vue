<template>
  <div id="collection-area">
    <div id="card-display-area">
      <card-display
        v-bind:collectionItem="collectionItem"
        v-for="collectionItem in $store.state.currentCollectionObject"
        v-bind:key="collectionItem.card.id"
      />
    </div>
    <div id = "collection-and-search-options">
    <p>Change collection to public</p>
    <div class="switch-container">
      <label class="switch">
        <input type="checkbox" />
        <span class="slider round"></span>
      </label>
      </div>
      <div id="add-cards">
        <add-cards />
      </div>
      <footer v-if="$store.state.isPremium == false">
        <p>Standard users can have up to 100 unique cards in their collection.</p>
        <button @click="redirectToPremium" id="go-premium">Go Premium!</button>
      </footer>
    <div id = "spacer"/>
  </div>
  </div>
</template>

<script>
import CardDisplay from "../components/CardDisplay.vue";
import AddCards from "../components/AddCards.vue";

export default {
  name: "collection",
  components: {
    CardDisplay,
    AddCards,
  },
  methods: {
    redirectToPremium() {
      this.$router.push({ name: "premium" });
    },
    checkForPremium() {
      if (this.$store.state.user.isPremium == true) {
        this.$store.state.isPremium == true;
      }
    },
  },
  created() {
    this.checkForPremium();
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
  font-family: "Pokemon Solid", sans-serif;
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
.switch {
  display: inline-block;
  position: relative;
  
  
  width: 60px;
  height: 34px;
}
.switch-container {
  display: flex;
  justify-content:center;
  flex-direction: row;
  padding-bottom: 10px;
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
</style>