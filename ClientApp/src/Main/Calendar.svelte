<script lang="ts">
	import Events from "./Events.svelte";

	export let targetDate: Date;
	export let showEventInfo;

	let currentDate: Date = new Date();

	$: firstDate = new Date(targetDate.getFullYear(), targetDate.getMonth(), 1);
	$: lastDate = new Date(targetDate.getFullYear(), targetDate.getMonth() + 1, 0);
	$: prevDate = new Date(targetDate.getFullYear(), targetDate.getMonth(), 0);
	$: nextDate = new Date(targetDate.getFullYear(), targetDate.getMonth() + 1, 1);
	$: firstDateInSixthLine = (35 - firstDate.getDay()) - lastDate.getDate();

	$: checkIsCurMonth = (i: number, j: number) => {
		return (i === 0 && j >= firstDate.getDay())
			|| ((firstDateInSixthLine >= 0) 
				&& ((i > 0 && i < 4) || (i === 4 && j <= lastDate.getDay())))
			|| ((firstDateInSixthLine < 0) 
				&& ((i > 0 && i < 5) || (i === 5 && j <= lastDate.getDay())));
	};

	$: checkIsCurDate = (i: number, j: number) => {
		return (firstDate.getDate() + j + i * 7 - firstDate.getDay() === currentDate.getDate())
				&& targetDate.getMonth() === currentDate.getMonth()
				&& targetDate.getFullYear() === currentDate.getFullYear();
	};

	function setTargetDate(date: Date, offset: number): Date {
		return new Date(firstDate.getFullYear(), 
			date.getMonth(), 
			date.getDate() + offset - date.getDay());
	}
</script>

<main>
	<h1>{targetDate.getFullYear() + '.' + (targetDate.getMonth() + 1)}</h1>
	<table>
	<thead>
		<tr>
		<th class="weekend">Sun</th>
		<th>Mon</th>
		<th>Tue</th>
		<th>Wed</th>
		<th>Thu</th>
		<th>Fri</th>
		<th class="weekend">Sat</th>
		</tr>
	</thead>
	<tbody>
		{#each Array.from({length:6}, (v,i)=>i) as i }
		<tr>
			{#each Array.from({length:7}, (v,i)=>i) as j }
				{#if checkIsCurMonth(i, j)}
					{#if checkIsCurDate(i, j)}
					<td class="curDate">
						<Events {showEventInfo} date={setTargetDate(firstDate, j + i * 7)}/>
					</td>
					{:else}
					<td>
						<Events {showEventInfo} date={setTargetDate(firstDate, j + i * 7)}/>
					</td>
					{/if}
				{:else if i === 0 && j < firstDate.getDay()}
					<td class="notCurMonth">
						<Events {showEventInfo} date={setTargetDate(prevDate, j + i * 7)}/>
					</td>
				{:else if firstDateInSixthLine <= 0}
					<td class="notCurMonth">
						<Events {showEventInfo} date={setTargetDate(nextDate, j + (i - 5) * 7)}/>
					</td>
				{:else}
					<td class="notCurMonth">
						<Events {showEventInfo} date={setTargetDate(nextDate, j + (i - 4) * 7)}/>
					</td>
				{/if}
			{/each}
		</tr>
		{/each}
	</tbody>
	</table>
</main>

<style>
	main {
		text-align: center;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	.notCurMonth {
		background-color: #f7f7f7;
	}

	.curDate{
		background-color: #ffecdf;
	}

	table {
		width: 100%;
		border-collapse: collapse;
	}

	thead {
		background-color: #fabca5;
		text-transform: uppercase;
	}

	.weekend {
		background-color: #ff3e00;
		text-transform: uppercase;
		color: white;
	}

	th {
		width: 120px;
		height: 20px;
		text-align: center;
		font-size: 1.1em;
	}

	td {
		margin: auto;
		width: 120px;
		height: 120px;
		text-align: left;
		vertical-align: top;
	}

	th, td {
		border: 1px solid #ddd;
		padding: 2px;
	}
</style>