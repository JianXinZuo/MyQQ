module.exports = {
    proxy: {
        '/api': {
            target: 'http://localhost:5000/api', //接口域名
            //target: 'http://140.143.80.72:5000/api', //接口域名
            changeOrigin: true, //是否跨域
            pathRewrite: {
                '^api': '/' //需要rewrite的
            }
        }
    }
}