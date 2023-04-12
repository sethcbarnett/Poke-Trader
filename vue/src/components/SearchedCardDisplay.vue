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
#card-display {
    display: flex;
    flex-direction: column;
    align-items: center;
}
h4 {
    word-wrap: break-word;
    display: block;
    max-width: 150px;
}
button {
    background-color: #ffcb05;
    color: #3466af;
    border-color: #3466af;
    border-radius: 5px;
    cursor: pointer;
}
#bottom-text {
    display: flex;
    flex-direction: column;
    align-items: center;
}

</style>