using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    class TopFamousFriendsStartegy : IFriendsSortable
    {
        public void SortFriends(UserData i_UserData)
        {
           foreach (User currUser in i_UserData.Friends)
            {
                countFriendsNumOfPhotos(i_UserData, currUser);
                countFriendsNumOfStatus(i_UserData, currUser);
                countFriendsNumOfVideos(i_UserData, currUser);
            }
               
        }

        private void countFriendsNumOfPhotos(UserData i_UserData, User i_Friend)
        {
            foreach (Album currAlbum in i_Friend.Albums)
            {
                if (currAlbum.Count == 0)
                {
                    return;
                }
                else
                {
                    i_UserData.addDataToUserFriendsDict(i_Friend, currAlbum.Photos.Count);
                }
            }
        }

        private void countFriendsNumOfVideos(UserData i_UserData, User i_Friend)
        {
            if (i_Friend.Videos.Count == 0)
            {
                return;
            }
            else
            {
                i_UserData.addDataToUserFriendsDict(i_Friend, i_Friend.Videos.Count);
            }
        }

        private void countFriendsNumOfStatus(UserData i_UserData, User i_Friend)
        {
            if (i_Friend.Videos.Count == 0)
            {
                return;
            }
            else
            {
                i_UserData.addDataToUserFriendsDict(i_Friend, i_Friend.Statuses.Count);
            }

            
        }

    }
}
