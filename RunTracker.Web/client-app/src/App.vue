<template>
  <div id="app">
    <h1>Run-tracker</h1>
    <ActiveTrip />
    <!-- <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/> -->
  </div>
</template>

<script lang="ts">
import { Component, Vue, Mixins } from 'vue-property-decorator';
import HelloWorld from './components/HelloWorld.vue';
import ActiveTrip from './components/ActiveTrip.vue';
import TripService from './services/trip.service';
import Vuex from 'vuex';


@Component({
  components: {
    ActiveTrip
  },
  mixins: [
    TripService
  ]
})
// export default class App extends Vue {
export default class App extends Mixins(TripService) {
  private _tripService: TripService | null = null;
  public created() {
    console.log('Vue created');
    // this._tripService = 
  } 
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
