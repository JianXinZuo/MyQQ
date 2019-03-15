import Vue from 'vue'
import Vuex from 'vuex'
import actions from './action'
import mutations from './mutation'
import getters from './getters'

Vue.use(Vuex);

const state = {
    BaseURL: 'https://chatdemo01.blob.core.windows.net/',
    Users: {},
    ChatUserList: [],
    onlineTotal: 0, //在线人数
    token: '',
    IsLogin: false, //是否登录
    IsConnectOnline: false, //用户是否链接上线
    NoticeList: [], //添加好友通知列表
    NewNotice: { //最新的好友通知提醒
        HeadImg: 'headimg01/1ace8192-5e61-4608-bd36-26c782fb972e.jpg',
        NickName: '',
        UserName: ''
    },
    NoticeRemind: false, //通知提醒

    MyReceiveCount: 0, //正在聊天的人收到的消息数量
    CurrentChatUser: '', //当前正在聊天的人
    NewMsgCount: 0, //新消息的数量

    chat_user_list: [
        //{
        // id: '',
        // user_name: '',
        // nick_name: '',
        // new_msg_count: 0,
        // head_img: '',
        // msg_list: [
        //     // {
        //     //     Me: '',
        //     //     createTime: "2019-03-14T13:24:09.4214288",
        //     //     from: "16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
        //     //     id: "65784ba0-4619-11e9-86ea-870b11593170",
        //     //     message: '',
        //     //     to: "9bf1d854-393e-4ff9-ad6f-7702b42c3088",
        //     //     type: 1
        //     // }
        // ]
        //}
    ],
    current_chat_user_id: '0',
    ChatMessage: [], //聊天内容
}

export default new Vuex.Store({
    actions,
    mutations,
    getters,
    state
})