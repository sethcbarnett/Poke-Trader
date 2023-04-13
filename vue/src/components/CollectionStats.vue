<template>
    <div id="collection-statistics">
      <div id="total-cards">
        Total number of cards in collection: {{ $store.state.totalCardsInCurrentCollection }}
      </div>
      <div id="unique-cards">
        Unique cards in collection: {{ $store.state.uniqueCardsInCurrentCollection }}
      </div>
      <div id="trade-cards">
        Number of cards for trade: {{ $store.state.numberCardsForTradeInCurrentCollection }}
      </div>
      <div id="collection-value">
        Value of all cards in collection: ${{ $store.state.currentCollectionValue }}
      </div>
    </div>
</template>

<script>
export default {
    name: 'collection-stats'
}
</script>

<style>

</style>