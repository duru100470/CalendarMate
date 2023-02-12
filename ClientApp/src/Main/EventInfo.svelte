<script lang="ts">
    import type { event } from "./EventDBStore";
    export let curEvent: event;
</script>

<h2>{curEvent.title}</h2>
<p>Event ID: {curEvent.eventId}</p>
<p>Date: {curEvent.date}</p>
<p>Description: {curEvent.description}</p>
<p>User: {curEvent.user}</p>
<p>User ID: {curEvent.userId}</p>

<style>
    h2 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 2em;
		font-weight: 100;
	}
</style>