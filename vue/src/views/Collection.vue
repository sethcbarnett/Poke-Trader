<template>
  <div>
    <card-display class = "card1"/>
    <card-display/>
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

<style>
div{
  display: flex;
}
.card1 {
  padding-right: 10px;
}
</style>