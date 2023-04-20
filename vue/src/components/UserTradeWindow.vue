<template>
  <div id="user-window">
    <h3>{{ this.username }} will give:</h3>
    

    <div class="cards-for-trade" v-if="!this.$store.state.isPendingTrade">
      <div class="trades-scroll">
        <simplified-card-display
          v-bind:availableCard="availableCard"
          v-bind:username="username"
          :clickAction="MakeCardUnproposed"
          v-for="availableCard in CheckProposedCards"
          v-bind:key="availableCard.card.id"
        />
      </div>
    </div>

    <div class="cards-for-trade" v-if="this.$store.state.isPendingTrade">
      <div class = "trades-scroll">
        <simplified-card-display
          v-bind:availableCard="availableCard"
          v-bind:username="username"
          :clickAction="DoNothing"
          v-for="availableCard in CheckProposedCards"
          v-bind:key="availableCard.card.id"
        />
      </div>
    </div>

    <h3>Est. Value: ${{ AddAllCardValues }}</h3>

    <div id="collection" v-if="!this.$store.state.isPendingTrade">
      <div id="collection-scroll">
        <simplified-card-display
          v-bind:availableCard="availableCard"
          v-bind:username="username"
          :clickAction="MakeCardProposed"
          v-for="availableCard in CheckAvailableCards"
          v-bind:key="availableCard.card.id"
        />
      </div>
    </div>

    
  </div>
</template>

<script>
import SimplifiedCardDisplay from "./SimplifiedCardDisplay.vue";

export default {
  name: "user-trade-window",
  props: ["username"],
  components: { SimplifiedCardDisplay },
  data() {
    return {
      availableCard: {},
      newUsername: "",
      MakeCardProposed: "MakeCardProposed",
      MakeCardUnproposed: "MakeCardUnproposed",
      DoNothing: "DoNothing",
    };
  },
  methods: {
    GetAvailableCards() {
      this.$store.commit("SET_USER_INFO", this.$store.state.user.username);
    },

    SetNewUsername() {
      this.newUsername = this.username;
    },
  },
  created() {
    this.GetAvailableCards();
    this.SetNewUsername();
  },
  computed: {
    CheckAvailableCards() {
      let cards = [];
      if (this.username == this.$store.state.user.username) {
        cards = this.$store.state.loginUserAvailableCards;
      } else {
        cards = this.$store.state.otherUserAvailableCards;
      }
      return cards;
    },
    CheckProposedCards() {
      let cards = [];
      if (this.username == this.$store.state.user.username) {
        cards = this.$store.state.loginUserProposedCards;
      } else {
        cards = this.$store.state.otherUserProposedCards;
      }
      return cards;
    },
    AddAllCardValues() {
      let cardsValue = 0;
      for (let card of this.CheckProposedCards) {
        cardsValue += Number(card.card.price);
      }
      return cardsValue.toFixed(2);
    },
  },
};
</script>

<style scoped>
#user-window {
  border-style: solid;
  margin: 5px;
  padding-bottom: 2px;
  flex-basis: 50%;
  height: 95%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-around;
  color: black;
  background: rgba(52, 102, 175, 0.85);
  max-width: 50%;
}
.cards-for-trade {
  flex-basis: 50%;
  border-style: solid;
  width: 90%;
  max-height: 50%;
  color: black;
  border-width: 2px;
  border-radius: 10px;
  background-color: rgb(250, 218, 92);
  overflow: visible;
}
#collection {
  flex-basis: 30%;
  border-style: solid;
  width: 90%;
  color: black;
  border-width: 2px;
  border-radius: 10px;
  min-height: 213px;
  gap: 2px;
  max-width: 90%;
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  background-color: rgb(250, 218, 92);
  
}
h4 {
  margin: 0;
  align-self: center;
  margin-left: 5px;
  flex-basis: 2%;
  font-size: .7em;
}
h3 {
  margin: 0;
  flex-basis: 5%;
}

#collection-scroll::-webkit-scrollbar {
  width: 20px;
}

#collection-scroll::-webkit-scrollbar-track {
  background: #ffcb05;
  border-radius: 20px;
  
}

#collection-scroll::-webkit-scrollbar-thumb {
  background-color: #3466af;
  border-radius: 20px;
  border: 3px solid orange;
}
#collection-scroll {
  display: flex;
  justify-content: flex-start;
  min-width: 35.2vw;
  align-self: flex-end;
  flex-direction: row;
  overflow-y: hidden;
  overflow-x: scroll;
  max-width: 36vw;
  flex-wrap: nowrap;
  

}
.trades-scroll::-webkit-scrollbar {
  width: 20px;
}

.trades-scroll::-webkit-scrollbar-track {
  background: #ffcb05;
  border-radius: 20px;
}

.trades-scroll::-webkit-scrollbar-thumb {
  background-color: #3466af;
  border-radius: 20px;
  border: 3px solid orange;
}
.trades-scroll {
  display: flex;
  justify-content: center;
  align-content: flex-start;
  flex-direction: row;
  overflow-y: scroll;
  overflow-x: hidden;
  width: 100%;
  height: 100%;
  flex-wrap: wrap;
}
#user-window {
  border-width: 3px;
  border-radius: 10px;
}
</style>