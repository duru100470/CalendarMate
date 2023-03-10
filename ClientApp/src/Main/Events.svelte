<script lang="ts">
	import { eventDBStore, event } from "./EventDBStore";
	export let date: Date;
	export let showEventInfo;

	let eventList: event[] = [];

	function checkIsSameDate(date1: Date, date2: Date): boolean {
		return date1.getFullYear() === date2.getFullYear()
			&& date1.getMonth() === date2.getMonth()
			&& date1.getDate() === date2.getDate();
	}

	eventDBStore.subscribe((data) => {
		eventList = data.filter(e => checkIsSameDate(e.date, date));
	});
</script>

<main>
	<div class="date">
	{#if date.getDay() === 0 || date.getDay() === 6}
		<div class="weekend">{date.getDate()}</div>
	{:else}
		<div>{date.getDate()}</div>
	{/if}
	</div>
	<div class="event-container">
		{#each eventList as e}
		<button class="event-btn" on:click={() => showEventInfo(e.eventId)}>{e.title}</button>
		{/each}
	</div>
</main>

<style>
	main {
		margin: auto;
		width: 100%;
		height: 100%;
	}

	.date {
		margin-left: 4px;
	}

	.weekend {
		color: #ff3e00;
	}

	.event-btn {
		margin: auto;
		margin-bottom: 2px;
		width: 100%;
		min-height: 20px;
		display: flex;
		align-items: center; /* 가로 - 중앙으로 */
		justify-content: flex-start;
		padding-left: 1;
		padding-top: 0;
		padding-bottom: 0;
		border: none;
		transition: all 0.2s ;
		background-color: #ffd49f;
	}

	.event-btn:hover {
		background-color: #fab765;
	}
</style>