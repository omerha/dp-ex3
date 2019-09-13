using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public class TopFriendsForUser : ITopWantedItem
    {
        public List<string> TopList { get; set; }

        private FriendsSorter m_FriendsSorter;
        object ITopWantedItem.TopList { get => TopList; set => throw new NotImplementedException(); }

        public TopFriendsForUser(FriendsSorter i_FriendsSorter)
        {
            m_FriendsSorter = i_FriendsSorter;
        }
        public void GetData(AppLogic i_AppLogic, UserData i_UserData)
        {
            m_FriendsSorter.LocalUserData = i_UserData;
            TopList = m_FriendsSorter.GetTopFriends();
        }
    }
}
