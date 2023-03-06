import { writable } from "svelte/store";

type UserInfo = {
    userId: number,
    userName: string,
    email: string,
    passwordHash: string,
    events: any
}

const getUserInfo: () => UserInfo = () => JSON.parse(sessionStorage.getItem('userinfo'));
const setUserInfo: (userInfo: UserInfo) => void = (userInfo) => sessionStorage.setItem('userinfo', JSON.stringify(userInfo));

const {subscribe, update} = writable(getUserInfo());

export const userinfo = {
    subscribe,
    set: (_info: UserInfo) => {
        setUserInfo(_info);
        update(() => _info);
    },
    get: getUserInfo
}
export type { UserInfo };