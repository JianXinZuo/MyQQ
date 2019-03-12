<template>
    <v-layout app justify-center >
        <v-card>
                <v-toolbar dark color="blue" >
                    <v-btn icon @click="GotoBack">
                        <v-icon medium dark>arrow_back</v-icon>
                    </v-btn>
                    <v-toolbar-title>注册中心</v-toolbar-title>
                    <v-spacer></v-spacer>
                </v-toolbar>
                <v-container fluid grid-list-lg>
                    <v-layout row wrap>
                        
                        <v-card-text>
                            <v-form dark color="blue" >
                                <v-flex xs12 justify-center align-center text-xs-center>
                                    <v-avatar :tile="false" :size="75" color="grey lighten-4" @click="AvatarClick">
                                        <img id="myAvatarImg" alt="avatar" :src="defaultSrc">
                                    </v-avatar>
                                </v-flex>
                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="昵称"  
                                        placeholder="请输入昵称"
                                        v-model="account.NickName"
                                        type="text"
                                    ></v-text-field>
                                </v-flex>

                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="账号"  
                                        placeholder="请输入用户名"
                                        v-model="account.userName"
                                        type="text"
                                    ></v-text-field>
                                </v-flex>

                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="密码"  
                                        placeholder="请输入密码"
                                        v-model="account.password"
                                        type="password"
                                    ></v-text-field>
                                </v-flex>

                                <v-flex xs12 d-flex>
                                    <v-select 
                                        :items="SexItem" 
                                        label="性别"
                                        v-model="account.sex"
                                    ></v-select>
                                </v-flex>
                                
                                <v-flex xs12 d-flex>
                                    <v-text-field 
                                        label="年龄"  
                                        placeholder="请输入年龄"
                                        v-model="account.age"
                                        type="number"
                                    ></v-text-field>
                                </v-flex>
                            </v-form>
                        </v-card-text>

                        <v-flex xs12 d-flex>
                            <v-btn color="blue" @click="RegistoryFun" dark >注册</v-btn>
                        </v-flex>

                    </v-layout>
                </v-container>

                <input id="avatarFileUpload" class="file" name="file" style="display:none;" 
                    type="file" 
                    accept="image/png,image/gif,image/jpeg" 
                    @change="ModifyHeadImg"/>

                <v-dialog v-model="toast" hide-overlay persistent width="300">
                    <v-card color="primary" dark>
                        <v-card-text>
                            正在注册...
                            <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
                        </v-card-text>
                    </v-card>
                </v-dialog>
        </v-card>
    </v-layout>
</template>
<script>
import MyAxios from 'axios' //这个用来上传图片提交FormData表单
export default {
    name: 'Registory',
    data () {
        return {
            toast:false,
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
            defaultSrc:'https://vuetifyjs.com/apple-touch-icon-180x180.png',
            avatarSrc:'https://vuetifyjs.com/apple-touch-icon-180x180.png',
            msg: 'sss',
            SexItem:["男","女"],
            account:{
                userName:'admin',
                password:'',
                age:'20',
                sex: '男',
                HeadImg: '',
                NickName:''
            }
        }
    },
    methods: {
        GotoBack(){
            this.$router.go(-1);
        },
        AvatarClick(){
            document.getElementById("avatarFileUpload").click();
        },
        ModifyHeadImg(e){
            let file = e.target.files[0];
            let formData = new FormData(); //创建form对象
            formData.append('file',file);//通过append向form对象添加数据
            //console.log(formData.get('file')); //FormData私有类对象，访问不到，可以通过get判断值是否传进去

            if (window.FileReader) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    document.getElementById('myAvatarImg').setAttribute('src', e.target.result);
                }
                reader.readAsDataURL(file);
            }

            let config = {
                headers:{'Content-Type':'multipart/form-data'}  //添加请求头
            };

            MyAxios.post('/api/FileUpLoad',formData,config).then(res =>{
                console.log(res);

                if(res.data.isok){
                    this.avatarSrc = this.BaseURL + res.data.data;
                    this.account.HeadImg = res.data.data;
                }

            });

        },
        RegistoryFun(){
            this.toast = true;

            let model = {
                UserName: this.account.userName,
                PassWord: this.account.password,
                NickName: this.account.NickName,
                HeadImg: this.account.HeadImg,
                Sex: false,
                Age: this.account.age,
                FullName: ''
            };
            if(this.account.sex == "男"){
                model.Sex = true;
            }else if (this.account.sex == "女"){
                model.Sex = false;
            }else{
                model.Sex = false;
            }

            this.$axios.post('/api/user', model).then((res)=>{
                console.log(res);
                this.toast = false;

                if(res.isok){
                    //this.$store.dispatch("CreateAccount", res);
                    this.$router.push({ path:'/login'});
                }else{
                    
                }
                
            }).catch((e)=>{
                console.log(e);
            });
        }
    }
}
</script>
