<template>
  <v-layout app justify-center >
    <v-flex xs12>
        <v-card app>
          <v-list two-line>
            <template v-for="item in UserList">
              <v-list-tile :key="item.nickName" avatar @click="OpenChat(item)">
                
                <v-list-tile-avatar>
                  <img :src="BaseURL + item.headImg">
                </v-list-tile-avatar>
                
                <v-list-tile-content>
                  <v-list-tile-title v-html="item.nickName"></v-list-tile-title>
                  <v-list-tile-sub-title v-html="item.userName">
                  </v-list-tile-sub-title>
                </v-list-tile-content>
                <v-badge left bottom color="#f00">
                  <span slot="badge">{{ item.counter }}</span>
                </v-badge>
              </v-list-tile>
              <v-divider></v-divider>
            </template>
          </v-list>
        </v-card>
    </v-flex>
  </v-layout>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'
export default {
    name: 'Message',
    data () {
        return {
            msg:'消息列表',
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            UserList:[]
        }
    },
    computed: {
      ...mapGetters([ 
        'Users',
        'GetChatUserList'
      ])
    },
    created(){
      let list = localStorage.getItem('SetChatUserList');
      if(list)
        this.UserList = JSON.parse(list);
    },
    mounted(){
        //let conn = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/myChatHub").build();
        //console.log(conn);
    },
    methods:{
        OpenChat(info){
            localStorage.setItem('to_id',info.id);
            localStorage.setItem('CurrentChatUser',JSON.stringify(info));
            this.$store.dispatch('SetChatUserList',info);
            this.$router.push(
                { 
                    path:'/MyChat',
                    name:'MyChat',
                    params:{
                        key:'toid',
                        val: info.id
                    }
                });
        }
    }
}
</script>
