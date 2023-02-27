<script lang="ts">
    import type { UserInfo } from "./UserInfoStore";
    import { userinfo } from "./UserInfoStore";
    import { fetchPost } from './functions';

    let isLoggedIn = false;
    let user: UserInfo = {username: 'test', email: 'test@gmail.com'};
    
    userinfo.subscribe(data => {
        user = data;
        if (user === null) isLoggedIn = false;
        else isLoggedIn = true;
    })

    async function logout(): Promise<void> {
        await fetchPost('/auth/logout', null);
        userinfo.set(null);
    }
</script>

<main>
<div class="logo">CalendarMate</div>
<div class="menu"><a href="/#">Home</a></div>
<div class="menu"><a href="/#/main">Main</a></div>
{#if isLoggedIn}
    <div class="account"><a href="/#/auth/login" on:click={logout}>Logout</a></div>
    <div class="account"><a href="/#/auth/account">Hello, {user.username}</a></div>
{:else}
    <div class="account"><a href="/#/auth/login" on:click={logout}>Logout</a></div>
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