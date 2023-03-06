<script lang="ts">
    import { fetchPost } from '../functions'

    let email = '';
    let username = '';
    let validMessage = '';

    async function clickRecoverBtn(): Promise<void> {

        if (!checkValidation()) return;

        let ret = await fetchPost('/auth/forgot', {
            "UserName": username,
            "Email": email,
            "PasswordHash": "dummy"
        });

        await processLogin(ret.status);
    }

    async function processLogin(status: number): Promise<void> {
        switch (status)
        {
            case 200:
                validMessage = 'Email was sent. Click link in an email to recover yout password';
                break;
            case 404:
                validMessage = 'This account does not exist';
                break;
            default:
                validMessage = 'Unexpected error occured';
                break;
        }
    }

    function checkValidation(): boolean {
        validMessage = '';

        if (username === '') {
            validMessage = 'Username field is empty';
            return false;
        }

        if (email === '') {
            validMessage = 'Email field is empty';
            return false;
        }

        let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");

        if (!regex.test(email)) {
            validMessage = 'Email field is not valid';
            return false;
        }

        return true;
    }
</script>

<main>
    <h1>Recover</h1>
    <form>
        <p class="valid">{validMessage}</p>
        <p>Username</p>
        <p><input class="field" type="text" bind:value={username} required/></p>
        <p>Email</p>
        <p><input class="field" type="text" bind:value={email} required/></p>
    </form>
    <button class="field" on:click={clickRecoverBtn}>Reset Password</button>
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