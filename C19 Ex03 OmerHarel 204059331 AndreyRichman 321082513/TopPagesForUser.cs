using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public class TopPagesForUser : ITopWantedItem
    {
        public List<Page> TopList { get; set; }

        object ITopWantedItem.TopList { get => TopList; set => throw new NotImplementedException(); }

        public void GetData(AppLogic i_AppLogic, UserData i_UserData)
        {
            TopList = i_AppLogic.GetTopNumberPages(i_UserData);
        }
    }
}
