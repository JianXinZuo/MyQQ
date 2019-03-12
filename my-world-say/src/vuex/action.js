export default {

    //创建账户
    CreateAccount({commit}, userInfo){
        console.log('触发了CreateAccount方法');
        commit('CreateAccount',userInfo);
    },

    //登录
    AccountLogin({ commit }, account){
        //const user_id = localStorage.getItem('user_id')
        console.log('触发了AccountLogin 方法');
        commit(
            'AccountLogin',
            account
        );
    },

    //登出
    AccountLogout({ commit }){
        //console.log('AccountLogout 方法');
        commit('AccountLogout');
    },

    InitConnectStart({ commit }){
        commit('ConnectionOpen');
    },

    //用户上线
    ClientOnline({ commit },user_id){
        commit('ClientOnline',user_id);
    },

    //接收添加好友通知
    ClientNoticeRemind({ commit }, msg ){
        commit('ClientNoticeRemind', msg );
    },

    //同意添加好友
    AgreeFriend({commit}){
        commit('AgreeFriend');
    },

    //拒绝添加好友
    RejectFriend({commit}){
        commit('RejectFriend');
    },

    //接受文本消息
    AccpetChatMsg({ commit }, msg){
        commit('AccpetChatMsg', msg);
    },
    SendMsg({ commit }, msg){
        commit('SendMsg', msg);
    },
    //下拉加载消息
    LoadChatMsg({commit}, list){
        commit('LoadChatMsg',list);
    },
    SetChatUserList({commit},use_info){
        commit('SetChatUserList',use_info);
    }
}