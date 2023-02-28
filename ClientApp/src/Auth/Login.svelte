<script lang="ts">
    import type { UserInfo } from '../UserInfoStore';
    import { userinfo } from '../UserInfoStore';
    import { fetchGet, fetchPost } from '../functions'

    let email = '';
    let password = '';
    let validMessage = '';

    async function clickLoginBtn(): Promise<void> {

        if (!checkValidation()) return;

        let ret = await fetchPost('/auth/login', {
            "UserName": 'username',
            "Email": email,
            "PasswordHash": password
        });

        await processLogin(ret.status);
    }

    async function processLogin(status: number): Promise<void> {
        switch (status)
        {
            case 200:
                let res = await fetchGet('/auth/account');
                if (res.status != 200) {
                    validMessage = "Something went wrong...";
                    return;
                }
                
                let data: UserInfo = await res.json();
                userinfo.set(data);
                document.location.href = '/';
                break;
            case 401:
                validMessage = 'Email or Password is incorrect';
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

        return true;
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