<template>
  <div id="trade-interface">
      <h2>Trade with {{ $store.state.otherUserUsername }}!</h2>
      <div id="trade-windows-container">
          <user-trade-window :username="this.$store.state.user.username" />
          <user-trade-window :username="this.$store.state.otherUserUsername" />
      </div>
      <input type="submit" value="Propose Trade!" @click="PostTrade(MakeTradeObject)"/>
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
    },
    computed: {
        MakeTradeObject() {
            let tradeObject = {};
            tradeObject.usernameFrom = this.$store.state.user.username;
            tradeObject.usernameTo = this.$store.state.otherUserUsername;
            tradeObject.collectionItemsFrom = this.$store.state.loginUserProposedCards;
            tradeObject.collectionItemsTo = this.$store.state.otherUserProposedCards;
            return tradeObject;
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
    margin:5px;
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
    
    padding: 0px;
    
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