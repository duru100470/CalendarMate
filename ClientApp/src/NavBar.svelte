<script lang="ts">
    let isLoggedIn = false;
    let username = 'Test';
</script>

<main>
<div class="logo">CalendarMate</div>
<div class="menu"><a href="/#">Home</a></div>
<div class="menu"><a href="/#/main">Main</a></div>
{#if isLoggedIn}
    <div class="account"><a href="/#/auth/account">Hello, {username}</a></div>
{:else}
    <div class="account"><a href="/#/auth/login">Login</a></div>
{/if}
</main>

<style>
    main {
        width: 100%;
        height: 50px;
		background-color: #f39775;
    }

    .logo {
        margin-top: 5px;
        margin-left: 1em;
        margin-right: 2em;
        float: left;
        font-size: 2em;
        color: white;
    }

    .menu {
        margin-top: 8px;
        margin-left: 1em;
        margin-right: 1em;
        float: left;
        font-size: 24px;
        color: white;
    }

    .account {
        margin-top: 8px;
        margin-left: 1em;
        margin-right: 1em;
        float: right;
        font-size: 24px;
        color: white;
    }

    a {
        color: inherit;
        text-decoration: none;
    }
</style>