<script lang="ts">
	import Calendar from "./Calendar.svelte";

	let curDate: Date = new Date(2019, 6, 1);

	console.log(curDate);

	function increaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() + 1, curDate.getDate());
	}

	function decreaseMonth(): void {
		curDate = new Date(curDate.getFullYear(), curDate.getMonth() - 1, curDate.getDate());
	}
</script>

<main>
	<button on:click={decreaseMonth}>&lt;</button>
	<button on:click={increaseMonth}>&gt;</button>
	<Calendar targetDate={curDate}/>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		margin: 0 auto;
	}
</style>