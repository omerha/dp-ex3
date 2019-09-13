using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    class BestFriendsSorterStartegy : IFriendsSortable
    {
        public void SortFriends(UserData i_UserData)
        {
            getAllTaggedFriendsFromCheckins(i_UserData);
            getAllTaggedFriendsFromPhotos(i_UserData);
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
                                    i_UserData.addDataToUserFriendsDict(currPhotoTag.User);
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
                        i_UserData.addDataToUserFriendsDict(currUser);
                    }
                }
            }
            catch (Facebook.FacebookOAuthException)
            {
                System.Console.WriteLine("Facebook auth error");
            }
        }
    }
}
