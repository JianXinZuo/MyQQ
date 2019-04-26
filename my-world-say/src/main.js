// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import 'babel-polyfill'
import axios from 'axios'
import store from './vuex/index'
import VueScroller from 'vue-scroller'

Vue.config.productionTip = false
Vue.use(Vuetify)
Vue.use(VueScroller)

//axios 封装
let axiosIns = axios.create({});

//生产环境
if (process.env.NODE_ENV == 'production') {
    axiosIns.defaults.baseURL = process.env.API_ROOT;
}

axiosIns.defaults.headers.post['X-Requested-With'] = 'XMLHttpRequest';
axiosIns.defaults.headers.get['X-Requested-With'] = 'XMLHttpRequest';
axiosIns.defaults.contentType = 'application/json';

axiosIns.defaults.validateStatus = function(status) {
    return true;
};

//配置拦截器AOP
axiosIns.interceptors.request.use(function(config) {
    //配置config
    // config.headers.Accept = 'application/json';

    let token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = 'bearer ' + token;
    }

    return config;
});

axiosIns.interceptors.response.use(function(response) {
    let data = response.data;

    if (response.status === 200) {
        return Promise.resolve(response);
    } else {
        return Promise.reject(response);
    }
});

let ajaxMethod = ['get', 'post', 'put', 'delete'];
let api = {};

ajaxMethod.forEach((method) => {

    api[method] = function(uri, data, config) {

        return new Promise(function(resolve, reject) {

            axiosIns[method](uri, data, config).then((res) => {
                //console.log(res);    
                resolve(res.data);
            }).catch((err) => {
                console.log(err);
                if (err.status === 401 && err.statusText === "Unauthorized") {
                    router.push({ path: '/login' });
                }
                reject(err);
            });

        });
    }
});

Vue.prototype.$axios = api;

//导航守卫
router.beforeEach((to, from, next) => {
    let is_login = false;

    try {
        is_login = JSON.parse(localStorage.getItem("IsLogin"));
    } catch (e) {
        console.log(e);
    }

    if (to.meta.requireAuth) { // 判断该路由是否需要登录权限
        if (store.state.IsLogin || is_login) { // 通过vuex state获取当前的token是否存在
            next();
        } else {
            next({
                path: '/login',
                query: { redirect: to.fullPath } // 将跳转的路由path作为参数，登录成功后跳转到该路由
            })
        }
    } else {
        next();
    }
})

Date.prototype.format = function(format) {
    var args = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var i in args) {
        var n = args[i];
        if (new RegExp("(" + i + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? n : ("00" + n).substr(("" + n).length));
    }
    return format;
};

/* eslint-disable no-new */
new Vue({
    store,
    el: '#app',
    router,
    components: { App },
    template: '<App/>'
})