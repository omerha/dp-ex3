using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookApp;
using System.Windows.Forms;
using System.Drawing;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public interface ITopWantedItem
    {
        object TopList { get; set; }

        ButtonColorSwapVisitor ButtonColorSwapper { get; set; }

        void GetData(AppLogic i_AppLogic, UserData i_UserData);

        Color GetClickedColor();

        void Accept(Button i_Button);
    }
}
