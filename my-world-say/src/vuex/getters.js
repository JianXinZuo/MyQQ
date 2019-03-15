export default {
    onlineTotal: (state) => {
        return state.onlineTotal
    },
    Users: (state) => {
        return state.Users
    },
    GetUsers: (state) => {
        return state.Users
    },
    NoticeRemind: (state) => {
        return state.NoticeRemind
    },
    GetNewNotice: (state) => {
        return state.NewNotice
    },
    GetBaseURL: (state) => {
        return state.BaseURL;
    },
    GetChatMassage: (state) => {
        return state.ChatMessage;
    },
    CurrentChatUser: (state) => {
        return state.CurrentChatUser;
    },
    GetNewMsgCount: (state) => {
        return state.NewMsgCount;
    },
    MyReceiveCount: (state) => {
        return state.MyReceiveCount;
    },
    GetChatUserList: (state) => {
        return state.chat_user_list;
    }
}