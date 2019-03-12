import Vue from 'vue'
import Vuex from 'vuex'
import actions from './action'
import mutations from './mutation'
import getters from './getters'

Vue.use(Vuex);

const state = {
    BaseURL:'https://chatdemo01.blob.core.windows.net/',
    Users:{},
    ChatUserList:[],
    onlineTotal: 0, //在线人数
    token: '',
    IsLogin: false,     //是否登录
    IsConnectOnline: false, //用户是否链接上线
    NoticeList: [],     //添加好友通知列表
    NewNotice: {        //最新的好友通知提醒
        HeadImg:'headimg01/1ace8192-5e61-4608-bd36-26c782fb972e.jpg',
        NickName:'',
        UserName:''
    },
    NoticeRemind:false,      //通知提醒
    ChatMessage: [],    //聊天内容
    MyReceiveCount:0,   //正在聊天的人收到的消息数量
    CurrentChatUser:'',     //当前正在聊天的人
    NewMsgCount: 0      //新消息的数量
}

export default new Vuex.Store({
	actions,
    mutations,
    getters,
    state
})