<script lang="ts">
	import Calendar from "./Calendar.svelte";
	import Modal from "./Modal.svelte";
	import EventInfo from "./EventInfo.svelte";
	import CreateEvent from "./CreateEvent.svelte";
	import { eventDBStore } from "./EventDBStore";
	import { fetchGet } from "../functions"
	import { MainModalType } from "../util";
	import type { event } from "./EventDBStore";

	// Set variables
	let curDate: Date = new Date();
	let events: event[];
	let seletedEvent: event = null;

	// Set flags
	let showModal: boolean = false;
	let modalType: number = MainModalType.EventInfo;

	eventDBStore.subscribe(db => { events = db });

	function increaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() + 1, curDate.getDate());
		fetchCalendarData();
	}

	function decreaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() - 1, curDate.getDate());
		fetchCalendarData();
	}

	function showEventInfo(id: number) {
		seletedEvent = events.find(e => e.eventId === id);
		modalType = MainModalType.EventInfo;
		showModal = true;
	}

	function showCreateEvent() {
		modalType = MainModalType.CreateEvent;
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
</script>

<main>
	{#if showModal}
	<Modal on:close={() => showModal = false}>
		{#if modalType === MainModalType.EventInfo}
		<EventInfo curEvent={seletedEvent} />
		{:else if modalType === MainModalType.CreateEvent}
		<CreateEvent />
		{/if}
	</Modal>
	{/if}
	<button class="calendar-btn-l" on:click={decreaseMonth}>&lt;</button>
	<button class="calendar-btn-r" on:click={increaseMonth}>&gt;</button>
	<button on:click={showCreateEvent}>Create Event</button>
	<Calendar targetDate={curDate} {showEventInfo}/>
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