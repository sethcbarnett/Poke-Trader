<template>
  <div id = "card-display" :href ="searchedCard.tcgUrl"> 
      <div id ="top-text" >
        <h4>{{searchedCard.name}}</h4>
      </div>
      <a :href="searchedCard.tcgUrl" target="_blank">
      <div id="card-image">
        <img v-bind:src="searchedCard.img"/>
      </div>
      </a>
      <div id ="bottom-text">
        <h4><span v-if="searchedCard.price.length < 13">$</span>{{Number(searchedCard.price).toFixed(2)}}</h4>
        <div id = "add-cards-div">
            <input id = "quantity-ticker" type = "number" v-model.number="quantity" min = "1"/>
            <button @click = "addCardToCollection">Add</button>
        </div>
      </div>
    </div>
</template>

<script>
import CollectionService from '../services/CollectionService.js';
export default {
    name: "searched-card-display",
    props: {
        searchedCard: {
            id: "",
            name: "",
            img: "",
            price: "",
            lowPrice: "",
            highPrice: "",
            rarity: "",
            tcgUrl: ""
        }
    },
    data() {
        return {
            collectionItem: {},
            quantity: 1,
            quantityForTrade: 0
        }
    },
    methods: {
        addCardToCollection(){
            if (!this.$store.state.isPremium && this.$store.state.currentCollectionObject.length >= 100){
                if (confirm('Standard users can only have up to 100 unique cards in their collection.\nWould you like to Catch \'Em All?') == true) {
                    this.$store.commit('TOGGLE_SEARCHING_OFF');
                    this.$router.push({name: 'premium'});
                } else {
                    return;
                }
            }
            else {
                this.collectionItem = {
                    card: {
                        id: this.searchedCard.id,
                        name: this.searchedCard.name,
                        img: this.searchedCard.img,
                        price: this.searchedCard.price,
                        lowPrice: this.searchedCard.lowPrice,
                        highPrice: this.searchedCard.highPrice,
                        rarity: this.searchedCard.rarity,
                        tcgUrl: this.searchedCard.tcgUrl,
                    },
                    quantity: this.quantity,
                    quantityForTrade: this.quantityForTrade
                };
                CollectionService.addCardToCollection(this.$store.state.user.username, this.collectionItem).then(() => {
                    this.$store.commit('SET_CURRENT_COLLECTION_OBJECT');
                    this.$store.commit('SET_FILTERED_COLLECTION_OBJ', {name: "", minPrice: 0, maxPrice: 200000, rarity: ["common", "uncommon", "rare"]});
                    alert(`${this.collectionItem.card.name} ${this.collectionItem.card.id} has been added to your collection.`);
                });
            }
            
        },
    },
    computed: {
            getDisplayedPriceString() {
                let initialPriceString = this.searchedCard.price;
                let price = Number(initialPriceString);
                return price.toFixed(2);
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
#add-cards-div {
    display: flex;
}
#quantity-ticker {
    width: 30px;
}
</style>