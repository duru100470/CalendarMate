<script lang="ts">
    import { fetchPost } from '../functions'

    let userName = '';
    let password = '';
    let validMessage = '';

    function clickLoginBtn(): void {
        validMessage = '';

        if (userName === '') {
            validMessage = 'Username field is empty';
            return;
        }

        if (password === '') {
            validMessage = 'Password field is empty';
            return;
        }
    }
</script>

<main>
    <h1>Login</h1>
    <form>
        <p class="valid">{validMessage}</p>
        <p>Username</p>
        <p><input class="field" type="text" bind:value={userName} required/></p>
        <p>Password</p>
        <p><input class="field" type="password" bind:value={password} required/></p>
        <p>Forgot Password?</p>
        <a href="/#/auth/register">Create Account</a>
    </form>
    <button class="field" on:click={clickLoginBtn}>Login</button>
</main>

<style>
    main {
		text-align: center;
		align-items: center;
		padding: 1em;
		max-width: 300px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

    form {
        text-align: left;
        width: 100%;
    }

    .field {
        width: 100%;
    }

    .valid {
		color: #ff3e00;
    }
</style>