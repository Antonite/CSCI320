using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    private String summonerName;

    public String getName()
    {
        return summonerName;
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void submit(object sender, EventArgs e)
    {
        Session["summonerName"] = summoner_box.Text;
        Response.Redirect("main.aspx");
    }
}