<template>
    <v-content app>

        <v-navigation-drawer fixed v-model="IsLfetNavNenu" app>
            
            <v-img :aspect-ratio="16/9" :src='headImg' width="300" height="170" >
            <!-- src="https://cdn.vuetifyjs.com/images/parallax/material.jpg" -->
                <v-layout pa-2 column fill-height class="lightbox white--text">
                <v-spacer></v-spacer>
                <v-flex shrink>
                    <div class="subheading">{{ GetUsers.nickName }}</div>
                    <div class="body-1">{{ GetUsers.userName }}</div>
                </v-flex>
                </v-layout>
            </v-img>

            <v-list>

                <v-list-tile @click="GoToHome">
                    <v-list-tile-action>
                    <v-icon>home</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                    <v-list-tile-title>主页</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>

                <v-list-tile @click="GoToSetting">
                    <v-list-tile-action>
                    <v-icon>contact_mail</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                    <v-list-tile-title>联系人</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>

                
                <v-footer fixed color="white">
                    <v-flex xs6 @click="Logout">
                        <v-btn icon>
                            <v-icon medium dark>arrow_back</v-icon>
                        </v-btn>
                        退出账号
                    </v-flex>
                </v-footer>

            </v-list>
        </v-navigation-drawer>
      
        <v-toolbar color="blue" dark fixed app >
            <v-avatar dark :tile="false" :size="37" color="grey lighten-4" @click.stop="IsLfetNavNenu = !IsLfetNavNenu" >
                <img alt="avatar" :src='headImg'>
            </v-avatar>
            <v-layout justify-center align-center>
                <v-toolbar-title :size="5">{{ navTitle }}</v-toolbar-title>
            </v-layout>
            <v-menu offset-y>
                <v-btn icon slot="activator">
                    <v-icon medium dark>add</v-icon>
                </v-btn>
                <v-list>
                    <v-list-tile 
                        v-for="(item, index) in topMenuList"
                        :key="index"
                        @click="item.myEvent"
                    >
                        
                        <v-list-tile-title>{{ item.menuName }}</v-list-tile-title>
                    </v-list-tile>
                </v-list>
            </v-menu>
            
        </v-toolbar>

        <v-layout app>
            <router-view/>
        </v-layout>

        <v-footer color="blue" app fixed>
            <v-bottom-nav
                :active.sync="bottomNav"
                color="blue"
                :value="true"
                absolute
            >
                <v-btn dark color="blue" to="/Message" @click="GoToMessage" value="Message">
                    <span color="white">消息</span>
                    <v-icon>chat</v-icon>
                </v-btn>

                <v-btn dark color="blue" @click="GoToContact" value="Contact">
                    <span>联系人</span>
                    <v-icon>person</v-icon>
                </v-btn>

                <v-btn dark color="blue"  @click="GoToDynamic" value="Dynamic">
                    <span>动态</span>
                    <v-icon>star</v-icon>
                </v-btn>
                
            </v-bottom-nav>
        </v-footer>

<input id="avatarFileUpload" class="file" name="file" style="display:none;" 
                    type="file" 
                    accept="image/png,image/gif,image/jpeg" 
                    @change="ModifyHeadImg"/>

        <v-dialog v-model="NoticeRemind" persistent >
            <v-card>
                <v-card color="blue" class="white--text">
                    <v-layout>
                        <v-flex xs7>
                            <v-img :src=" GetBaseURL + GetNewNotice.HeadImg" height="125px" contain ></v-img>
                        </v-flex>
                        <v-flex xs7>
                            <v-card-title primary-title>
                                <div>
                                    <div class="headline">{{ GetNewNotice.NickName}}</div>
                                    <div>{{ GetNewNotice.UserName }}</div>
                                    <div>2018-09-21</div>
                                </div>
                            </v-card-title>
                        </v-flex>
                    </v-layout>
                    <v-divider light></v-divider>
                    <v-card-actions class="pa-3" >
                        对方请求添加您为好友
                        <v-spacer></v-spacer>
                        <v-btn icon slot="activator" @click.native="AgreeFriend">
                            <v-icon medium dark>add</v-icon>
                        </v-btn>
                        <v-btn icon slot="activator" @click.native="RejectFriend">
                            <v-icon medium dark>remove</v-icon>
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-card>
        </v-dialog>
    </v-content>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'
