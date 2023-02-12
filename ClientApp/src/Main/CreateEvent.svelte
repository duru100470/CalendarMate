<script lang="ts">
    import { fetchPost } from "../functions";
    import { eventDBStore } from "./EventDBStore";
    import type { event } from "./EventDBStore";

    let title: string;
    let date: Date;
    let description: string;

    async function createEvent(): Promise<void> {
        if (!checkFormValid()) return;

        let curEvent = {
            title: title,
            date: date + "T00:00:00",
            description: description,
            userId: 1
        };

        let res = await fetchPost("/calendar", curEvent);
        
        if (res.status !== 200) return;
    }

    function checkFormValid(): boolean {
        if (title === '') return false;
        if (date === undefined) return false;
        return true;
    }
</script>

<h1>Create Event</h1>
<p>Title: <input type="text" bind:value={title} /></p>
<p>Date: <input type="date" bind:value={date} /></p>
<p>Description: <input type="text" bind:value={description} /></p>
<button on:click={createEvent}>Create Event</button>

<style>
    h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}
</style>