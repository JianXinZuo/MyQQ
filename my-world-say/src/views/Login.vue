<template>
    <v-layout app justify-center >
        <v-card>
                <v-toolbar dark color="blue" >
                    <v-toolbar-title>用户登录</v-toolbar-title>
                    <v-spacer></v-spacer>
                </v-toolbar>
                <v-container fluid grid-list-lg>
                    <v-layout row wrap>
                        
                        <v-card-text>
                            <v-form dark color="blue" >
                                <v-flex xs12 justify-center align-center text-xs-center>
                                    <v-avatar :tile="false" :size="75" color="grey lighten-4">
                                        <img id="myAvatarImg" alt="avatar" :src="headImg">
                                    </v-avatar>
                                </v-flex>

                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="账号"  
                                        placeholder="请输入用户名"
                                        prepend-icon="person" 
                                        v-model="account.userName"
                                        type="text"
                                    ></v-text-field>
                                </v-flex>

                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="密码"  
                                        placeholder="请输入密码"
                                        prepend-icon="lock"
                                        v-model="account.password"
                                        type="password"
                                    ></v-text-field>
                                </v-flex>
                            </v-form>
                        </v-card-text>

                        <v-flex xs12 d-flex>
                            <v-btn color="blue" @click="LoginIn" dark >登录</v-btn>
                        </v-flex>

                        <v-flex xs12 d-flex>
                            <v-btn color="blue" to="/Registory" dark >注册</v-btn>
                        </v-flex>
                        
                    </v-layout>
                </v-container>

                <v-dialog v-model="toast" hide-overlay persistent width="300">
                    <v-card color="primary" dark>
                        <v-card-text>
                            {{ toastText }}
                            <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
                        </v-card-text>
                    </v-card>
                </v-dialog>
        </v-card>
    </v-layout>
</template>
<script>
import {mapActions} from "vuex";
export default {
    name:'Login',
    data () {
        return {
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            toast: false,
            toastText: '',
            msg: '登录页面',
            headImg:'',
            account:{
                userName:'admin',
                password:''
            }
        }
    },
    created(){

        let imgSrc = localStorage.getItem('MyHeadImg');

        if(imgSrc){
            this.headImg = imgSrc;
        }else{
            this.headImg = 'https://vuetifyjs.com/apple-touch-icon-180x180.png';
        }
    },
    methods: {
        LoginIn(){
            this.toastText = '正在登录中...';
            this.toast = true;

            let model = {
                UserName: this.account.userName,
                PassWord: this.account.password
            };

            this.$axios.post('/api/Authorize',model).then((res)=>{
                console.log(res);
                if(res.isok){
                    localStorage.setItem('token',res.data.token);
                    localStorage.setItem('user_id',res.data.user.id);
                    localStorage.setItem('MyHeadImg', this.BaseURL + res.data.user.headImg);
                    this.$store.dispatch("AccountLogin", res.data.user);

                    //this.$store.dispatch('InitConnectStart');   //打开Signalr
                    //this.$store.dispatch('ClientOnline');

                    this.$router.push({ path:'/'});
                }
                this.toast = false;
            }).catch((err)=>{
                console.log(err);
                this.toast = false;
            });
        }
    }
}
</script>
