<script lang="ts">
    import { fetchPost } from '../functions'

    let userName = '';
    let email = '';
    let password = '';
    let passwordConfirm = '';
    let validMessage = '';

    function clickRegisterBtn(): void {
        validMessage = '';

        if (userName === '') {
            validMessage = 'Username field is empty';
            return;
        }

        if (email === '') {
            validMessage = 'Email field is empty';
            return;
        }

        let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");

        if (!regex.test(email)) {
            validMessage = 'Email field is not valid';
            return;
        }

        if (password === '') {
            validMessage = 'Password field is empty';
            return;
        }

        if (password !== passwordConfirm) {
            validMessage = 'Password does not match';
            return;
        }
    }
</script>

<main>
    <h1>Register</h1>
    <form>
        <p class="valid">{validMessage}</p>
        <p>Username</p>
        <p><input class="field" type="text" bind:value={userName}/></p>
        <p>Email</p>
        <p><input class="field" type="email" bind:value={email}/></p>
        <p>Password</p>
        <p><input class="field" type="password" bind:value={password}/></p>
        <p>Repeat your password</p>
        <p><input class="field" type="password" bind:value={passwordConfirm}/></p>
    </form>
    <button class="field" on:click={clickRegisterBtn}>Register</button>
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