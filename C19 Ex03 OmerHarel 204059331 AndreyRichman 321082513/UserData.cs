using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FacebookWrapper.ObjectModel;

namespace FacebookApp
{
    public class UserData
    {
        public Dictionary<string, int> TopFriendsDict { get; set; }


        private List<Post> m_NewsFeed;

        public List<Post> NewsFeed
        {
            get
            {
                if (m_NewsFeed == null)
                {
                    FetchNewsFeed();
                }

                return m_NewsFeed;
            }
        }

        public Image ImageLarge
        {
            get
            {
                return LocalUser.ImageLarge;
            }
        }

        public string Birthday
        {
            get
            {
                return LocalUser.Birthday;
            }
        }

        public string Name
        {
            get
            {
                return LocalUser.Name;
            }
        }

        public string FirstName
        {
            get
            {
                return LocalUser.FirstName;
            }
        }

        public string LastName
        {
            get
            {
                return LocalUser.LastName;
            }
        }

        private List<User> m_Friends;

        public List<User> Friends
        {
            get
            {
                if (m_Friends == null)
                {
                    FetchFriends();
                }

                return m_Friends;
            }
        }

        private List<Album> m_Albums;

        public List<Album> Albums
        {
            get
            {
                if (m_Albums == null)
                {
                    FetchAlbums();
                }

                return m_Albums;
            }
        }

        private List<Status> m_Statuses;

        public List<Status> Statuses
        {
            get
            {
                if (m_Statuses == null)
                {
                    FetchStatuses();
                }

                return m_Statuses;
            }
        }

        public User LocalUser { get; set; }

        private List<Checkin> m_Checkins;

        public List<Checkin> Checkins
        {
            get
            {
                if (m_Checkins == null)
                {
                    FetchCheckins();
                }

                return m_Checkins;
            }
        }

        public List<Page> m_Pages;

        public List<Page> Pages
        {
            get
            {
                if (m_Pages == null)
                {
                    FetchPages();
                }

                return m_Pages;
            }
        }

        public List<Event> m_Events;

        public List<Event> Events
        {
            get
            {
                if (m_Events == null)
                {
                    FetchEvents();
                }

                return m_Events;
            }
        }

        public UserData(User i_User)
        {
            LocalUser = i_User;
            TopFriendsDict = new Dictionary<string, int>();
        }

        public void FetchNewsFeed()
        {
            if (m_NewsFeed == null)
            {
                m_NewsFeed = new List<Post>();
                if (LocalUser.NewsFeed != null)
                {
                    foreach (Post post in LocalUser.NewsFeed)
                    {
                        m_NewsFeed.Add(post);
                    }
                }
            }
        }

        public void FetchFriends()
        {
            if (m_Friends == null)
            {
                m_Friends = new List<User>();

                foreach (User user in LocalUser.Friends)
                {
                    m_Friends.Add(user);
                }
            }
        }

        public void FetchAlbums()
        {
            if (m_Albums == null)
            {
                m_Albums = new List<Album>();

                foreach (Album album in LocalUser.Albums)
                {
                    if (album.Count != 0)
                    {
                        m_Albums.Add(album);
                    }
                }
            }
        }

        public void FetchStatuses()
        {
            if (m_Statuses == null)
            {
                m_Statuses = new List<Status>();
                foreach (Status currStatus in LocalUser.Statuses)
                {
                    m_Statuses.Add(currStatus);
                }
            }
        }

        public void FetchCheckins()
        {
            if (m_Checkins == null)
            {
                m_Checkins = new List<Checkin>();
                try
                {
                    if (LocalUser.Checkins != null)
                    {
                        foreach (Checkin check in LocalUser.Checkins)
                        {
                            m_Checkins.Add(check);
                        }
                    }
                }
                catch (Facebook.FacebookOAuthException)
                {
                }
            }
        }

        public void FetchEvents()
        {
            if (m_Events == null)
            {
                m_Events = new List<Event>();
                foreach (Event userEvent in LocalUser.Events)
                {
                    m_Events.Add(userEvent);
                }
            }
        }

        public void FetchPages()
        {
            if (m_Pages == null)
            {
                m_Pages = new List<Page>();
                try
                {
                    foreach (Page page in LocalUser.LikedPages)
                    {
                        m_Pages.Add(page);
                    }
                }
                catch (Facebook.FacebookOAuthException)
                {
                    System.Console.WriteLine("facebook auth error");
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

        public void addDataToUserFriendsDict(User i_FriendUser, int i_NumToAdd = 1)
        {
            string currUserNameAsKey = getUserFullName(i_FriendUser);
            if (checkIfFriendIsNotLocalUser(LocalUser, i_FriendUser))
            {
                return;
            }

            if (TopFriendsDict.ContainsKey(currUserNameAsKey))
            {
                TopFriendsDict[currUserNameAsKey] += i_NumToAdd;
            }
            else
            {
                TopFriendsDict.Add(currUserNameAsKey, i_NumToAdd);
            }
        }

        private bool checkIfFriendIsNotLocalUser(User i_LocalUser, User i_FriendUser)
        {
            return getUserFullName(i_LocalUser) == getUserFullName(i_FriendUser);
        }
    }
}
