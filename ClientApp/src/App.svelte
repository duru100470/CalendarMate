<script lang="ts">
	import { routes, authRoutes } from "./routes";
    import { userinfo } from "./UserInfoStore";
	import NavBar from "./NavBar.svelte";
	import Router, { location } from "svelte-spa-router";

	// Redirect to login page
	$: if (authRoutes[$location]) {
		if (userinfo.get() == null)
			document.location.href = '/#/auth/login';
	}
</script>

<main>
	<NavBar />
	<Router {routes} />
</main>