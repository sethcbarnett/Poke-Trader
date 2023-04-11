<template>
  <div> 
    <div class ="toptext" >
      <h1>{{card.name}}</h1>
    </div>
    <div class="card-image">
      <img v-bind:src="card.img"/>

    </div>
    <div class ="bottom">
      <h3>{{card.id}}</h3>

    </div>
  </div>
</template>

<script>
 import CardService from "../services/CardService.js";
export default {
    name: 'card-display',
    data() {
        return {
            card:{ 

                id:"xy2-1",
                name: "",
                img: ""

            }
        }
    },
    created() {
      CardService.getCardById(this.$route.params.id).then((response)=>{
      this.card = response });
    }

}
</script>

<style>

</style>