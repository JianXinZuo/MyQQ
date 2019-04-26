export default {
    CreateAccount(state, userInfo) {
        //console.log(state,userInfo);
        state.Users = userInfo;
    },
    UpdateAccount(state, userinfo) {
        state.Users = userinfo;
        localStorage.setItem("Users", JSON.stringify(userinfo));
    },

    AccountLogin(state, account) {

        if (!state.IsLogin) {
            state.IsLogin = localStorage.getItem('IsLogin');
            state.token = localStorage.getItem('token');
            let user = localStorage.getItem('Users');
            if (user) {
                state.Users = JSON.parse(user);
            }
        }

        if (account) {
            state.IsLogin = true;
            localStorage.setItem("IsLogin", true);
            state.Users = account;
            localStorage.setItem("Users", JSON.stringify(account));
        }
    },

    AccountLogout(state) {
        state.Users = {};
        state.token = '';
        localStorage.setItem('token', '');
        localStorage.setItem('user_id', '');
        localStorage.setItem('Users', '');
        localStorage.setItem('IsLogin', false);
        localStorage.setItem('chat_user_list', '');
    },
    UpdateChatList(state, list) {
        state.chat_user_list = list;
    },
    //同意添加好友
    AgreeFriend(state) {
        state.NoticeRemind = false;
        MyConnection.invoke("AgreeFriend", state.NewNotice.Id).catch(function(err) {
            console.log(err);
        });
    },
    //拒绝加好友
    RejectFriend(state) {
        state.NoticeRemind = false;
        MyConnection.invoke("RejectFriend", state.NewNotice.Id).catch(function(err) {
            return console.log(err);
        });
    },
    //接收用户来的信息
    ClientNoticeRemind(state, msg) {
        state.NewNotice = JSON.parse(msg);
        state.NoticeRemind = true;
    },
    //接收用户推送的文本数据
    AccpetChatMsg(state, msg) {
        if (state.IsLogin) {
            /*{
                "Id":"874410e8-5931-4d46-9339-8c3a072db89e",
                "From":{"Id":"16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
                    "UserName":"admin",
                    "NickName":"小新",
                    "HeadImg":"/upload/1c0ffb31-653f-4531-8410-6ea53a380950.jpg",
                    "Mobile":null,
                    "Sex":true,
                    "Age":20
                },
                "To":{
                    "Id":"16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
                    "UserName":"admin",
                    "NickName":"小新",
                    "HeadImg":"/upload/1c0ffb31-653f-4531-8410-6ea53a380950.jpg",
                    "Mobile":null,"Sex":true,"Age":20
                },
                "Text":"sdsd",
                "Type":1,
                "CreateTime":"2018-10-11T18:03:48.8788752+08:00"
            }*/

            let model = JSON.parse(msg);

            /* 判断新消息是否在列表中 */
            let index = -1;

            /*判断是否自己发的消息*/
            if (model.From.Id === state.Users.id) {
                model.Me = true;
                index = state.chat_user_list.findIndex((user, index) => {
                    return user.id === state.current_chat_user_id
                });
            } else {
                model.Me = false;
                index = state.chat_user_list.findIndex((user, index) => {
                    return user.id === model.From.Id;
                });
            }
            /*不在列表中的消息就添加到消息列表 */
            if (index === -1) {
                var user = {
                    id: model.From.Id,
                    user_name: model.From.UserName,
                    nick_name: model.From.NickName,
                    head_img: model.From.HeadImg,
                    mobile: model.From.Mobile,
                    sex: model.From.sex,
                    new_msg_count: 1,
                    msg_list: []
                }
                user.msg_list.push(model);
                state.chat_user_list.push(user);
            } else {

                state.chat_user_list[index].msg_list.push(model);
                /*
                if (state.chat_user_list[index].msg_list.length > 10) {
                    var arr = state.chat_user_list[index].msg_list;
                    state.chat_user_list[index].msg_list = arr.slice(arr.length - 10, arr.length);
                }*/

                //判断是否当前聊天的人
                if (state.current_chat_user_id === state.chat_user_list[index].id) {
                    state.chat_user_list[index].new_msg_count = 0;
                    state.MyReceiveCount++;
                    //state.ChatMessage.push(model);
                } else {
                    state.chat_user_list[index].new_msg_count++;
                }
            }
        }
    },
    //用户发送聊天信息（未启用）
    SendMsg(state, msg) {
        //state.ChatMessage.push(msg);
        /* 判断新消息是否在列表中 */
        let index = state.chat_user_list.findIndex((user, index) => {
            return user.id === state.current_chat_user_id
        });
        state.chat_user_list[index].msg_list.push(msg);
    },
    //分页加载历史聊天信息
    LoadChatMsg(state, list) {
        if (state.IsLogin) {

            /* 判断新消息是否在列表中 */
            let index = state.chat_user_list.findIndex((user, index) => {
                return user.id === state.current_chat_user_id;
            });
            list.forEach(item => {
                let msg = JSON.parse(item.message);
                let model = eval(msg);

                if (model.From.Id === state.Users.id) {
                    model.Me = true;
                } else {
                    model.Me = false;
                }
                state.chat_user_list[index].msg_list.unshift(model);
                state.ChatMessage = state.chat_user_list[index].msg_list;
            });
            state.chat_user_list[index]['new_msg_count'] = 0;
        }
    },
    //设置消息用户列表
    SetChatUserList(state, use_info) {
        console.log('设置消息用户列表', use_info);

        //从消息列表中查询我打开的聊天对象是否存在
        let index = state.chat_user_list.findIndex((user, index) => {
            return user.id === use_info.id
        });

        //如果聊天对象不存在， 就在消息列表增加一个聊天对象
        if (index === -1) {
            var user = {
                id: use_info.id,
                user_name: use_info.userName,
                nick_name: use_info.nickName,
                head_img: use_info.headImg,
                mobile: use_info.mobile,
                sex: use_info.sex,
                new_msg_count: 0,
                msg_list: []
            }
            state.chat_user_list.push(user);
            state.ChatMessage = [];
        } else {

            //如果存在该聊天对象，就在打开聊天窗口时，把该对象的未读消息设置为已读，未读消息数量设置为0条
            state.chat_user_list[index].new_msg_count = 0;
            state.ChatMessage = state.chat_user_list[index].msg_list;
        }

        state.current_chat_user_id = use_info.id;
        localStorage.setItem('chat_user_list', JSON.stringify(state.chat_user_list));
        localStorage.setItem('CurrentChatUser', JSON.stringify(use_info));
    }
}