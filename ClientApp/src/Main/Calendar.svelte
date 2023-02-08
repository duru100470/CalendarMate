<script lang="ts">
	export let targetDate: Date;

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
						{firstDate.getDate() + j + i * 7 - firstDate.getDay()}
					</td>
					{:else}
					<td>
						{firstDate.getDate() + j + i * 7 - firstDate.getDay()}
					</td>
					{/if}
				{:else if i === 0 && j < firstDate.getDay()}
					<td class="notCurMonth">
						{prevDate.getDate() + j + i * 7 - prevDate.getDay()}
					</td>
				{:else if firstDateInSixthLine <= 0}
					<td class="notCurMonth">
						{nextDate.getDate() + j + (i - 5) * 7 - nextDate.getDay()}
					</td>
				{:else}
					<td class="notCurMonth">
						{nextDate.getDate() + j + (i - 4) * 7 - nextDate.getDay()}
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
		padding: 1em;
		max-width: 240px;
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
		width: 120px;
		height: 120px;
		text-align: left;
		vertical-align: top;
	}

	th, td {
		border: 1px solid #ddd;
		padding: 8px;
	}

	td:nth-child(1), td:nth-last-child(1) {color:red;}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>