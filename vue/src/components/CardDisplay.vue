<template>
  <div id="card-display">
    <div id="top-text">
      <h4>{{ collectionItem.card.name }}</h4>
    </div>
    <a :href="collectionItem.card.tcgUrl" target="_blank">
      <div id="card-image">
        <img v-bind:src="collectionItem.card.img" />
      </div>
    </a>
    <div id="bottom-text">
      <h4>
        <span v-if="collectionItem.card.price.length < 13">$</span
        >{{ Number(collectionItem.card.price).toFixed(2) }}
      </h4>
      <div class="quantity-wrapper">
        <h4>Quantity Owned:</h4>
        <h4 v-show="!this.editingQuantity">{{ collectionItem.quantity }}</h4>
        <img
          v-show="!this.editingQuantity"
          class="pencil-img"
          @click="editQuantity"
          src="../assets/edit-pencil-icon.png"
        />
        <input
          v-model="changedQuantity"
          class="small-input-field"
          v-show="this.editingQuantity"
          type="text"
          @keyup.enter.prevent="submitQuantity"
        />
      </div>
      <div class="quantity-wrapper">
        <h4>Quantity For Trade:</h4>
        <h4 v-show="!this.editingForTrade">
          {{ collectionItem.quantityForTrade }}
        </h4>
        <img
          v-show="!this.editingForTrade"
          class="pencil-img"
          @click="editQuantityForTrade"
          src="../assets/edit-pencil-icon.png"
        />
        <input
          v-model="changedQuantityForTrade"
          class="small-input-field"
          v-show="this.editingForTrade"
          type="text"
          @keyup.enter.prevent="submitQuantityForTrade"
        />
      </div>
      <div class="quantity-wrapper">
        <h4>Grade: </h4>
        <h4 v-show="!this.editingGrade">
          {{ collectionItem.grade }}
        </h4>
        <img
          v-show="!this.editingGrade"
          class="pencil-img"
          @click="editGrade"
          src="../assets/edit-pencil-icon.png"
        />
        <input
          v-model="changedGrade"
          class="small-input-field"
          v-show="this.editingGrade"
          type="text"
          @keyup.enter.prevent="submitGrade"
        />
      </div>
    </div>
  </div>
</template>

<script>
//import CardService from "../services/CardService.js";
export default {
  data() {
    return {
      editingQuantity: false,
      editingForTrade: false,
      editingGrade: false,
      changedQuantity: 0,
      changedQuantityForTrade: 0,
      changedGrade: ""
    };
  },
  name: "card-display",
  props: {
    collectionItem: {
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
  },
  methods: {
    editQuantity() {
      this.editingQuantity = true;
    },
    editQuantityForTrade() {
      this.editingForTrade = true;
    },
    editGrade() {
      this.editingGrade = true;
    },
    submitQuantity() {
      this.editingQuantity = false;
      this.$emit("set-quantity", Number(this.changedQuantity));
    },
    submitQuantityForTrade() {
      this.editingForTrade = false;
      this.$emit("set-quantity-for-trade", Number(this.changedQuantityForTrade));
    },
    submitGrade() {
      this.editingGrade = false;
      this.$emit("set-grade", this.changedGrade);
    }
  },
  created() {
    this.changedQuantity = this.collectionItem.quantity;
    this.changedQuantityForTrade = this.collectionItem.quantityForTrade;
    this.changedGrade = this.collectionItem.grade;
  }
};
</script>

<style>
#card-display {
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
#display-area {
  background-color: blue;
  margin-top: 1px;
}
#top-text h4 {
  margin: 5px;
  text-align: center;
}
#card-image {
  display: block;
}
#bottom-text h4 {
  margin: 5px;
  text-align: center;
  width: 100%;
}
a {
  text-decoration: none;
  color: black;
}
img {
  width: 160px;
}
h4 {
  font-size: 0.7em;
  font-weight: 700;
}
.pencil-img {
  height: 20px;
  width: auto;
  background-color: rgb(207, 200, 177);
  cursor: pointer;
}
.quantity-wrapper {
  display: flex;
  flex-direction: row;
  justify-content: center;
}
.quantity-wrapper h4 {
  font-size: 0.65em;
}
.small-input-field {
  width: 25px;
}
</style>