<script lang="ts">
    import type { UserInfo } from '../UserInfoStore';
    import { fetchPost } from '../functions'

    let username = '';
    let email = '';
    let password = '';
    let passwordConfirm = '';
    let validMessage = '';

    let isRegistrationFinished: boolean = false;

    async function clickRegisterBtn(): Promise<void> {
        if (!checkValidation()) return;

        let newUser: UserInfo = {
            userId: 1,
            userName: username,
            email: email,
            passwordHash: password,
            events: null
        };

        let res = await fetchPost('/auth/register', newUser);
        await processRegister(res.status);
    }

    async function processRegister(status: number): Promise<void> {
        switch (status) {
            case 201:
                isRegistrationFinished = true;
                break;
            case 409:
                validMessage = 'This username or email is already used';
                return;
            default:
                validMessage = 'Unexpected error occured';
                return;
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

        if (password === '') {
            validMessage = 'Password field is empty';
            return false;
        }

        if (password !== passwordConfirm) {
            validMessage = 'Password does not match';
            return false;
        }
        
        return true;
    }
</script>

<main>
    <h1>Register</h1>
    {#if !isRegistrationFinished}
    <form>
        <p class="valid">{validMessage}</p>
        <p>Username</p>
        <p><input class="field" type="text" bind:value={username}/></p>
        <p>Email</p>
        <p><input class="field" type="email" bind:value={email}/></p>
        <p>Password</p>
        <p><input class="field" type="password" bind:value={password}/></p>
        <p>Repeat your password</p>
        <p><input class="field" type="password" bind:value={passwordConfirm}/></p>
    </form>
    <button class="field" on:click={clickRegisterBtn}>Register</button>
    {:else}
    <p class="verify">Registration is finished!! Check your email to verify your account</p>
    {/if}
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

    .verify {
		color: #ff3e00;
    }
</style>