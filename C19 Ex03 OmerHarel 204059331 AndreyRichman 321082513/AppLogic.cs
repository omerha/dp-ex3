using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using C19_Ex01_Omer_204059331_Andrey_321082513.sln;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookApp
{
    public class AppLogic
    {
        private static readonly object m_AppLogicLock = new object();
        private static AppLogic m_AppLogic = null;
        private readonly string r_TokenFileName = "DB.txt";
        private StoreToken m_StoreToken = new StoreToken();

        private AppLogic()
        {
        }
        
        public static AppLogic Instance
        {
            get
            {
                if (m_AppLogic == null)
                {
                    lock (m_AppLogicLock)
                    {
                        if (m_AppLogic == null)
                        {
                            m_AppLogic = new AppLogic();
                        }
                    }
                }

                return m_AppLogic;
            }
        }

        public LoginResult LoginResult { get; set; }

        public void PostPhoto(Photo i_Photo, User i_User)
        {
            i_User.PostLink(i_Photo.Link);
        }

        public UserData LoginAndInit()
        {
            UserData resUserData = null;
            if (m_StoreToken.LoadLogin(r_TokenFileName))
            {
                LoginResult = FacebookService.Connect(m_StoreToken.m_Token);
            }
            else
            {
                LoginResult = FacebookService.Login(
                    //"1450160541956417",
                    "753926335063958",
                    "public_profile",
                    "email",
                    "publish_to_groups",
                    "user_birthday",
                    "user_age_range",
                    "user_gender",
                    "user_link",
                    "user_tagged_places",
                    "user_videos",
                    "publish_to_groups",
                    "groups_access_member_info",
                    "user_friends",
                    "user_events",
                    "user_likes",
                    "user_location",
                    "user_photos",
                    "user_posts",
                    "user_hometown");

                if (!string.IsNullOrEmpty(LoginResult.AccessToken))
                {
                    m_StoreToken.SaveLogin(LoginResult.AccessToken, r_TokenFileName);
                }
            }

            resUserData = new UserData(LoginResult.LoggedInUser);
            FetchUserData(resUserData);
            return resUserData;
        }

        public Status PostStatus(string io_text, User i_User)
        {
            Status postedStatus = i_User.PostStatus(io_text);
            return postedStatus;
        }

        public string PostStatusToAllFriendsAdapter(UserData i_UserData, string i_StatusText)
        {
            string res = "Posted status successfully on all friends walls";
            foreach (User currUser in i_UserData.Friends)
            {
                try
                {
                    PostStatus(i_StatusText, currUser);
                }
                catch (Exception)
                {
                    res = "There was error trying to post status";
                    break;
                }
            }

            return res;
        }

        public User FindFriendByName(string i_friendNameToFind, UserData i_UserData)
        {
            User friend = null;
            foreach (User user in i_UserData.Friends)
            {
                    string name = user.Name.ToUpper();

                    if (name.Equals(i_friendNameToFind.ToUpper()) == true)
                    {
                        friend = user;
                        break;
                    }
            }

            return friend;
        }

        public void FetchUserData(UserData i_UserData)
        {
            //ThreadStart starter = new ThreadStart(() => getAllUserFriends(i_UserData));
            /*starter += () =>
            {
                getAllTaggedFriendsFromCheckins(i_UserData);
                getAllTaggedFriendsFromPhotos(i_UserData);
            };*/
            //Thread thread = new Thread(starter) { IsBackground = true };
            //thread.Start();
            
            //getAllUserStatus(i_UserData);
            //getUserNewsFeed(i_UserData);
            //getUserEvents(i_UserData);
        }

        public void LogOutFromFacebook()
        {
            m_StoreToken.SaveLogin(string.Empty, r_TokenFileName);
        }

        public List<Event> GetTopNumberEvents(UserData i_UserData, int i_NumOfTopEventsToReturn = 5)
        {
            List<Event> res = null;
            if (i_NumOfTopEventsToReturn <= i_UserData.Events.Count)
            {
             res = i_UserData.Events.OrderBy(currEvent => currEvent.AttendingUsers).Take(i_NumOfTopEventsToReturn).ToList();
            }

            return res;
        }

        public List<Page> GetTopNumberPages(UserData i_UserData, int i_NumOfTopPagessToReturn = 5)
        {
            List<Page> res = null;
            if (i_NumOfTopPagessToReturn <= i_UserData.Pages.Count)
            {
                res = i_UserData.Pages.OrderBy(currEvent => currEvent.LikesCount).Take(i_NumOfTopPagessToReturn).ToList();
            }

            return res;
        }
    }
}