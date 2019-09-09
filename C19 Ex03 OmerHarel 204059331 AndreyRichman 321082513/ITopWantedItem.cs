using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public interface ITopWantedItem
    {
        object TopList { get; set; }

        void GetData(AppLogic i_AppLogic, UserData i_UserData);
    }
}
