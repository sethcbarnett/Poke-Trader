<template>
  <div id="simplified-card-display">
      <div id="top-text">
          <h4>{{ availableCard.name }}</h4>
      </div>
      <div id="card-image">
          <img v-bind:src="availableCard.card.img" />
      </div>
      <div id="bottom-text">
          <h4>
            <span v-if="availableCard.card.price.length < 13">$</span
            >{{ Number(availableCard.card.price).toFixed(2) }}
            </h4>
      </div>
  </div>
</template>

<script>
export default {
    name: 'simplified-card-display',
    props: {
        availableCard: {
            card: {
                id: "",
                name: "",
                img: "",
                price: "",
                tcgUrl: "",
            },
            quantity: 0,
            quantityForTrade: 0,
            grade: ""
        }
    }
}
</script>

<style>

</style>