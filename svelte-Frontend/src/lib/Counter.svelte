<script lang="ts">
  import axios from 'axios';
  import { onMount } from "svelte";
  import AxiosRequestConfig from 'axios';
  interface TranslationResponse {
      message: string;
      storedSentence: string;
  }

  interface TranslationResponsePacket {
      dataOne: TranslationResponse | null;
      dataTwo: TranslationResponse | null;
      dataThree: TranslationResponse | null;
  }




  const client = axios.create({
    baseURL: 'http://localhost:5000',
  });


  let inputValue = ""
  let outputValue: any  = "Your translation here";
  let packetResponse: TranslationResponsePacket | null = null;

  let translations =[]
  onMount(async () => {
      try {
          packetResponse = await pullData();
      } catch (error) {

          console.error("Error fetching sets:", error);
      }
  });

  export const sendSentence = async (sentence: string) => {
      try {
          const response = await client.post<TranslationResponse>("/runEntry", { sentence });
          console.log("Response from backend:", response.data.storedSentence);
          packetResponse = await pullData();
          outputValue = response.data.storedSentence;
          return response.data.storedSentence;

      } catch (error) {
          outputValue = "error sending";
          console.error("Error sending sentence:", error);
      }
  };


  export const pullData = async () => {
      try {
          const response = await client.get<TranslationResponsePacket>("/pullEntries");
          console.log("Response from backend:", response.data.dataOne, response.data.dataTwo, response.data.dataThree);

          packetResponse = response.data;
          return response.data;

      } catch (error) {
          console.error("Error sending sentence:", error);
      }
  };

  async function handleSubmit() {

      try {
          const response = await sendSentence(inputValue);
          console.log(response)
          outputValue = response
      } catch (error) {
          outputValue = "error sending";
          console.error("Error:", error);
      }
  }

  $: translations = packetResponse
      ? [packetResponse.dataOne, packetResponse.dataTwo, packetResponse.dataThree]
          .filter(item => item !== null) // Remove null entries
      : [];

</script>

<main>

    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <div class="form-group">

            <div class="form-group flex flex-wrap items-center px-16 gap-4">
                <div class="pt-14 p-10 flex-grow">
                    <textarea  class="w-full h-16 border-2 border-blue-300 rounded-xl p-2" bind:value={inputValue} placeholder="english here...">

                    </textarea>

                </div>


                <button on:click={handleSubmit} class="border-4 border-blue-500 p-2 rounded-2xl hover:border-blue-700 font-bold  text-red-300">
                    <img src="../../public/piggy.png" alt="submit" class="w-24">
                    anslateTray!
                </button>
            </div>
            <div class="py-2 ">
                <h3 class=" py-10 font-sans text-2xl font-bold tracking-wider text-white bg-rose-200 rounded-lg">
                    {outputValue}
                </h3>
            </div>

        </div>
    </div>

    <div class="form-group">
        <div>
            <h2 class="pt-10 font-sans text-xl font-bold tracking-wider text-blue-400"> Recent translations: </h2>
            <hr style="border: 4px solid lightpink; margin: 20px 0;">
        </div>

        <div class="pt-4">
            {#each translations as translation}
                <p> "{translation.bt}" →  "{translation.at}"</p>
            {/each}

        </div>
    </div>




</main>


