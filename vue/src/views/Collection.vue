<template>
  <div>
    <div id="card-display-area">
      <card-display/>
      <card-display/>
    </div>
  </div>
</template>

<script>
import CardDisplay from "../components/CardDisplay.vue"
export default {
    name: "collection",
    components: {
        CardDisplay
    }
}
</script>

<style scoped>
div {
  display: flex;
  font-family: sans-serif;
}
</style>