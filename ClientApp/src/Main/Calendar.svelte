<script lang="ts">
	let currentDate: Date = new Date();
	let { firstDate, lastDate } = getFirstAndLastDate(currentDate);
	let { prevDate, nextDate } = getPrevAndNextDate(currentDate);

	function getFirstAndLastDate(date: Date) {
		let firstDate = new Date(date.getFullYear(), date.getMonth(), 1);
		let lastDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
		return { firstDate, lastDate };
	}

	function getPrevAndNextDate(date: Date) {
		let prevDate = new Date(date.getFullYear(), date.getMonth(), 0);
		let nextDate = new Date(date.getFullYear(), date.getMonth() + 1, 1);
		return { prevDate, nextDate };
	}
</script>

<main>
	<h1>{currentDate.getFullYear() + '.' + (currentDate.getMonth() + 1)}</h1>
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
		{#each Array.from({length:5}, (v,i)=>i) as i }
		<tr>
			{#each Array.from({length:7}, (v,i)=>i) as j }
				{#if (i === 0 && j >= firstDate.getDay())
					|| (i === 4 && j <= lastDate.getDay())
					|| (i > 0 && i < 4)}
					{#if (firstDate.getDate() + j + i * 7 - firstDate.getDay() 
						=== currentDate.getDate())}
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
				{:else}
					<td class="notCurMonth">
						{nextDate.getDate() + j - nextDate.getDay()}
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