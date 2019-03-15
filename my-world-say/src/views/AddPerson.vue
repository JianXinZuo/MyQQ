<template>
    <v-content app>
        <v-toolbar color="blue" dark fixed app >
            <v-btn icon @click="GotoBack">
                <v-icon medium dark>arrow_back</v-icon>
            </v-btn>
            <v-layout justify-center align-center>
                <v-toolbar-title :size="5">{{ navTitle }}</v-toolbar-title>
            </v-layout>
        </v-toolbar>

        <v-card app>
            <v-flex xs12>
                <!-- <v-autocomplete
                    color="blue"
                    label="查找用户名"
                    persistent-hint
                    prepend-icon="person"
                    v-model="searchUser"
                >
                
                    <v-slide-x-reverse-transition slot="append-outer" mode="out-in">
                        <v-icon color="primary" @click="GetUserByName">search</v-icon>
                    </v-slide-x-reverse-transition>
                </v-autocomplete> -->
                <v-container>
                    <v-layout>
                        <v-flex>
                            <v-text-field
                                prepend-icon="person"
                                label="搜索"
                                placeholder="查找好友"
                                solo
                                v-model="searchUser"
                            ></v-text-field>
                            
                        </v-flex>

                        <v-btn flat icon color="blue" @click="GetUserByName">
                            <v-icon>search</v-icon>
                        </v-btn>
                        
                        <!-- <v-btn fab icon color="white" small @click="GetUserByName">
                            <v-icon>search</v-icon>
                        </v-btn> -->
                        
                    </v-layout>
                    <v-divider light vertical></v-divider>
                    
                    <template v-for="(item) in userList">
                        <v-layout>
                            <v-flex xs12>
                                <v-card color="blue" class="white--text">
                                    <v-layout>
                                        <v-flex xs5>
                                            <v-img
                                                :src="BaseURL + item.headImg"
                                                height="125px"
                                                contain
                                            ></v-img>
                                        </v-flex>
                                        <v-flex xs7>
                                            <v-card-title primary-title>
                                                <div>
                                                    <div class="headline">{{ item.nickName }}</div>
                                                    <div>{{ item.userName }}</div>
                                                    <div>{{ new Date(item.createTime).format("yyyy-MM-dd") }}</div>
                                                </div>
                                            </v-card-title>
                                        </v-flex>
                                    </v-layout>
                                    <v-divider light></v-divider>
                                    <v-card-actions class="pa-3" @click="AddPersonFunc(item)">
                                        是否添加好友 -->
                                        <v-spacer></v-spacer>
                                        <v-icon large>add</v-icon>
                                    </v-card-actions>
                                </v-card>
                            </v-flex>
                        </v-layout>
                        <v-divider light vertical></v-divider>
                    </template>
                    
                    
                </v-container>
                

            </v-flex>  
        </v-card>

        <v-dialog v-model="toast" hide-overlay persistent width="300">
            <v-card color="primary" dark>
                <v-card-text>
                    {{ toastText }}
                    <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
                </v-card-text>
            </v-card>
        </v-dialog>



    </v-content>
</template>

<script>
export default {
    name:'AddPerson',
    data () {
        return {
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            toastText: '正在添加好友...',
            toast: false, 
            isLoading: false,
            headImg:'',
            nickName:'',
            navTitle:'添加',
            searchUser:'',
            userList:[]
        }
    },
    mounted(){
    },
    computed: {
    },
    methods: {
        GotoBack(){
            this.$router.go(-1);
        },
        GetUserByName(){
            if(!this.searchUser){
                this.userList = [];
                return;
            }
            //加载数据
            this.$axios.get('/api/user/GetUserByUserName?uname=' + this.searchUser).then(res => {
                console.log(res);
                this.userList = res.data;
            }).catch(err => {
                console.log(err)
            }).finally(() => {
                this.isLoading = false
            });
        },
        AddPersonFunc(val){
            console.log(val.id);
            let user_id = localStorage.getItem('user_id');
            if(user_id != val.id){
                MyConnection.invoke("SendAddPerson", user_id, val.id, "对方请求添加你为好友").catch(function (err) {
                    return console.error(err.toString());
                });
            }else{
                console.log('');
                this.toastText = '无法添加自己为好友';
                this.toast = true;
                setTimeout(() => {
                    this.toast = false;
                }, 2000);
            }
            
        }
    }
}
</script>
