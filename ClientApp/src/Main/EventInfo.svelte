<script lang="ts">
    import { fetchDelete, fetchPut } from "../functions";
    import type { event } from "./EventDBStore";
    export let curEvent: event;

    let newEvent = {
        title: '',
        date: '',
        description: '',
        userId: 1
    };
    // Set flags
    let showEdit: boolean = false;

    async function deleteEvent(): Promise<void> {
        let res = await fetchDelete(`/calendar/${curEvent.eventId}`);

        console.log(res.status);
    }

    function onClickEditBtn() {
        newEvent.title = curEvent.title;
        newEvent.date = curEvent.date.toJSON().substring(0, 10);
        newEvent.description = curEvent.description;
        showEdit = true;
    }

    async function onClickSaveBtn(): Promise<void> {
        newEvent.date = newEvent.date + "T00:00:00";
        let res = await fetchPut(`/calendar/${curEvent.eventId}`, newEvent);

        console.log(res.status);
        showEdit = false;
    }

</script>

{#if !showEdit}
<h2>{curEvent.title}</h2>
<p>Event ID: {curEvent.eventId}</p>
<p>Date: {curEvent.date}</p>
<p>Description: {curEvent.description}</p>
<p>User: {curEvent.user}</p>
<p>User ID: {curEvent.userId}</p>
<button on:click={onClickEditBtn}>Edit</button>
<button on:click={deleteEvent} style="color: #ff3e00;">Delete</button>
{:else}
<form>
    <p>Title: <input type="text" bind:value={newEvent.title} /></p>
    <p>Date: <input type="date" bind:value={newEvent.date} /></p>
    <p>Description: <input type="text" bind:value={newEvent.description} /></p>
    <button on:click={onClickSaveBtn}>Save</button>
</form>
{/if}

<style>
    h2 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 2em;
		font-weight: 100;
	}
</style>