<script lang="ts">
	import Home from "./Home/Home.svelte";
	import Test from "./Test/Test.svelte";
	import Main from "./Main/Main.svelte";
	import NotFound from "./Error/404.svelte";
	import Router from "svelte-spa-router";
	const routes = {
		'/': Home,
		'/test': Test,
		'/main': Main,
		'/*': NotFound
	};
</script>

<main>
	<Router {routes} />
</main>