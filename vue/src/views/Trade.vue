<template>
  <div id="trade-view">
      <trade-side-bar />
      <trade-interface v-if="$store.state.otherUserUsername != ''" />
  </div>
</template>

<script>
import TradeInterface from '../components/TradeInterface.vue'
import TradeSideBar from '../components/TradeSideBar.vue'
export default {
  components: { TradeSideBar, TradeInterface },
    name:'trade'
}
</script>

<style scoped>
#trade-view {
    display: flex;
    width: 100vw;
}
</style>