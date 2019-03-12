export default {
    CreateAccount(state,userInfo){
        //console.log(state,userInfo);
        state.Users = userInfo;
    },
    AccountLogin(state,account){

        if(!state.IsLogin){
            state.IsLogin = localStorage.getItem('IsLogin');
            state.token = localStorage.getItem('token');
            let user = localStorage.getItem('Users');
            if(user) {
                state.Users = JSON.parse(user);
            }
        }

        if(account){
            state.IsLogin = true;
            localStorage.setItem("IsLogin", true);
            state.Users = account;
            localStorage.setItem("Users", JSON.stringify(account));
        }
    },
    AccountLogout(state){
        state.Users = {};
        state.token = '';
        localStorage.setItem('token','');
        localStorage.setItem('user_id','');
        localStorage.setItem('Users', '');
        localStorage.setItem('IsLogin', false);

        setTimeout(() => {
            MyConnection.stop().catch(function (err) {
                return console.log(err);
            });
        }, 50);

    },

    //打开连接
    ConnectionOpen(state){
        setTimeout(() => {
            console.log(MyConnection);
            MyConnection.start().catch(function (err) {
                console.error(err.toString());
                window.location.reload();
            });
        }, 50);
    },

    //用户上线
    ClientOnline(state, user_id){
        if(state.IsLogin){
            setTimeout(() => {
                MyConnection.invoke("ClientOnline", user_id).catch(function (err) {
                    return console.log(err);
                });
            }, 2000);
        }
    },
    //接收用户来的信息
    ClientNoticeRemind(state, msg){
        state.NewNotice = JSON.parse(msg);
        state.NoticeRemind = true;
    },
    //同意添加好友
    AgreeFriend(state){
        state.NoticeRemind = false;
        setTimeout(() => {
            MyConnection.invoke("AgreeFriend", state.NewNotice.Id).catch(function (err) {
                return console.log(err);
            });
        }, 500);
    },
    //拒绝加好友
    RejectFriend(state){
        state.NoticeRemind = false;
        setTimeout(() => {
            MyConnection.invoke("RejectFriend", state.NewNotice.Id).catch(function (err) {
                return console.log(err);
            });
        }, 500);
    },
    //接收用户推送的文本数据
    AccpetChatMsg(state, msg){
        if(state.IsLogin){
            // {
            //     "Id":"874410e8-5931-4d46-9339-8c3a072db89e",
            //     "From":{"Id":"16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
            //         "UserName":"admin","NickName":"小新",
            //         "HeadImg":"/upload/1c0ffb31-653f-4531-8410-6ea53a380950.jpg",
            //         "Mobile":null,
            //         "Sex":true,
            //         "Age":20
            //     },
            //     "To":{
            //         "Id":"16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
            //         "UserName":"admin",
            //         "NickName":"小新",
            //         "HeadImg":"/upload/1c0ffb31-653f-4531-8410-6ea53a380950.jpg",
            //         "Mobile":null,"Sex":true,"Age":20
            //     },
            //     "Text":"sdsd",
            //     "Type":1,
            //     "CreateTime":"2018-10-11T18:03:48.8788752+08:00"
            // }
            let model = JSON.parse(msg);
            if(model.From.Id === state.Users.id){
                model.Me = true;
            }else{
                model.Me = false;
                let CurrentChatUser = JSON.parse(localStorage.getItem('CurrentChatUser'));
                if(model.From.Id !== CurrentChatUser.id){
                    state.NewMsgCount++;

                    //如果用户收到的消息不是当前聊天人的ID， 就给消息列表添加一个当前人
                    setTimeout(() => {
                        let flag = 0;
                        state.ChatUserList.forEach((item)=>{
                            if(item.id == model.From.Id){
                                item.counter++;
                                flag++;
                            }
                        });

                        if(flag === 0){
                            let obj = {
                                id: model.From.Id,
                                headImg: model.From.HeadImg,
                                nickName: model.From.NickName,
                                userName: model.From.UserName,
                                content: model.Text,
                                counter: 1,
                            }
                            state.ChatUserList.push(obj);
                            localStorage.setItem('SetChatUserList',JSON.stringify(state.ChatUserList));
                        }
                    }, 80);
                }
            }
            state.ChatMessage.push(model);
            state.MyReceiveCount++;
        }
    },
    //用户发送聊天信息（未启用）
    SendMsg(state, msg){
        state.ChatMessage.push(msg);
    },
    //分页加载历史聊天信息
    LoadChatMsg(state, list){
        if(state.IsLogin){
            list.forEach(item => {
                let msg = JSON.parse(item.message);
                let model = eval(msg);
                if(model.From.Id === state.Users.id){
                    model.Me = true;
                }else{
                    model.Me = false;
                }
                state.ChatMessage.unshift(model);
            });
        }
    },
    //设置消息用户列表
    SetChatUserList(state, use_info){
        console.log('设置消息用户列表');
        let flag = 0;
        let model = use_info;
        model.content = '';
        model.counter = 0;
        let str = localStorage.getItem('SetChatUserList');
        console.log(str);
        if(str){
            state.ChatUserList = JSON.parse(str);
            state.ChatUserList.forEach((item) => {
                if(item.id == model.id){
                    flag++;
                }
            });
        }
        if(flag === 0 ){
            state.ChatUserList.push(model);
        }
        localStorage.setItem('SetChatUserList',JSON.stringify(state.ChatUserList));
    }
}
