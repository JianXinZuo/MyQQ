<template>
    <v-app id="app">
        <router-view/>
    </v-app>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'App',
  data(){
    return {
      msg:'MyQL'
    }
  },
  created(){
    
    MyConnection.on("StartCallBackFunc",(connid, txt )=>{
        console.log(connid,txt);
    });

    MyConnection.on("OnlineCallbackFunc",(msg)=>{
        console.log(msg);
    });

    //断开链接方法
    MyConnection.on("OnDisconnectedAsync",(msg)=>{
        console.log('断开链接方法：',msg);
    });
    
    //接收消息
    MyConnection.on("ReceiveMessage", (msg)=>{
        console.log(msg);
        this.$store.dispatch('AccpetChatMsg', msg);
    });

    MyConnection.on("ClientNoticeRemind",(msg)=>{
        console.log(msg);
        this.$store.dispatch('ClientNoticeRemind',msg);
    });
    
    //对方同意添加你为好友
    MyConnection.on("ReceiveAgreeFriendByFromUser",(res)=>{
        console.log(res);
    });
    //同意添加好友成功
    MyConnection.on("ReceiveAgreeFriendByToUser",(res)=>{
        console.log(res);
    });

    //对方拒绝添加你为好友
    MyConnection.on("ReceiveRejectFriendByFromUser",(res)=>{
        console.log(res);
    });

  }
}
</script>
