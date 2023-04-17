<template>
  <div id="trade-view">
      <trade-side-bar />
      <trade-interface />
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
trade-side-bar {
    height: 1000px;
}
trade-interface {
    flex-basis: 80%;
    width: 100%;
}
</style>