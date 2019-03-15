<template>
    <router-view/>
</template>
<script>
export default {
    name: 'IndexLayout',
    data(){
        return {
            msg:'IndexLayout',
            BaseURL:'https://chatdemo01.blob.core.windows.net/',
        }
    },
    beforeCreate(){
    },
    created(){
        
        let user_id = localStorage.getItem('user_id');
        user_id && this.$axios.get('/api/user/' + user_id).then((res)=>{
            console.log(res);

            if(res.isok){
                this.headImg = this.BaseURL + res.data.headImg;
                this.nickName = res.data.nickName;
                this.userName = res.data.userName;

                localStorage.setItem("Users", JSON.stringify(res.data));

                this.$store.dispatch('AccountLogin',res.data).then(()=>{    //用户登录
                    console.log('登陆操作');

                    MyConnection.start().then(() => {
                        console.log('ConnectionOpen方法打开链接成功');
                        MyConnection.invoke("ClientOnline", res.data.id).then(() => {
                            console.log('用户上线成功');
                        }).catch(function(err) {
                            console.log(err);
                        });
                    }).catch(function(err) {
                        console.log(err);
                    });
                }).catch(err =>{
                    console.log(err);
                });
            }
            
        }).catch((err)=>{
            console.log(err);
        });
    },
}
</script>
