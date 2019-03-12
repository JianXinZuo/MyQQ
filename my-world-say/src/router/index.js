import Vue from 'vue'
import Router from 'vue-router'
import Index from '@/views/Index'
import Message from '@/views/index/Message'
import Contact from '@/views/index/Contact'
import Dynamic from '@/views/index/Dynamic'
import Login from '@/views/Login'
import Registory from '@/views/Registory'
import AddPerson from '@/views/AddPerson'
import MyChat from '@/views/chat/MyChat'
import IndexLayout from '@/views/IndexLayout'

Vue.use(Router)

export default new Router({
  routes: [
    { 
        path: '/',
        name: 'IndexLayout',
        meta: {
            requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
        },
        component: IndexLayout,
        redirect: '/Index',
        children:[
        {
            path: '/',
            name: 'Index',
            meta: {
                requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
            },
            component: Index,
            redirect: '/Message',
            children: [
              {
                  path: '/Message',
                  name: 'Message',
                  meta: {
                    requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
                  },
                  component: Message,
              },
              {
                path: '/Contact',
                name: 'Contact',
                meta: {
                  requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
                },
                component: Contact,
              },
              {
                path: '/Dynamic',
                name: 'Dynamic',
                meta: {
                  requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
                },
                component: Dynamic,
              }
            ]
        },
        {
            path: '/AddPerson',
            name: 'AddPerson',
            meta: {
              requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
            },
            component: AddPerson
        },{
            path: '/MyChat',
            name: 'MyChat',
            meta: {
              requireAuth: true,  // 添加该字段，表示进入这个路由是需要登录的
            },
            component: MyChat
        }
      ]
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },{
      path: '/Registory',
      name: 'Registory',
      component: Registory
    }
  ]
})
