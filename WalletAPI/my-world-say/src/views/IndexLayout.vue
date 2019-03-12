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

                this.$store.dispatch('AccountLogin',res.data);  //用户登录
                this.$store.dispatch('InitConnectStart');   //打开Signalr连接
                this.$store.dispatch('ClientOnline',user_id);   //用户上线
            }
            
        }).catch((err)=>{
            console.log(err);
        });
    },
}
</script>
