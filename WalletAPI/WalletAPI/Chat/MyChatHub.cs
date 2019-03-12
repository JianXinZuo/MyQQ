using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using WalletComponent.ChatMessage;
using WalletComponent.Domains;
using WalletComponent.Repositorys;

namespace WalletAPI.Chat
{
    public class MyChatHub : Hub
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly static ConcurrentDictionary<string, string> ClientByUserId = new ConcurrentDictionary<string, string>();
        private readonly static ConcurrentDictionary<string, string> UserIdByClient = new ConcurrentDictionary<string, string>();
        private readonly static ConcurrentDictionary<string, ChatMessageUser> UserDictionary = new ConcurrentDictionary<string, ChatMessageUser>();
        
        /**
         * 配置依赖注入
         * **/
        public IUsersRepository UsersRepository { get; set; }
        public IFriendNotificationRepository FriendNotificationRepository { get; set; }
        public IUserGroupRelationshipRepository UserGroupRelationshipRepository { get; set; }
        public IChatMessagesRepository ChatMessagesRepository { get; set; }
        public IMapper Mapper { get; set; }

        /**
         * 消息推送
         * **/
        public async Task SendMessage(string msgId, string from, string to, string message, string type)
        {
            type = type.Trim().ToLower();

            if(type == "text")
            {
                TextMessage txtMsg = new TextMessage();
                txtMsg.Id = new Guid(msgId);

                if (UserDictionary.ContainsKey(from))
                {
                    txtMsg.From = UserDictionary[from];     //UsersRepository.Load();
                }
                else
                {
                    var user = UsersRepository.Load(new Guid(from));
                    txtMsg.From = Mapper.Map<ChatMessageUser>(user);
                    UserDictionary.TryAdd(from, txtMsg.From);
                }

                if (UserDictionary.ContainsKey(to))
                {
                    txtMsg.To = UserDictionary[to];     //UsersRepository.Load();
                }
                else
                {
                    var user = UsersRepository.Load(new Guid(to));
                    txtMsg.To = Mapper.Map<ChatMessageUser>(user);
                    UserDictionary.TryAdd(to, txtMsg.To);
                }

                txtMsg.Type = MessageType.Text;
                txtMsg.CreateTime = DateTime.Now;
                txtMsg.Text = message;

                ChatMessages chatMsg = new ChatMessages();
                chatMsg.Id = txtMsg.Id;
                chatMsg.From = from;
                chatMsg.To = to;
                chatMsg.Type = MessageType.Text;
                chatMsg.CreateTime = txtMsg.CreateTime;
                chatMsg.Message = txtMsg.ToJsonString();

                ChatMessagesRepository.Add(chatMsg);
                ChatMessagesRepository.SaveChanges();

                //如果对方在线，给对方发消息
                if (ClientByUserId.ContainsKey(to))
                {
                    var ToConnectionId = ClientByUserId[to];
                    await Clients.Client(ToConnectionId).SendAsync("ReceiveMessage", chatMsg.Message);
                }
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", chatMsg.Message);
            }
            //await Clients.All.SendAsync("ReceiveMessage", to, message);     //调用客户端方法
        }

