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
                    "1450160541956417",
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

        private void getAllUserStatus(UserData i_UserData)
        {
            foreach (Status currStatus in i_UserData.LocalUser.Statuses)
            {
                i_UserData.Statuses.Add(currStatus);
            }
        }

        private void getUserNewsFeed(UserData i_UserData)
        {
            if (i_UserData.LocalUser.NewsFeed != null)
            {
                foreach (Post post in i_UserData.LocalUser.NewsFeed)
                {
                    i_UserData.NewsFeed.Add(post);
                }
            }
        }

        private void getUserPages(UserData i_UserData)
        {
            if (i_UserData.LocalUser.LikedPages != null)
            {
                foreach (Page page in i_UserData.LocalUser.LikedPages)
                {
                    i_UserData.Pages.Add(page);
                }
            }
        }
        
        private void getUserEvents(UserData i_UserData)
        {
            if (i_UserData.LocalUser.Events != null)
            {
                foreach (Event userEvent in i_UserData.LocalUser.Events)
                {
                    i_UserData.Events.Add(userEvent);
                }
            }
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
            ThreadStart starter = new ThreadStart(() => getAllUserFriends(i_UserData));
            starter += () =>
            {
                getAllTaggedFriendsFromCheckins(i_UserData);
                getAllTaggedFriendsFromPhotos(i_UserData);
            };
            Thread thread = new Thread(starter) { IsBackground = true };
            thread.Start();
            getAllTheNoEmptyAlbums(i_UserData);
            getAllUserStatus(i_UserData);
            getUserNewsFeed(i_UserData);
            getUserEvents(i_UserData);
        }

        private void getAllTheNoEmptyAlbums(UserData i_UserData)
        {
            foreach (Album album in i_UserData.LocalUser.Albums)
            {
                if (album.Count != 0)
                {
                    i_UserData.Albums.Add(album);
                }
            }
        }

        private string getUserFullName(User i_User)
        {
            string name;
            try
            {
                name = i_User.FirstName + " " + i_User.LastName;
            }
            catch (Exception)
            {
                name = i_User.Name;
            }

            return name;
        }

        private void collectUsersFromTag(PhotoTag i_Tag, UserData i_UserData)
        {
            string currUserNameAsKey = string.Empty;
            currUserNameAsKey = getUserFullName(i_Tag.User);
            if(checkIfFriendIsNotLocalUser(i_UserData.LocalUser, i_Tag.User))
            {
                return;
            }

            if (i_UserData.BestFriendsDict.ContainsKey(currUserNameAsKey))
            {
                i_UserData.BestFriendsDict[currUserNameAsKey] += 1;
            }
            else
            {
                i_UserData.BestFriendsDict.Add(currUserNameAsKey, 1);
            }
        }

        private bool checkIfFriendIsNotLocalUser(User i_LocalUser, User i_FriendUser)
        {
            bool isLocalUser = false;
            string currLoggedInUserFullName = getUserFullName(i_LocalUser);
            string friendFullName = getUserFullName(i_FriendUser);
            isLocalUser = friendFullName == currLoggedInUserFullName;
            return isLocalUser;
        }

        private void getAllTaggedFriendsFromPhotos(UserData i_UserData)
        {
            foreach (Album currAlbum in i_UserData.Albums)
            {
                try
                {
                    foreach (Photo currPhoto in currAlbum.Photos)
                    {
                        if (currPhoto.Tags != null)
                        {
                            try
                            {
                                foreach (PhotoTag currPhotoTag in currPhoto.Tags)
                                {
                                    collectUsersFromTag(currPhotoTag, i_UserData);
                                }
                            }
                            catch (Facebook.FacebookOAuthException)
                            {
                                continue;
                            }
                        }
                    }
                }
                catch (Facebook.FacebookOAuthException)
                {
                    continue;
                }
            }
        }

        private void getAllTaggedFriendsFromCheckins(UserData i_UserData)
        {
            string currUserNameAsKey = string.Empty;
            try
            {
                foreach (Checkin currCheckin in i_UserData.LocalUser.Checkins)
                {
                    foreach (User currUser in currCheckin.TaggedUsers)
                    {
                        currUserNameAsKey = getUserFullName(currUser);
                        if (checkIfFriendIsNotLocalUser(i_UserData.LocalUser, currUser))
                        {
                            continue;
                        }

                        if (i_UserData.BestFriendsDict.ContainsKey(currUserNameAsKey))
                        {
                            i_UserData.BestFriendsDict[currUserNameAsKey] += 1;
                        }
                        else
                        {
                            i_UserData.BestFriendsDict.Add(currUserNameAsKey, 1);
                        }
                    }
                }
            }
            catch (Facebook.FacebookOAuthException)
            {
                System.Console.WriteLine("Facebook auth error");
            }
        }

        private void getAllUserFriends(UserData i_UserData)
        {
            foreach (User user in i_UserData.LocalUser.Friends)
            {
                i_UserData.Friends.Add(user);
            }
        }

        public List<string> GetTopFiveBestFriends(UserData i_UserData)
        {
            return new List<string>(i_UserData.OrderDictByValueInt(i_UserData.BestFriendsDict).Keys);
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