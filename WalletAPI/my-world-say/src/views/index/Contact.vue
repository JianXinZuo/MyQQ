<template>
    <v-layout app justify-center >
        <v-flex xs12>
            <v-card app>
                <v-list color="white">
                        <v-list-group
                            v-for="item in Users.groupList" :key="item.id"
                            prepend-icon="person" 
                            no-action
                        >
                            <v-list-tile slot="activator">
                                <v-list-tile-content>
                                    <v-list-tile-title>{{ item.groupName }}</v-list-tile-title>
                                </v-list-tile-content>
                            </v-list-tile>

                            <v-list-tile 
                                v-for="subItem in item.userGroupRelationship"
                                :key="subItem.id"
                                avatar
                                @click="OpenChat(subItem.users)"
                            >
                                <v-list-tile-avatar>
                                    <img :src=" BaseURL + subItem.users.headImg">
                                </v-list-tile-avatar>

                                <v-list-tile-content>
                                    <v-list-tile-title v-html="subItem.users.nickName"></v-list-tile-title>
                                    <v-list-tile-sub-title v-html="subItem.users.userName"></v-list-tile-sub-title>
                                </v-list-tile-content>

                            </v-list-tile>

                        </v-list-group>
                </v-list>
            </v-card>
        </v-flex>
    </v-layout>
</template>
<script>
import { mapGetters, mapActions } from "vuex";
export default {
    name: 'Contact',
    data () {
        return {
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            msg:'联系人列表',
        }
    },
    computed: {
        ...mapGetters([
            'Users'
        ])
    },
    created(){
        
    },
    mounted(){
        //let conn = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/myChatHub").build();
        //console.log(conn.star);
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
