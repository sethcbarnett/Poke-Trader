<template>
  <div id="trade-interface">
      <h2>Trade with {{ $store.state.otherUserUsername }}!</h2>
      <div id="trade-windows-container">
          <user-trade-window :username="this.$store.state.user.username" />
          <user-trade-window :username="this.$store.state.otherUserUsername" />
      </div>
      <input type="submit" value="Propose Trade!" @click="PostTrade(tradeObject)"/>
  </div>
</template>

<script>
import UserService from '../services/UserService'
import UserTradeWindow from './UserTradeWindow.vue'
export default {
  components: { UserTradeWindow },
    name: 'trade-interface',
    data() {
        return {
            tradeObject: {
                usernameFrom: this.$store.state.user.username,
                usernameTo: this.$store.state.otherUserUsername,
                collectionItemsFrom: this.$store.state.loginUserProposedCards,
                collectionItemsTo: this.$store.state.otherUserProposedCards
            }
        }
    },
    methods: {
        PostTrade(tradeObject) {
            UserService.postTrade(tradeObject).then((response) => {
                if(response.status == 200) {
                    alert(`Trade Requested with ${this.$store.state.otherUserUsername}`);
                    this.$store.commit('SET_OTHER_USER_INFO', '');
                } else {
                    alert("something broked");
                }
            });
        }
    }
}
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
    background-color: #ffcb05
    
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
}
</style>