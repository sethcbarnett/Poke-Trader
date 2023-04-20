<template>
  <div id="side-bar">
      <h2>Trades In Progress</h2>
      <button class="trade" v-for="trade in $store.state.tradesInProgress" v-bind:key="trade.id" @click="GetTrade(trade)">Open trade with {{ trade }}</button>
      <form v-on:submit.prevent="SubmitUserSearch">
        <h2>Start a New Trade</h2>
        <input type="button" value="Search Users" v-on:click="NewTradeToggle" v-show="!$store.state.creatingNewTrade" />
        <div id="user-search-container" v-show="$store.state.creatingNewTrade">
            <input type="text" placeholder="Search for user..." v-model="searchString" />
            <input type="submit" value="Search" />
        </div>
      </form>
      <div class="searchedUser" v-for="user in $store.state.searchedUsers" v-bind:key="user.id">
          <button id="tradeable-users" @click="SetPendingTradeInfo(user)">{{ user }}</button>
      </div>
  </div>
</template>

<script>
import UserService from '../services/UserService.js';

export default {
    name: 'trade-side-bar',
    data() {
        return {
            creatingNewTrade: false,
            searchString: ''
        }
    },
    methods: {
        NewTradeToggle() {
            this.$store.commit('TOGGLE_CREATING_NEW_TRADE');
        },
        SubmitUserSearch() {
            UserService.getUsersThatContainSearch(this.searchString).then((response) => {
                this.$store.commit('SET_SEARCHED_USERS', response.data);
                this.searchString = '',
                this.NewTradeToggle();
            });
        },
        SetOtherUserInfo(username) {
            this.$store.commit('SET_OTHER_USER_INFO', username);
            this.$store.commit('SET_USER_INFO', this.$store.state.user.username);
            this.$store.commit('SET_IS_PENDING_TRADE', false);
        },
        SetPendingTradeInfo(username){
            this.$store.commit('RESET_PROPOSED_CARDS');
            this.SetOtherUserInfo(username);
        },
        SetTradesInProgress() {
            this.$store.commit('SET_TRADES_IN_PROGRESS');
        },
        //TODO: fix this so it uses mutations
        GetTrade(user) {
            UserService.getTrade(this.$store.state.user.username, user).then((response) => {
                console.log("hello");
                if(response.data.usernameFrom == this.$store.state.user.username) {
                    this.$store.state.otherUserUsername = response.data.usernameTo;
                    this.$store.state.loginUserProposedCards = response.data.collectionItemsFrom;
                    this.$store.state.otherUserProposedCards = response.data.collectionItemsTo;
                    this.SetOtherUserInfo(response.data.usernameTo);
                } else {
                    this.$store.state.otherUserUsername = response.data.usernameFrom;
                    this.$store.state.loginUserProposedCards = response.data.collectionItemsTo;
                    this.$store.state.otherUserProposedCards = response.data.collectionItemsFrom;
                    this.SetOtherUserInfo(response.data.usernameFrom);
                }
                this.$store.commit('SET_IS_PENDING_TRADE', true);
            });
        }
    },
    created() {
        this.SetTradesInProgress();
    }
}
</script>

<style>
#side-bar {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  border-radius: 10px;
  width: 330px;
  height: 89vh;
  max-height: 89vh;
  color: #3466af;
  background-color: #ffcb05;
  border-width: 5px;
  border-style: solid;
  border-color: #3466af;
  font-size: 1.25em;
  padding-bottom: 10px;
  line-height: 0.55em;
  text-align: center;
  line-height: 25px;
  margin: 5px;
}
#side-bar h2 {
    text-decoration: underline;
}
.searchedUser {
    margin-top: 5px;
    width: 90%;
}
#tradeable-users {
    width: 90%;
    font-size: 1.1em;
}
.trade {
    color: black;
    background-color: #ffcb05;
    width: 100%;
}
form {
    margin-bottom: 20px;
}
</style>