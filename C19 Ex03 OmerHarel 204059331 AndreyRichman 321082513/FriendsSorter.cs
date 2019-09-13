using FacebookApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public class FriendsSorter
    {
        public IFriendsSortable i_CompareStartegy { get; set; }
        public UserData LocalUserData { get; set; }

        public FriendsSorter(IFriendsSortable i_Sortable)
        {
            i_CompareStartegy = i_Sortable;
        }
        public List<string> GetTopFriends()
        {
            List<string> res = null;
            i_CompareStartegy.SortFriends(LocalUserData);
            res = GetTopFiveFriends();
            return res;
        }

        private List<string> GetTopFiveFriends()
        {
            return new List<string>(OrderDictByValueInt(LocalUserData.TopFriendsDict).Keys);

        }
        private Dictionary<string, int> OrderDictByValueInt(Dictionary<string, int> i_Dict)
        {
            Dictionary<string, int> resDict = i_Dict.OrderByDescending(r => r.Value).Take(5).ToDictionary(pair => pair.Key, pair => pair.Value);
            return resDict;
        }
        
    }
}
