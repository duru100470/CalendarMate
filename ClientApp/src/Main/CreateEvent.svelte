<script lang="ts">
    import { fetchPost } from "../functions";
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

        console.log(JSON.stringify(curEvent));
        fetchPost("/calendar", curEvent);
    }

    function checkFormValid(): boolean {
        if (title === '') return false;
        if (date === undefined) return false;
        return true;
    }
</script>

<p>Title: <input type="text" bind:value={title} /></p>
<p>Date: <input type="date" bind:value={date} /></p>
<p>Description: <input type="text" bind:value={description} /></p>
<button on:click={createEvent}>Create Event</button>