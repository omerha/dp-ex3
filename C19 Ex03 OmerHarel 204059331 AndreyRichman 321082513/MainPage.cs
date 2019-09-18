using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public partial class MainPage : Form
    {
        private AppLogic m_AppLogic;
        private UserData m_UserData;
        private ITopWantedItem i_TopWantedItem;
        ButtonColorSwapVisitor m_ButtonColorSwapper = new ButtonColorSwapVisitor();

        public MainPage()
        {
            InitializeComponent();
            m_AppLogic = AppLogic.Instance;

        }

        private void updateAllPanels(UserData i_User)
        {
            if (i_User != null)
            {
                postBindingSource.DataSource = i_User.NewsFeed;
                userPostsBindingSource.DataSource = i_User.Statuses;
                friendsBindingSource.DataSource = i_User.Friends;
                eventsCreatedBindingSource.DataSource = i_User.Events;
                bestFriendsBindingSource.DataSource = i_User.TopFriendsDict;
            }
        }

        private void updateUserPanel(UserData i_ToShow)
        {
            if (i_ToShow != null && i_ToShow.LocalUser != null)
            {
                userImage.Image = i_ToShow.LocalUser.ImageLarge;
                userName.Text = i_ToShow.LocalUser.Name;
                userBirthday.Text = i_ToShow.LocalUser.Birthday;
            }
        }

        private void showUIComponents()
        {
            panelPageOwner.Visible = true;
            panelUpPart.Visible = true;
        }

        private void hideUIComponents()
        {
            panelPageOwner.Visible = false;
            panelUpPart.Visible = false;     
        }

        private void loginFacebook()
        {
            try
            {
                m_UserData = m_AppLogic.LoginAndInit();
                showUIComponents();
            }
            catch(Exception)
            {
                MessageBox.Show("There was error trying to login to Facebook, please try again.");
            }

            updateUserPanel(m_UserData);
            updateAllPanels(m_UserData);
            Text = "Welcome To Facebook!";
        }

        private void buttonPostOnAllFriends_Click(object sender, EventArgs e)
        {
           MessageBox.Show(m_AppLogic.PostStatusToAllFriendsAdapter(m_UserData, textBoxPostStatus.Text));
        }

        private void buttonLogInOut_Click(object sender, EventArgs e)
        {
            if (m_UserData != null)
            {
                m_AppLogic.LogOutFromFacebook();
                hideUIComponents();
                buttonLogInOut.Text = "Login";
                m_UserData = null;
            }
            else
            {
                loginFacebook();
                if (m_UserData != null)
                {
                    buttonLogInOut.Text = "Logout";
                }
            }
        }

        private void buttonTop_Click(object sender, EventArgs e)
        {
            i_TopWantedItem = TopWantedItemFactory.Build((sender as Button).Text);
               i_TopWantedItem.ButtonColorSwapper = m_ButtonColorSwapper;
               i_TopWantedItem.Accept(sender as Button);
               i_TopWantedItem.GetData(m_AppLogic, m_UserData);
            listBoxTops.DisplayMember = "Name";

 
            listBoxTops.DataSource = i_TopWantedItem.TopList;
            labelTopTitle.Text = (sender as Button).Text;
        }
    }
}
