<script lang="ts">
  import svelteLogo from './assets/svelte.svg'
  import viteLogo from '/vite.svg'
  import Counter from './lib/Counter.svelte'
  import sendSentence from  './lib/Counter.svelte'
  import axios from "axios";
  let inputValue = ""
  let outputValue: any  = "Your translation here"

  interface TranslationResponse {
    translation: string;
  }

  const client = axios.create({
    baseURL: 'http://localhost:5173',
  });

</script>

<main>

<div class="top-padding">
  <div class="row">
    <div class="hidden-xs col-sm-1"></div>
    <div class="col-xs-12 col-sm-10">

      <div class="row">
        <div class="col-md-12">
          <h1 class="py-6 font-sans text-3xl font-bold tracking-wider text-blue-400">Translate english to pig latin!</h1>
          <Counter />
        </div>
      </div>


    </div>
  </div>
</div>
</main>

<style>
  .logo {
    height: 6em;
    padding: 1.5em;
    will-change: filter;
    transition: filter 300ms;
  }
  .logo:hover {
    filter: drop-shadow(0 0 2em #646cffaa);
  }
  .logo.svelte:hover {
    filter: drop-shadow(0 0 2em #ff3e00aa);
  }
  .read-the-docs {
    color: #888;
  }
</style>
