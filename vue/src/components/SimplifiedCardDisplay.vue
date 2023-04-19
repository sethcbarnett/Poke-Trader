<template>
  <div id="simplified-card-display">
      <div id="top-text">
          <h4>{{ availableCard.card.name }}</h4>
      </div>
      <div id="card-image">
          <img v-bind:src="availableCard.card.img" 
          @click="MakeCardProposed(availableCard)" />
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
        },
        username: String
    },
    methods: {
        
        MakeCardProposed(availableCard) {
          let payload = { 'card':availableCard, 'user':this.username}
            this.$store.commit('MAKE_CARD_PROPOSED', payload);
        }
    }
}
</script>

<style scoped>
#simplified-card-display {
  background-color: rgb(207, 200, 177);
  border: 2px solid black;
  border-radius: 20px;
  padding: 20px;
  display: flex;
  flex-direction: column;
  margin: 5px;
  padding-bottom: 5px;
  padding-top: 5px;
}
#top-text h4 {
  margin: 5px;
  text-align: center;
}
#card-image {
  display: block;
  align-self: center;
}
#bottom-text h4 {
  margin: 5px;
  text-align: center;
}
img {
  width: 90px;
}
h4 {
  font-size: 0.7em;
  font-weight: 700;
}

</style>