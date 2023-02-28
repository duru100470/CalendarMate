<script lang="ts">
    import { fetchPost } from '../functions'

    let email = '';
    let password = '';
    let validMessage = '';

    async function clickLoginBtn(): Promise<void> {
        validMessage = '';

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

        let ret = await fetchPost('/auth/login', {
            "UserName": 'username',
            "Email": email,
            "PasswordHash": password
        });

        console.log(ret.status);
        switch (ret.status)
        {
            case 404:
                validMessage = 'This account does not exist';
                break;

            case 401:
                validMessage = 'Email or Password is incorrect';
                break;
        }
    }
</script>

<main>
    <h1>Login</h1>
    <form>
        <p class="valid">{validMessage}</p>
        <p>Email</p>
        <p><input class="field" type="text" bind:value={email} required/></p>
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