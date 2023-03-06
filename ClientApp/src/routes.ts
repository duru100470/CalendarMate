import Home from "./Home/Home.svelte";
import Test from "./Test/Test.svelte";
import Main from "./Main/Main.svelte";
import Login from "./Auth/Login.svelte";
import Register from "./Auth/Register.svelte";
import Forgot from "./Auth/Forgot.svelte";
import NotFound from "./Error/404.svelte";

const value = {
    '/': { "comp": Home, "auth": false },
    '/test': { "comp": Test, "auth": true },
    '/main': { "comp": Main, "auth": true },
    '/auth/login': { "comp": Login, "auth": false },
    '/auth/register': { "comp": Register, "auth": false },
    '/auth/forgot': { "comp": Forgot, "auth": false },
    '/*': { "comp": NotFound, "auth": false }
};

const getRoutes = () => {
    let tmp = {};
    Object.keys(value).forEach(e => {
        tmp[e] = value[e].comp;
    })
    return tmp;
};

const getAuthRoutes = () => {
    let tmp = {};
    Object.keys(value).forEach(e => {
        tmp[e] = value[e].auth;
    })
    return tmp;
};

export const routes = getRoutes();

export const authRoutes = getAuthRoutes();