export default {
    name:'Index',
    data () {
        return {
            topMenuList:[
                {
                    icon:'',
                    menuName:'创建群聊',
                    myEvent:()=>{
                        console.log('创建群聊操作');
                    }
                },{
                    icon:'',
                    menuName:'添加好友',
                    myEvent:()=>{
                        console.log('添加好友操作');
                        this.$router.push({ path:'/AddPerson'});
                    }
                },{
                    icon:'',
                    menuName:'更换头像',
                    myEvent:()=>{
                        console.log('更换新头像');
                        document.getElementById("avatarFileUpload").click();
                    }
                },{
                    icon:'',
                    menuName:'刷新好友列表',
                    myEvent:()=>{
                        this.UpdateList();
                    }
                }
            ],
            avatarSrc:'', 
            userName:'',
            headImg:'',
            nickName:'',
            navTitle:'消息',
            bottomNav: "Message",
            tile: false,
            IsLfetNavNenu:false, //是否显示导航
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
        }
    },
    created(){
        let str = localStorage.getItem('Users');
        let user =JSON.parse(str);
        this.headImg = this.BaseURL + user.headImg;
        this.nickName = user.nickName;
        this.userName = user.userName;
    },
    mounted(){
        
    },
    computed: {
        color () {
            this.IsLfetNavNenu = false;
            this.bottomNav = "Message";
        },
        ...mapGetters([
            'NoticeRemind',
            'GetNewNotice',
            'GetBaseURL',
            'GetUsers'
        ]),

    },
    methods: {
        ...mapActions([
            'AgreeFriend',
            'RejectFriend'
        ]),

        GoToMessage(){
            this.navTitle = '消息',
            this.bottomNav="Message"
        },
        GoToContact(){
            this.navTitle = '联系人'
            this.$router.push({ path:'/Contact' });
        },
        GoToDynamic(){
            this.navTitle = '动态'
            this.$router.push({ path:'/Dynamic' });
        },
        GoToHome(){
            navTitle:'消息',
            console.log('跳转到Home 页面');
        },
        GoToSetting(){
            console.log('跳转到Setting页面');
        },
        Logout(){
            this.$store.dispatch('AccountLogout').then(()=>{

                MyConnection.stop().then(()=>{
                    this.$router.push({ path: '/login' });
                }).catch(function(err) {
                    console.log(err);
                });

            });
            
        },
        ModifyHeadImg(e){

            let file = e.target.files[0];
            let formData = new FormData(); //创建form对象
            formData.append('file',file);//通过append向form对象添加数据

            if (window.FileReader) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    this.headImg = e.target.result;
                }
                reader.readAsDataURL(file);
            }

            let config = {
                headers:{'Content-Type':'multipart/form-data'}  //添加请求头
            };

            this.$axios.post('/api/FileUpLoad',formData,config).then(res =>{
                console.log(res);

                if(!res.isok){
                    return;
                }

                let tempImg = res.data;
                let str = localStorage.getItem('Users');
                let user =JSON.parse(str);
                console.log(user);
                user.headImg = tempImg;

                this.$axios.put('/api/User/' + user.id, user).then((res)=>{

                    console.log(res);
                    localStorage.setItem('Users', JSON.stringify(res.data));
                    this.$store.dispatch('UpdateAccount',res.data);
                }).catch((err)=>{
                    
                    console.log(err);
                })

            }).catch((err)=>{
                console.log(err);
            });

        },
        UpdateList(){
            let user_id = localStorage.getItem('user_id');
            user_id && this.$axios.get('/api/user/' + user_id).then((res)=>{
                console.log(res);

                if(res.isok){
                    this.headImg = this.BaseURL + res.data.headImg;
                    this.nickName = res.data.nickName;
                    this.userName = res.data.userName;
                    localStorage.setItem("Users", JSON.stringify(res.data));
                }
                
            }).catch((err)=>{
                console.log(err);
            });
        }
    }
}
</script>
