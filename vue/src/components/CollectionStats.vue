<template>
    <div id="collection-statistics">
      <div id="collection-stats">
        Total cards: <p class="number-display">{{ $store.state.totalCardsInCurrentCollection }}</p>
      </div>
      <div id="collection-stats">
        Unique cards:<p class="number-display"> {{ $store.state.uniqueCardsInCurrentCollection }}</p>
      </div>
      <div id="collection-stats">
         Cards for trade:<p class="number-display"> {{ $store.state.numberCardsForTradeInCurrentCollection }}</p>
      </div>
      <div id="collection-stats">
        Value of collection:<p class="number-display"> ${{ $store.state.currentCollectionValue }}</p>
      </div>
      <div id="collection-stats">
        Commons:<p class="number-display"> {{$store.state.numberCommonCards}}</p>
      </div>
      <div id="collection-stats">
        Uncommons:<p class="number-display"> {{$store.state.numberUncommonCards}}</p>
      </div>
      <div id="collection-stats">
        Rares:<p class="number-display"> {{$store.state.numberRareCards}}</p>
      </div>
    </div>
</template>

<script>
export default {
    name: 'collection-stats'
}
</script>

<style>
#collection-stats{
  background-color: #ffcb05;
  color: black;
  
  
  border-radius: 5px;
  font-family: "Pokemon Solid", sans-serif;
  font-size: 10px;
  text-align: center;
  text-justify: auto;
  letter-spacing: 1px;
  padding-bottom: 5px;
  display: inline-block;
  margin-left: 10px;
  width: 120px;
  height: 80px;
  align-self: center;
}
#collection-statistics{
  display: flex;
  flex-direction: row;
}
.number-display{
  font-weight:bold;
  font-size: 20px;
  margin:0px;
}

</style>