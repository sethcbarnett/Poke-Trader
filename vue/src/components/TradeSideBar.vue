<template>
  <div id="side-bar">
      <h2>Trades In Progress</h2>
      <p class="trade" v-for="trade in $store.state.tradesInProgress" v-bind:key="trade.id"></p>
      <form v-on:submit.prevent="SubmitUserSearch">
        <input type="button" value="+ New Trade" v-on:click="NewTradeToggle" v-show="!$store.state.creatingNewTrade" />
        <div id="user-search-container" v-show="$store.state.creatingNewTrade">
            <input type="text" placeholder="Search for user..." v-model="searchString" />
            <input type="submit" value="Search" />
        </div>
      </form>
      <div class="searchedUser" v-for="user in $store.state.searchedUsers" v-bind:key="user.id">
          <button @click="SetOtherUserInfo(user)">{{ user }}</button>
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
        }
    }
}
</script>

<style>
#side-bar {
    border-style: solid;
    flex-basis: 20%;
    display: flex;
    flex-direction: column;
    align-items: center;
    height: 91vh;
    margin-right: 10px;
    margin-top: 5px;
    margin-left: 5px;
}
</style>