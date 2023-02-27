<script lang="ts">
	import Home from "./Home/Home.svelte";
	import Test from "./Test/Test.svelte";
	import Main from "./Main/Main.svelte";
	import Login from "./Auth/Login.svelte";
	import Register from "./Auth/Register.svelte";
	import NotFound from "./Error/404.svelte";
	import NavBar from "./NavBar.svelte";
	import Router from "svelte-spa-router";

	const routes = {
		'/': Home,
		'/test': Test,
		'/main': Main,
		'/auth/login': Login,
		'/auth/register': Register,
		'/*': NotFound
	};
</script>

<main>
	<NavBar />
	<Router {routes} />
</main>