<script lang="ts">
	import Calendar from "./Calendar.svelte";
	import { eventDBStore } from "./EventDBStore";
	import { fetchGet } from "../functions"
	import type { event } from "./EventDBStore";

	let curDate: Date = new Date();

	console.log(curDate);

	function increaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() + 1, curDate.getDate());
	}

	function decreaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() - 1, curDate.getDate());
	}

	async function fetchCalendarData(): Promise<void> {
		let res = await fetchGet("/calendar");
		let data: any[] = await res.json();

		let events: event[] = data.map(e => {
			return {
				...e,
				date: new Date(e.date)
			};
		});
		eventDBStore.update(db => events);
	}

	fetchCalendarData();
</script>

<main>
	<button class="calendar-btn-l" on:click={decreaseMonth}>&lt;</button>
	<button class="calendar-btn-r" on:click={increaseMonth}>&gt;</button>
	<Calendar targetDate={curDate}/>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		margin: 0 auto;
	}

	.calendar-btn-l, .calendar-btn-r {
		background-color: white;
		color: #ff3e00;
		font-size: 4em;
		font-weight: 100;
		margin: auto;
	}
</style>