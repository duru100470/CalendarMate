<script lang="ts">
    import type { event } from "./EventDBStore";
    export let curEvent: event;
</script>

{curEvent.eventId}
<br />
{curEvent.title}
<br />
{curEvent.date}
<br />
{curEvent.description}
<br />
{curEvent.user}
<br />
{curEvent.userId}