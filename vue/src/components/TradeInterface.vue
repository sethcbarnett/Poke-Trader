<template>
  <div id="trade-interface">
      <h2>Trade with (other username here)!</h2>
      <div id="trade-windows-container">
          <user-trade-window :username="this.loginUser" />
          <user-trade-window :username="this.tradingPartner" />
      </div>
      <input type="submit" value="Propose Trade!" />
  </div>
</template>

<script>
import UserTradeWindow from './UserTradeWindow.vue'
export default {
  components: { UserTradeWindow },
    name: 'trade-interface',
    data() {
        return {
            loginUser: this.$store.state.user.username,
            tradingPartner: 'Rachel'
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
}
#trade-windows-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 75vh;
}
h2 {
    text-align: center;
}
input {
    max-width: 100px;
    text-align: center;
    align-self: center;
}
</style>