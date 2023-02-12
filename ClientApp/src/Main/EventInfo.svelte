<script lang="ts">
    import { fetchDelete } from "../functions";
    import type { event } from "./EventDBStore";
    export let curEvent: event;

    async function deleteEvent(): Promise<void> {
        let res = await fetchDelete(`/calendar/${curEvent.eventId}`);

        console.log(res.status);
    }
</script>

<h2>{curEvent.title}</h2>
<p>Event ID: {curEvent.eventId}</p>
<p>Date: {curEvent.date}</p>
<p>Description: {curEvent.description}</p>
<p>User: {curEvent.user}</p>
<p>User ID: {curEvent.userId}</p>
<button on:click={deleteEvent}>Delete</button>

<style>
    h2 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 2em;
		font-weight: 100;
	}

    button {
		color: #ff3e00;
    }
</style>