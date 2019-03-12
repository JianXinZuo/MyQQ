module.exports = {
    proxy:{
        '/api':{
            target:'https://mychat01-dev-as.azurewebsites.net/api',   //接口域名
            changeOrigin: true, //是否跨域
            pathRewrite:{
                '^api':'/'  //需要rewrite的
            }
        }
    }
}