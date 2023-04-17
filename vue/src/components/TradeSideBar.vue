<template>
  <div id="side-bar">
      <h2>Trades In Progress</h2>
      <form v-on:submit.prevent="NewTradeToggle">
        <input type="button" value="+ New Trade" v-on:click="NewTradeToggle" v-show="!$store.state.creatingNewTrade" />
        <div id="user-search-container" v-show="$store.state.creatingNewTrade">
            <input type="text" placeholder="Search for user..." />
            <input type="submit" value="Search" />
        </div>
      </form>
  </div>
</template>

<script>
export default {
    name: 'trade-side-bar',
    data() {
        return {
            creatingNewTrade: false
        }
    },
    methods: {
        NewTradeToggle() {
            this.$store.commit('TOGGLE_CREATING_NEW_TRADE');
        },
        SubmitUserSearch() {

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