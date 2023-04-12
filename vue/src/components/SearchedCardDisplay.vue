<template>
  <div id = "card-display" :href ="searchedCard.tcgUrl"> 
      <div id ="top-text" >
        <h4>{{searchedCard.name}}</h4>
      </div>
      <div id="card-image">
        <img v-bind:src="searchedCard.img"/>
      </div>
      <div id ="bottom-text">
        <h4>{{searchedCard.price}}</h4>
        <button>Add</button>
      </div>
    </div>
</template>

<script>
export default {
    name: "searched-card-display",
    props: {
        searchedCard: {
            id: "",
            name: "",
            img: "",
            price: "",
            tcgUrl: ""
        }
    }
}
</script>

<style>

</style>