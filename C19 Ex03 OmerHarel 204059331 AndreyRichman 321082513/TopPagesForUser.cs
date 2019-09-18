using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public class TopPagesForUser : ITopWantedItem
    {
        public List<Page> TopList { get; set; }

          public ButtonColorSwapVisitor ButtonColorSwapper { get; set; }

          public void Accept(Button i_ButtonPressed)
          {
               ButtonColorSwapper.Accept(this);
               ButtonColorSwapper.Visit(i_ButtonPressed);
          }
          public Color GetClickedColor()
          {
               return Color.BlueViolet; 
          }

        object ITopWantedItem.TopList { get => TopList; set => throw new NotImplementedException(); }

        public void GetData(AppLogic i_AppLogic, UserData i_UserData)
        {
            TopList = i_AppLogic.GetTopNumberPages(i_UserData);
        }
    }
}
