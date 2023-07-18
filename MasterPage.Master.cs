using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eco.utils;
public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String menu = (String)Session["menu"];

        if (menu == null || menu.Length < 50)
        {
            SessionUtils sessionUtils = new SessionUtils();
            String newMenu = sessionUtils.getDbSession("menu", "menu");
            Session["menu"] = newMenu;
            menu = newMenu;
        }
        menu = menu.Replace("index.asp","/site/index.asp");
        Label_menu.Text = menu;
    }

}
