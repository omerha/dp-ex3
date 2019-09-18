using FacebookApp;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public static class TopWantedItemFactory
    {
  
        public static ITopWantedItem Build(string i_WantedItem)
        {
            ITopWantedItem res = null;
            switch (i_WantedItem)
            {
                case "Top best friends":
                    res = new TopFriendsForUser(new FriendsSorter(new BestFriendsSorterStartegy()));
                    break;
                case "Top events":
                    res = new TopEventsForUser();
                    break;
                case "Top pages":
                    res = new TopPagesForUser();
                    break;
                case "Top famous friends":
                    res = new TopFriendsForUser(new FriendsSorter(new TopFamousFriendsStartegy()));
                    break;
                default:
                    break;
            }

            return res;
        }
    }
}
