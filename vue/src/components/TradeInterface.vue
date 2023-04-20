<template>
  <div id="trade-interface">
      <h2>Trade with {{ $store.state.otherUserUsername }}!</h2>
      <div id="trade-windows-container">
          <user-trade-window :username="this.$store.state.user.username" />
          <user-trade-window :username="this.$store.state.otherUserUsername" />
      </div>
      <div id="button-container">
        <input v-if="!this.$store.state.isPendingTrade" type="submit" value="Propose Trade!" @click="PostTrade(MakeTradeObject)"/>
        <input v-if="DisplayAcceptTrade" type="submit" value="Accept Trade!" @click="PostTrade(MakeTradeObject)"/>
        <input v-if="DisplayAcceptTrade" type="submit" value="Reject Trade!" @click="PostTrade(MakeTradeObject)"/>
      </div>
  </div>
</template>

<script>
import UserService from "../services/UserService";
import UserTradeWindow from "./UserTradeWindow.vue";
export default {
  components: { UserTradeWindow },
  name: "trade-interface",
  data() {
    return {
      tradeObject: {
        usernameFrom: this.$store.state.user.username,
        usernameTo: this.$store.state.otherUserUsername,
        collectionItemsFrom: this.$store.state.loginUserProposedCards,
        collectionItemsTo: this.$store.state.otherUserProposedCards,
      },
    };
  },
  methods: {
    PostTrade(tradeObject) {
      UserService.postTrade(tradeObject)
        .then((response) => {
          if (response.status === 200) {
            alert(
              `Trade Requested with ${this.$store.state.otherUserUsername}`
            );
            this.$store.commit('SET_PROPOSED_TRADE_USER', tradeObject);
            this.$store.commit("SET_OTHER_USER_INFO", "");
            this.$store.commit('SET_TRADES_IN_PROGRESS');
          }
        })
        .catch((error) => {
          if (error.response.status === 400) {
            alert(
              `A proposed trade with ${this.$store.state.otherUserUsername} already exists. Please Accept it or Reject it.`
            );
          } else {
            alert("something broked");
          }
        });
    },
    DetermineTradeSender() {
      let ProposedUserTrades = this.$store.state.proposedUserTrades;
      for (let item of ProposedUserTrades)
      {
        if (item.trade.usernameFrom == this.tradeObject.usernameFrom && item.trade.usernameTo == this.tradeObject.usernameTo)
        {
          if (item.usernameFrom == this.$store.state.user.username)
          {
            return false;
          }
          else
          {
            return true;
          }
        }
      }
      return true;
    }
  },
  computed: {
    MakeTradeObject() {
        let tradeObject = {};
        tradeObject.usernameFrom = this.$store.state.user.username;
        tradeObject.usernameTo = this.$store.state.otherUserUsername;
        tradeObject.collectionItemsFrom = this.$store.state.loginUserProposedCards;
        tradeObject.collectionItemsTo = this.$store.state.otherUserProposedCards;
        return tradeObject;
    },
    DisplayAcceptTrade() {
      if (!this.$store.state.isPendingTrade)
      {
        return false;
      }
      else{
        return this.DetermineTradeSender();
      }
    }
  }
};
</script>

<style scoped>
#trade-interface {
    border-style: solid;
    flex-basis: 80%;
    display: flex;
    flex-direction: column;
    margin-right: 5px;
    margin-top: 5px;
    margin-bottom: 5px;
    color: #3466af;
    border-width: 5px;
    border-radius: 10px;
    background-color: #ffcb05;
    max-height: 89vh;
}
#trade-windows-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 75vh;
    color: black;
    background-color:  #ffcb05
    
}
h2 {
  text-align: center;
  color: black;
}
input {
  max-width: 100px;
  text-align: center;
  align-self: center;
  background-color: white;
  color: #3466af;
  border-color: #3466af;
  border-radius: 5px;
  height: 25px;
  width: 100px;
  cursor: pointer;
  font-size: 11px;
  margin: 5px;
  margin-top: 2px;
  font-weight: bold;
}
#button-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
}
</style>