        public async Task SendAddPerson(string from, string to, string msg)
        {
            string uid = string.Empty;
            string connId = string.Empty;

            if (UserIdByClient.ContainsKey(Context.ConnectionId))
                uid = UserIdByClient[Context.ConnectionId];

            if (ClientByUserId.ContainsKey(to))
                connId = ClientByUserId[to];

            var model = FriendNotificationRepository.Find( f => 
                    (f.FromUserId == new Guid(from) && f.ToUserId == new Guid(to)) || (f.FromUserId == new Guid(to) && f.ToUserId == new Guid(from))
                ).FirstOrDefault();

            if (model == null)
            {
                //通知添加到数据库
                model = new FriendNotification();
                model.Id = Guid.NewGuid();
                model.State = 1;
                model.FromUserId = new Guid(uid);
                model.FromUser = UsersRepository.Load(model.FromUserId);
                model.ToUserId = new Guid(to);
                model.ToUser = UsersRepository.Load(model.ToUserId);
                model.Message = msg;
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;

                FriendNotificationRepository.Add(model);
                FriendNotificationRepository.SaveChanges();
            }
            else
            {
                if (model.State != 3)
                {
                    model.State = 1;
                    model.Message = msg;
                    model.UpdateTime = DateTime.Now;

                    FriendNotificationRepository.SaveChanges();
                }
            }

            if (new Guid(uid) == model.ToUserId)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("OnlineCallbackFunc", "你们已经是好友了，无法重复添加");
            }
            else
            {
                AddFriend friend = new AddFriend();
                friend.Id = model.Id.ToString().ToLower();
                friend.Type = MessageType.AddUser;
                friend.FromUid = uid;
                friend.ToUid = to;
                friend.Message = msg;
                friend.State = model.State;
                friend.UserName = model.FromUser.UserName;
                friend.NickName = model.FromUser.NickName;
                friend.HeadImg = model.FromUser.HeadImg;
                friend.CreateTime = model.FromUser.CreateTime;
                friend.UpdateTime = model.FromUser.UpdateTime;

                if (!string.IsNullOrEmpty(connId) && model.State == 1)
                    await Clients.Client(connId).SendAsync("ClientNoticeRemind", friend.ToJsonString());
            }
            
        }

        public async Task ClientOnline(string userId)
        {
            try
            {
                ClientByUserId.TryAdd(userId, Context.ConnectionId);       //映射UserId => ConnectionId 
                UserIdByClient.TryAdd(Context.ConnectionId, userId);    //映射 ConnectionId => UserId

                var res = UsersRepository.Load(new Guid(userId));
                await Clients.Client(Context.ConnectionId).SendAsync("OnlineCallbackFunc", $"用户{ res.UserName }上线");
            }
            catch(Exception exp)
            {
                logger.Error("字典映射出错：" + exp.Message);
            }
        }

        /// <summary>
        /// 建立连接时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("StartCallBackFunc", Context.ConnectionId, "连接成功！");
        }

        /// <summary>
        /// 离开连接时触发
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string uid = string.Empty;
            string connId = string.Empty;

            UserIdByClient.TryRemove(Context.ConnectionId, out uid);
            ClientByUserId.TryRemove(uid, out connId);

            await Clients.All.SendAsync("OnDisconnectedAsync", $"UserId：{ uid }与连接：{ connId }的映射关系已经删除");
            logger.Info($"UserId：{ uid }与连接：{ connId }的映射关系已经删除");
        }

        /// <summary>
        /// 同意成为好友
        /// </summary>
        public async Task AgreeFriend(string friendId)
        {
            var model = FriendNotificationRepository.Find(f => f.Id == new Guid(friendId)).FirstOrDefault();

            string userId = string.Empty;
            string connId = string.Empty;
            string to = model.ToUserId.ToString().ToLower();

            if (UserIdByClient.ContainsKey(Context.ConnectionId))
                userId = UserIdByClient[Context.ConnectionId];

            if (ClientByUserId.ContainsKey(to))
                connId = ClientByUserId[to];

            //发起人添加关联
            UserGroupRelationship fromUgp = new UserGroupRelationship();
            fromUgp.Id = Guid.NewGuid();
            fromUgp.UserId = model.ToUserId;
            fromUgp.GroupId = model.FromUser.GroupList[0].Id;
            UserGroupRelationshipRepository.Add(fromUgp);

            //接收人添加关联
            UserGroupRelationship toUgp = new UserGroupRelationship();
            toUgp.Id = Guid.NewGuid();
            toUgp.UserId = model.FromUserId;
            toUgp.GroupId = model.ToUser.GroupList[0].Id;
            UserGroupRelationshipRepository.Add(toUgp);

            model.State = 3;

            UserGroupRelationshipRepository.SaveChanges();

            if (!string.IsNullOrEmpty(connId))
                await Clients.Client(connId).SendAsync("ReceiveAgreeFriendByFromUser","对方同意添加你为好友");

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveAgreeFriendByToUser", "同意成为好友");

            

        }

        /// <summary>
        /// 拒绝添加好友
        /// </summary>
        public async Task RejectFriend(string friendId)
        {
            var model = FriendNotificationRepository.Find(f => f.Id == new Guid(friendId)).FirstOrDefault();

            string userId = string.Empty;
            string connId = string.Empty;
            string to = model.ToUserId.ToString().ToLower();

            if (UserIdByClient.ContainsKey(Context.ConnectionId))
                userId = UserIdByClient[Context.ConnectionId];

            if (ClientByUserId.ContainsKey(to))
                connId = ClientByUserId[to];

            model.State = 2;
            UserGroupRelationshipRepository.SaveChanges();

            if (!string.IsNullOrEmpty(connId))
                await Clients.Client(connId).SendAsync("ReceiveRejectFriendByFromUser", "对方拒绝添加你为好友");
        }
    }
}
