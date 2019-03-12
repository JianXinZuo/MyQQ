<template>
    <v-content app>
        <v-toolbar color="blue" dark fixed app >
            <v-badge right bottom color="#ccc">
                <v-btn icon @click="GotoBack">
                    <v-icon medium dark>arrow_back</v-icon>
                </v-btn>
                <span>{{ GetNewMsgCount == 0 ? '': GetNewMsgCount }}</span>
            </v-badge>
            <v-layout justify-center align-center>
                <v-toolbar-title :size="5">{{ CurrentChatUser.nickName }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</v-toolbar-title>
            </v-layout>
        </v-toolbar>

        <div id="chat-container" class="chat chat-container" >
            <div class="chat-top-loading" v-show="ChatLoading">
                {{ ChatLoadingText }}
                <span class="chat-top-loading-animate" >
                    <svg data-v-ecaca2b0="" viewBox="0 0 64 64" class="spinner" style="fill: rgb(170, 170, 170); stroke: rgb(170, 170, 170);"><g stroke-width="4" stroke-linecap="round"><line y1="17" y2="29" transform="translate(32,32) rotate(180)"><animate attributeName="stroke-opacity" dur="750ms" values="1;.85;.7;.65;.55;.45;.35;.25;.15;.1;0;1" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(210)"><animate attributeName="stroke-opacity" dur="750ms" values="0;1;.85;.7;.65;.55;.45;.35;.25;.15;.1;0" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(240)"><animate attributeName="stroke-opacity" dur="750ms" values=".1;0;1;.85;.7;.65;.55;.45;.35;.25;.15;.1" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(270)"><animate attributeName="stroke-opacity" dur="750ms" values=".15;.1;0;1;.85;.7;.65;.55;.45;.35;.25;.15" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(300)"><animate attributeName="stroke-opacity" dur="750ms" values=".25;.15;.1;0;1;.85;.7;.65;.55;.45;.35;.25" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(330)"><animate attributeName="stroke-opacity" dur="750ms" values=".35;.25;.15;.1;0;1;.85;.7;.65;.55;.45;.35" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(0)"><animate attributeName="stroke-opacity" dur="750ms" values=".45;.35;.25;.15;.1;0;1;.85;.7;.65;.55;.45" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(30)"><animate attributeName="stroke-opacity" dur="750ms" values=".55;.45;.35;.25;.15;.1;0;1;.85;.7;.65;.55" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(60)"><animate attributeName="stroke-opacity" dur="750ms" values=".65;.55;.45;.35;.25;.15;.1;0;1;.85;.7;.65" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(90)"><animate attributeName="stroke-opacity" dur="750ms" values=".7;.65;.55;.45;.35;.25;.15;.1;0;1;.85;.7" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(120)"><animate attributeName="stroke-opacity" dur="750ms" values=".85;.7;.65;.55;.45;.35;.25;.15;.1;0;1;.85" repeatCount="indefinite"></animate></line><line y1="17" y2="29" transform="translate(32,32) rotate(150)"><animate attributeName="stroke-opacity" dur="750ms" values="1;.85;.7;.65;.55;.45;.35;.25;.15;.1;0;1" repeatCount="indefinite"></animate></line></g></svg>
                </span>
            </div>
            <ul class="chat-list" v-scroll:#chat-container="onScroll">
                <template v-for="(item) in GetChatMassage">
                    <li v-if="item.Type == 1" class="chat-item" :class="[ item.Me ? 'right' : 'left' ]">
                        <img :src="BaseURL + item.From.HeadImg" class="chat-avatar"/>
                        <div class="bubble-item" :class="[ item.Me ? 'bubble-dinosaur' : 'bubble-bear' ]">
                        <span class="chat-content" v-text="item.Text"></span>
                        <span class="cell cell-1"></span><span class="cell cell-2"></span>
                        <span class="cell cell-3"></span><span class="cell cell-4"></span>
                        <span class="cell cell-5"></span><span class="cell cell-6"></span>
                        <span class="cell cell-7"></span><span class="cell cell-8"></span>
                        </div>
                    </li>
                    
                    <li v-else-if="item.Type == 2" class="chat-item" :class="[ item.Me ? 'right' : 'left' ]">
                        <img :src="BaseURL + item.From.HeadImg" class="chat-avatar"/>
                        <div class="bubble-item" :class="[ item.Me ? 'bubble-dinosaur' : 'bubble-bear' ]">
                        <span class="chat-content" @click="PlayVideo(item)">(((语音消息)))</span>
                        <span class="cell cell-1"></span><span class="cell cell-2"></span>
                        <span class="cell cell-3"></span><span class="cell cell-4"></span>
                        <span class="cell cell-5"></span><span class="cell cell-6"></span>
                        <span class="cell cell-7"></span><span class="cell cell-8"></span>
                        </div>
                    </li>

                </template>

            </ul>

        </div>
        <div id="div_speak" class="chat-speak">
            <div class="input-bar">
                <div class="left-area">
                    <span class="btn left-btn btn-toggle btn-voice"></span>
                </div>
                <div class="content-area">
                    <!-- <v-edit-div id="content" v-model="message" ref="mycontentedit"></v-edit-div> -->
                    <input type="text" id="content" v-model="message">
                </div>
                <div class="right-area">
                    <!-- <span class="btn btn-extra"></span> -->
                    <input type="button" id="btnSend" class="btn btn-send " value="发送" style="display:block" @click="SendTextMessage" />
                </div>
            </div>

        </div>
        
    </v-content>
</template>

<script>
import VEditDiv from '@/components/VEditDiv'
import Guid from 'node-uuid'
import { mapGetters, mapActions } from 'vuex'
export default {
    name:'MyChat',
    data () {
        return {
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            user_id: '',
            to_id:'',
            CurrentChatUser: {},
            ChatLoadingText:'正在加载中...',
            ChatLoading: false,
            LoadTimeOut: 1500,
            dom:null,
            pageIndex: 1,
            pageSize: 10,
            StopLoadData:false,
            scrollHeight:0,
            MessageType:0,
            message:''
        }
    },
    created(){
        let user_id = localStorage.getItem('user_id');
        this.user_id = user_id;
        let param = this.$route.params.val;
        this.to_id = param || localStorage.getItem('to_id');
        let toUser = JSON.parse(localStorage.getItem('CurrentChatUser'));
        this.CurrentChatUser = toUser;
        console.log('创建成功');
    },
    mounted(){
        this.dom = document.getElementById('chat-container');
        let num = this.dom.scrollHeight;
        if(this.dom.scrollTo){
            this.dom.scrollTo(0,num);
        }else{
            this.dom.scrollHeight = num;
        }

        if(this.GetChatMassage == null || this.GetChatMassage.length == 0){
            this.ChatLoading = true;

            this.$axios.get('api/ChatMessage/List?from=' + 
                this.user_id + '&to=' + this.to_id + '&index='+ this.pageIndex +'&size=' + this.pageSize
            ).then((res)=>{
                console.log(res);
                this.ChatLoading = false;

                if(res.data.length >= this.pageSize){
                    this.pageIndex ++;
                    this.StopLoadData = false;
                }else{
                    this.StopLoadData = true;
                }

                if(res.isok === true && res.total >0){
                    this.$store.dispatch('LoadChatMsg',res.data);
                    setTimeout(() => {
                        let num = this.dom.scrollHeight;
                        this.dom.scrollTo(0,num);
                    }, 50);
                }
            }).catch((err)=>{
                console.log(err);
            });
            
        }else{
            let len = this.GetChatMassage.length;
            let index = parseInt(len / this.pageSize);
            let remainder = len % this.pageSize;
            
            if(remainder === 0){
                this.pageIndex = (index + 1);
            }else{
                this.pageIndex = (index + 2);
            }
            
        }
    },
    components:{
        VEditDiv
    },
    computed: {
        ...mapGetters([
            'Users',
            'GetChatMassage',
            'GetNewMsgCount',
            'MyReceiveCount'
        ])
    },
    watch:{
        MyReceiveCount(){
            setTimeout(() => {
                let num = this.dom.scrollHeight;
                this.dom.scrollTo(0,num);
            }, 0);
        }
    },
    methods: {
        onScroll(e){
            if(e.target.scrollTop == 0){
                let model = {
                    "Id":"874410e8-5931-4d46-9339-8c3a072db89e",
                    "From":{"Id":"16dcda59-37a1-4b1f-aa9e-db1af5bb3bfe",
                        "UserName":"admin","NickName":"小新",
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
                }
                this.ChatLoading = true;

                if(this.StopLoadData === true){
                    this.ChatLoadingText ='没有更多数据...';
                    setTimeout(() => {
                        this.ChatLoading = false;
                        this.ChatLoadingText ='正在加载中...';
                    }, 500);
                    return
                }
                this.$axios.get('api/ChatMessage/List?from=' + 
                    this.user_id + '&to=' + this.to_id + '&index='+ this.pageIndex +'&size=' + this.pageSize
                ).then((res)=>{
                    this.ChatLoading = false;
                    if(res.isok === true && res.total >0){

                        this.$store.dispatch('LoadChatMsg',res.data);
                        let num = e.target.scrollHeight - this.scrollHeight;
                        e.target.scrollTop = num;
                        this.scrollHeight = e.target.scrollHeight;
                        if(res.data.length >= this.pageSize){
                            this.pageIndex ++;
                            this.StopLoadData = false;
                        }else{
                            this.StopLoadData = true;
                        }
                    }
                }).catch((err)=>{
                    console.log(err);
                });
            }
        },
        GotoBack(){
            this.$router.go(-1);
        },
        PlayVideo(){

        },
        SendTextMessage(){
            let msg = this.message.replace(/[^\u4e00-\u9fa5a-zA-Z0-9\w]/g,'');
            this.message= '';
            let msgId = Guid.v1();

            if(msg){
                MyConnection.invoke("SendMessage",
                    msgId,
                    this.user_id, 
                    this.to_id, 
                    msg, 
                    "text"
                ).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        }
    }
}
</script>
<style>
* {
  margin: 0;
  padding: 0;
}
ol, ul {
    padding-left: 0px;
}
.chat-container{
    position: relative;
    flex-direction: column;
    overflow: scroll;
    padding: 10px 20px;
    width:100%;
    overflow-y: scroll;
}
.chat-top-loading{
    text-align: center;
    padding: 0.6rem;
    color: #999;
    width: 100%;
}
.chat-top-loading-animate{
    width: 2rem;
    height: 2rem;
    display: block;
    text-align: center;
    margin: 0 auto;
}
.chat-msg{
    /* border: solid 1px red; */
    font-size: 10.5px;
    float: left;
    width: 100%;
    padding-left: 0.8em;
    padding-right: 3.5em;
    margin-top: 1em;
    display: flex;
    align-items: flex-start;
    color: #444;
    line-height: 1;
    font-family: Microsoft YaHei, Arial;
}
.chat-msg-main {
    display: -webkit-flex;
    display: flex;
    -webkit-flex-direction: column;
    flex-direction: column;
    -webkit-justify-content: flex-start;
    justify-content: flex-start;
    -webkit-align-items: stretch;
    align-items: stretch;
    max-width: 100%;
}
.chat-msg-content:not(.no-style) {
    /* background: #fff; */
    border: 1px solid #e6e6e6;
}
.chat-msg-content {
    align-self: flex-start;
    margin-top: 0.8em;
    /* min-width: 8.2em; */
    position: relative;
    color: #444;
    line-height: 1.5em;
    word-wrap: break-word;
    word-break: break-all;
    border-radius: 0.4em;
    max-width: 100%;
}
.chat-msg-content .msg-text {
    padding: 0.8em 1.2em;
    background: #fff;
}
.chat-msg .sending-status {
    display: none;
}
.chat-msg-user {
    width: 4em;
    height: 4em;
    border-radius: 50%;
    background: #e0e0e0 center no-repeat;
    background-size: cover;
    -webkit-flex: 0 0 auto;
    flex: 0 0 auto;
    display: block;
}
.chat-msg-body {
    margin-left: 1.3em;
    /* width: 100%; */
    display: -webkit-flex;
    display: flex;
    -webkit-flex-direction: column;
    flex-direction: column;
    -webkit-justify-content: flex-start;
    justify-content: flex-start;
    -webkit-align-items: flex-start;
    align-items: flex-start;
    flex: 1;
    -webkit-flex: 1;
    overflow: hidden;
}
@media screen and (max-height:568px){
    .chat{
        height: 455px;
    }
}
@media screen and (max-height:640px) and (min-height:569px){
    .chat{
        height: 528px;
    }
}
@media screen and (max-height:667px) and (min-height:641px){
    .chat{
        height: 555px;
    }
}
@media screen and (max-height:731px) and (min-height:668px){
    .chat{
        height: 618px;
    }
}
@media screen and (max-height:736px) and (min-height:732px){
    .chat{
        height: 624px;
    }
}
@media screen and (max-height:812px) and (min-height:737px){
    .chat{
        height: 700px;
    }
}
@media screen and (max-height:823px) and (min-height:813px){
    .chat{
        height: 711px;
    }
}

div[contenteditable="true"]{-webkit-user-modify: read-write-plaintext-only; }
.chat-speak{
    position:relative;z-index:1;-webkit-flex:0 0 auto;flex:0 0 auto;background-color:#fff; -webkit-touch-callout:none;
    position:absolute;bottom:0;left:0;right:0; 
    /* overflow:hidden;  */
}
.chat-speak .input-bar{position:relative;padding:10px 0;display:-webkit-flex;display:flex;-webkit-justify-content:space-between;justify-content:space-between;-webkit-align-items:flex-end;align-items:flex-end;}
.chat-speak .input-bar .left-area,.chat-speak .input-bar .right-area{-webkit-flex:0 0 auto;flex:0 0 auto;width:15%;height:100%;padding:0 5px;display:-webkit-flex;display:flex;-webkit-justify-content:center;justify-content:center;-webkit-align-items:center;align-items:center;}
.chat-speak .input-bar .content-area{-webkit-flex:1 1 auto;flex:1 1 auto;position:relative;padding-right:30px;border-radius:6px;border:1px solid #e6e6e6;}
.chat-speak .input-bar .btn{display:inline-block;width:35px;height:35px;background:none center no-repeat;background-size:contain;padding:0;font-size:inherit;border:none;}
.chat-speak .input-bar .btn-send{width:100%;padding:6px 0;border-radius:6px;background-color:#2196f3;color:#fff;}
.chat-speak .input-bar .btn-voice{background-image:url(../../assets/chatv2-btn-voice.png);}
.chat-speak .input-bar .btn-extra{background-image:url(../../assets/chatv2-btn-extra.png);}
.chat-speak .input-bar .btn:active{opacity:0.5;box-shadow:none;}
#show_bq.active{color:red;}
.btn-emoji{position:absolute;bottom:0px;right:0px;height:100%;display:inline-block;width:34px;height:34px;background:url(../../assets/emoji.png) no-repeat center center;background-size:24px;}
div#content{padding:5px 8px;right:80px;min-height:33px;max-height:34px;line-height:1.5em;font-size:16px;width:100%;border-radius:0;text-align:left;word-break:break-all;overflow-y:scroll;overflow-x:scroll;-webkit-user-select:auto;}
div#content::-webkit-scrollbar{display:none;}
div#content:focus{outline:none;}
div#content *{margin:0 !important;padding:0 !important;font-size:inherit !important;line-height:inherit !important;font-weight:inherit !important;color:inherit !important;}
div#content img.face{vertical-align:middle;margin-top:-0.1em;width:20px;height:20px;}
#content:empty::before{font-size:1.2rem;content:attr(placeholder);color:#969696;}

input#content{padding:5px 8px;right:80px;min-height:33px;max-height:34px;line-height:1.5em;font-size:16px;width:100%;border-radius:0;text-align:left;word-break:break-all;overflow-y:scroll;overflow-x:scroll;-webkit-user-select:auto;}
input#content::-webkit-scrollbar{display:none;}
input#content:focus{outline:none;}
input#content *{margin:0 !important;padding:0 !important;font-size:inherit !important;line-height:inherit !important;font-weight:inherit !important;color:inherit !important;}
input#content img.face{vertical-align:middle;margin-top:-0.1em;width:20px;height:20px;}

.chat-list {
  width: 100%;
}

.chat-item {
  display: block;
  width: 100%;
  padding-top: 5px;
}

.chat-item.left {
  float: left;
}

.chat-item.right {
  float: right;
}

.chat-item:after {
  content: '';
  display: block;
  clear: both;
  visibility: hidden;
  width: 0;
  height: 0;
}

.chat-avatar {
  display: inline-block;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: 1px solid #cdcdcd;
}

.bubble-item {
  display: inline-block;
  max-width: 80%;
}

.chat-item.left .chat-avatar,
.chat-item.left .bubble-item {
  float: left;
}

.chat-item.right .chat-avatar,
.chat-item.right .bubble-item {
  float: right;
}

.chat-item.left .chat-avatar {
  margin-right: 10px;
}

.chat-item.right .chat-avatar {
  margin-left: 10px;
}

/**
 * 气泡关键代码
 */
.bubble-item {
  position: relative;
}

.cell {
  position: absolute;
}

.bubble-bear .cell-1,
.bubble-bear .cell-3,
.bubble-bear .cell-5,
.bubble-bear .cell-7,
.bubble-dinosaur .cell-1,
.bubble-dinosaur .cell-3,
.bubble-dinosaur .cell-5,
.bubble-dinosaur .cell-7,
.bubble-luffy .cell-1,
.bubble-luffy .cell-3,
.bubble-luffy .cell-5,
.bubble-luffy .cell-7 {
  background: url(../../assets/img/bubbles.png) no-repeat;
}

.chat-content {
    word-break: break-all;
    position: relative;
    z-index: 10;
}

/* bear */

.left .bubble-bear .chat-content {
  margin: 17px 32px 15px 18px;
  display: inline-block;
  background-color: #fff8ef;
}

.left .bubble-bear .cell-1 {
  left: 0;
  top: 6px;
  width: 20px;
  height: 20px;
  background-position: -3px -10px;
}

.left .bubble-bear .cell-2 {
  left: 20px;
  top: 11px;
  right: 20px;
  height: 16px;
  background-color: #fff8ef;
  border-top: 1px solid #835426;
}

.left .bubble-bear .cell-3 {
  right: 0;
  top: 0;
  width: 60px;
  height: 28px;
  background-position: -58px -4px;
}

.left .bubble-bear .cell-4 {
  top: 28px;
  right: 6px;
  bottom: 10px;
  width: 60px;
  background-color: #fff8ef;
  border-right: 1px solid #835426;
}

.left .bubble-bear .cell-5 {
  right: 0;
  bottom: 0;
  width: 60px;
  height: 16px;
  background-position: -58px -50px;
}

.left .bubble-bear .cell-6 {
  right: 60px;
  bottom: 6px;
  left: 20px;
  height: 10px;
  background-color: #fff8ef;
  border-bottom: 1px solid #835426;
}

.left .bubble-bear .cell-7 {
  left: 0;
  bottom: 0;
  width: 20px;
  height: 20px;
  background-position: -3px -46px;
}

.left .bubble-bear .cell-8 {
  top: 25px;
  bottom: 20px;
  left: 4px;
  width: 20px;
  background-color: #fff8ef;
  border-left: 1px solid #835426;
}

.right .bubble-bear .chat-content {
  margin: 18px 18px 8px 32px;
  display: inline-block;
  background-color: #fff8ef;
}

.right .bubble-bear .cell-1 {
  left: 2px;
  top: 0;
  width: 60px;
  height: 28px;
  background-position: -124px -4px;
}

.right .bubble-bear .cell-2 {
  left: 60px;
  top: 11px;
  right: 20px;
  height: 16px;
  background-color: #fff8ef;
  border-top: 1px solid #835426;
}

.right .bubble-bear .cell-3 {
  right: 0;
  top: 5px;
  width: 20px;
  height: 20px;
  background-position: -219px -8px;
}

.right .bubble-bear .cell-4 {
  top: 23px;
  right: 6px;
  bottom: 16px;
  width: 20px;
  background-color: #fff8ef;
  border-right: 1px solid #835426;
}

.right .bubble-bear .cell-5 {
  right: 0;
  bottom: 0;
  width: 20px;
  height: 16px;
  background-position: -219px -43px;
}

.right .bubble-bear .cell-6 {
  right: 20px;
  bottom: 0;
  left: 20px;
  height: 10px;
  background-color: #fff8ef;
  border-bottom: 1px solid #835426;
}

.right .bubble-bear .cell-7 {
  left: 0;
  bottom: 0;
  width: 40px;
  height: 20px;
  background-position: -122px -40px;
}

.right .bubble-bear .cell-8 {
  top: 28px;
  bottom: 20px;
  left: 6px;
  width: 60px;
  background-color: #fff8ef;
  border-left: 1px solid #835426;
}

/* dinosaur */

.left .bubble-dinosaur .chat-content {
  margin: 17px 32px 10px 18px;
  display: inline-block;
  background-color: #8acc8e;
  color: #030001;
}

.left .bubble-dinosaur .cell-1 {
  left: 0;
  top: 6px;
  width: 20px;
  height: 20px;
  background-position: -3px -74px;
}

.left .bubble-dinosaur .cell-2 {
  left: 20px;
  top: 11px;
  right: 20px;
  height: 16px;
  background-color: #8acc8e;
  border-top: 1px solid #030001;
}

.left .bubble-dinosaur .cell-3 {
  right: 0;
  top: 0;
  width: 70px;
  height: 28px;
  background-position: -48px -68px;
}

.left .bubble-dinosaur .cell-4 {
  top: 28px;
  right: 6px;
  bottom: 10px;
  width: 70px;
  background-color: #8acc8e;
  border-right: 1px solid #030001;
}

.left .bubble-dinosaur .cell-5 {
  right: 0;
  bottom: 0;
  width: 60px;
  height: 16px;
  background-position: -55px -108px;
}

.left .bubble-dinosaur .cell-6 {
  right: 60px;
  bottom: 0;
  left: 20px;
  height: 10px;
  background-color: #8acc8e;
  border-bottom: 1px solid #030001;
}

.left .bubble-dinosaur .cell-7 {
  left: 0;
  bottom: 0;
  width: 20px;
  height: 20px;
  background-position: -3px -104px;
}

.left .bubble-dinosaur .cell-8 {
  top: 25px;
  bottom: 20px;
  left: 5px;
  width: 20px;
  background-color: #8acc8e;
  border-left: 1px solid #030001;
}

.right .bubble-dinosaur .chat-content {
  margin: 18px 18px 10px 38px;
  display: inline-block;
  background-color: #8acc8e;
  color: #030001;
}

.right .bubble-dinosaur .cell-1 {
  left: 2px;
  top: 0;
  width: 40px;
  height: 40px;
  background-position: -120px -68px;
}

.right .bubble-dinosaur .cell-2 {
  left: 40px;
  top: 10px;
  right: 20px;
  height: 16px;
  background-color: #8acc8e;
  border-top: 1px solid #030001;
}

.right .bubble-dinosaur .cell-3 {
  right: 0;
  top: 0;
  width: 20px;
  height: 26px;
  background-position: -219px -70px;
}

.right .bubble-dinosaur .cell-4 {
  top: 26px;
  right: 8px;
  bottom: 20px;
  width: 20px;
  background-color: #8acc8e;
  border-right: 1px solid #030001;
}

.right .bubble-dinosaur .cell-5 {
  right: 1px;
  bottom: 0;
  width: 20px;
  height: 24px;
  background-position: -218px -100px;
}

.right .bubble-dinosaur .cell-6 {
  right: 20px;
  bottom: 0;
  left: 30px;
  height: 10px;
  background-color: #8acc8e;
  border-bottom: 1px solid #030001;
}

.right .bubble-dinosaur .cell-7 {
  left: 4px;
  bottom: 0;
  width: 40px;
  height: 16px;
  background-position: -122px -108px;
}

.right .bubble-dinosaur .cell-8 {
  top: 33px;
  bottom: 16px;
  left: 12px;
  width: 60px;
  background-color: #8acc8e;
  border-left: 1px solid #030001;
} 

/* luffy */

.left .bubble-luffy .chat-content,
.right .bubble-luffy .chat-content{
  background-color: #feefc8;
  color: #000;
  border: 1px solid #000;
  border-radius: 10px;
  padding: 10px;
}

.left .bubble-luffy .chat-content {
  margin: 20px 35px 10px 0;
  display: inline-block;
  padding-right: 20px;
}

.left .bubble-luffy .cell-5 {
  right: 0;
  bottom: 0;
  width: 50px;
  height: 70px;
  background-position: -5px -130px;
  z-index: 10;
}

.right .bubble-luffy .chat-content {
  margin: 20px 0 10px 35px;
  display: inline-block;
  padding-left: 20px;
}

.right .bubble-luffy .cell-7 {
  left: 4px;
  bottom: 0;
  width: 50px;
  height: 70px;
  background-position: -62px -130px;
  z-index: 10;
}

/*定义滚动条高宽及背景 高宽分别对应横竖滚动条的尺寸*/
::-webkit-scrollbar 
{ 
    width: 2px;
    height: 0px; 
    background-color: #F5F5F5; 
} 
/*定义滚动条轨道 内阴影+圆角*/
::-webkit-scrollbar-track 
{ 
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3); 
    border-radius: 1px; 
    background-color: #F5F5F5; 
}
/*定义滑块 内阴影+圆角*/ 
::-webkit-scrollbar-thumb 
{ 
    border-radius: 10px; 
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3); 
    background-color: #555; 
}
</style>
