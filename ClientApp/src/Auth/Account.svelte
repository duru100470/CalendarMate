<script lang="ts">
	import { fetchPut } from '../functions';
	import type { UserInfo } from '../UserInfoStore';
	import { userinfo } from '../UserInfoStore';

    let curUser: UserInfo = null;
	let userName: string = '';
	let prevPassword: string = '';
	let password: string = '';
	let passwordConfirm: string = '';
	let validMessage: string = '';
	let isEditMode: boolean = false;

	function getAccountInfo(): void{
		curUser = userinfo.get();
		console.log(userinfo.get());

		userName = curUser.userName;
	}

	userinfo.subscribe(data => {
		curUser = data;
	});

    async function clickProfileEditBtn(): Promise<void> {
        if (!checkProfileValidation()) return;

		let data = {
			...curUser,
			userName: userName,
			passwordHash: 'dummy'
		}

        let res = await fetchPut('/auth/account', data);
        await processEditProfile(res);
    }

    async function processEditProfile(res: Response): Promise<void> {
        switch (res.status) {
            case 201:
				isEditMode = false;
				userinfo.set(await res.json());
                break;
            case 401:
				userinfo.set(null);
                document.location.href = '/#/auth/login';
                return;
            default:
                validMessage = 'Unexpected error occured';
                return;
        }
    }

    async function clickPasswordEditBtn(): Promise<void> {
        if (!checkPasswordValidation()) return;

		let data = {
			...curUser,
			userName: password,
			passwordHash: prevPassword
		}

        let res = await fetchPut('/auth/pass', data);
        await processEditPassword(res.status);
    }

    async function processEditPassword(status: number): Promise<void> {
        switch (status) {
            case 201:
				isEditMode = false;
                break;
            case 401:
				userinfo.set(null);
                document.location.href = '/#/auth/login';
                return;
            default:
                validMessage = 'Unexpected error occured';
                return;
        }
    }

	function checkProfileValidation(): boolean {
        validMessage = '';

        if (curUser.userName === '') {
            validMessage = 'Username field is empty';
            return false;
        }
        
        return true;
    }

	function checkPasswordValidation(): boolean {
        validMessage = '';

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

	getAccountInfo();
</script>

<main>
    <h1>Account</h1>
	{#if !isEditMode}
	<h3>Public profile</h3>
	<form>
		<p>Username</p>
        <p><input class="field" type="text" value={curUser.userName} readonly/></p>
		<p>Email</p>
        <p><input class="field" type="text" value={curUser.email} readonly/></p>
		<button class="field" on:click={() => {isEditMode = true}}>Edit profile</button>
	</form>
	{:else}
	<p class="valid">{validMessage}</p>
	<h3>Public profile</h3>
	<form>
		<p>Username</p>
        <p><input class="field" type="text" bind:value={userName}/></p>
		<p>Email</p>
        <p><input class="field" type="text" bind:value={curUser.email} readonly/></p>
		<button class="field" on:click={clickProfileEditBtn}>Save profile</button>
	</form>
	<h3>Password</h3>
	<form>
		<p>Current Password</p>
        <p><input class="field" type="text" bind:value={prevPassword}/></p>
		<p>New Password</p>
        <p><input class="field" type="text" bind:value={password}/></p>
		<p>Confirm Password</p>
        <p><input class="field" type="text" bind:value={passwordConfirm}/></p>
		<button class="field" on:click={clickPasswordEditBtn}>Save password</button>
	</form>
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

    .valid {
		color: #ff3e00;
		text-align: left;
    }

    h3 {
        text-align: left;
    }
	
	p {
		font-size: 1em;
	}

	.field {
		width: 100%;
	}
</style>