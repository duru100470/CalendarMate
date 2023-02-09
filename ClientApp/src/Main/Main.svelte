<script lang="ts">
	import Calendar from "./Calendar.svelte";

	let curDate: Date = new Date();

	console.log(curDate);

	function increaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() + 1, curDate.getDate());
	}

	function decreaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() - 1, curDate.getDate());
	}
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