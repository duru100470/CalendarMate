<script lang="ts">
	import Calendar from "./Calendar.svelte";
	import Modal from "./Modal.svelte";
	import EventInfo from "./EventInfo.svelte";
	import { eventDBStore } from "./EventDBStore";
	import { fetchGet } from "../functions"
	import type { event } from "./EventDBStore";

	let showModal: boolean = false;
	let curDate: Date = new Date();
	let events: event[];
	let seletedEvent: event = null;

	eventDBStore.subscribe(db => { events = db });

	function increaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() + 1, curDate.getDate());
	}

	function decreaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() - 1, curDate.getDate());
	}

	function onClickEvent(id: number) {
		console.log(id);
		seletedEvent = events.find(e => e.eventId === id);
		showModal = true;
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
	{#if showModal}
	<Modal on:close={() => showModal = false}>
		<EventInfo curEvent={seletedEvent} />
	</Modal>
	{/if}
	<button class="calendar-btn-l" on:click={decreaseMonth}>&lt;</button>
	<button class="calendar-btn-r" on:click={increaseMonth}>&gt;</button>
	<Calendar targetDate={curDate} onClickEvent={onClickEvent}/>
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