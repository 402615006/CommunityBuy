using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommunityBuy.WebControl
{
	/// <summary>
	/// 定制的DataGrid
	/// 
	/// </summary>
	[ToolboxData("<{0}:CustDataGrid runat=server></{0}:CustDataGrid>")]
	public class CustDataGrid : System.Web.UI.WebControls.DataGrid
	{
		public CustDataGrid()
		{
			this.init();
			this.Init +=new EventHandler(CustDataGrid_Init);
			this.EnableViewState = false;
		}

		protected void init()
		{
            this.CssClass = "List_tab";//"Grid_General";
			this.HeaderStyle.CssClass = "list_tab_tit";//"Grid_Header";
            this.ItemStyle.CssClass = "List_tab_tr";
            this.AlternatingItemStyle.CssClass = "List_tab_alter";//"Grid_AlternatingItem";
			this.SelectedItemStyle.CssClass = "";//"Grid_SelectedItem";
			this.PagerStyle.Visible = false;

			this.AutoGenerateColumns = false;
		}

		[Category("自定义"),
		Browsable(true),
		Description("显示序号的位置。"),
		DefaultValue("0")
		]
		public int SeqNo
		{
			get { return (ViewState["SeqNo"] != null) ? (int)ViewState["SeqNo"] : 0; }
			set { ViewState["SeqNo"] = value; }
		}


		[Category("自定义"),
		Browsable(true),
		Description("是否显示序号。"),
		DefaultValue(false)
		]
		public bool ShowSeqNo
		{
			get { return (ViewState["ShowSeqNo"] != null) ? (bool)ViewState["ShowSeqNo"] : false; }
			set { ViewState["ShowSeqNo"] = value; }
		}

		[Category("自定义"),
		Browsable(true),
		Description("分页控件ID，此属性必须设置。")]
		public string PagerID
		{
			get {return (ViewState["_GridPagerID"] != null) ? ViewState["_GridPagerID"].ToString() : "GridPager1";}
			set {ViewState["_GridPagerID"] = value;}
		}


		private void CustDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemIndex > -1)
			{
				if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
				{
					int pageSize = this.PageSize;
					int pageIndex = this.CurrentPageIndex;
					if (this.PagerID != null && this.PagerID != "")
					{
						GridPager pager = (GridPager)this.Parent.FindControl(this.PagerID);
						if(pager != null)
						{							
							pageSize = pager.PageSize;
							pageIndex = pager.CurrentPageIndex - 1;
							this.PageSize = pageSize;
							if (pageIndex * pageSize > pager.RecordCount)
							{
								pageIndex = pageIndex - 1;
							}
						}
					}
					int i = pageSize * pageIndex + e.Item.ItemIndex + 1;
					e.Item.Cells[SeqNo].Text = i.ToString();
				}
			}
		}

		private void CustDataGrid_Init(object sender, EventArgs e)
		{
			this.PageSize = 10;

			BoundColumn column = new BoundColumn();
			column.HeaderText = "序号";
			column.HeaderStyle.Width = new Unit("5%");
			column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			column.HeaderStyle.Wrap = false;
			column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			column.ItemStyle.Wrap = false;
			column.ReadOnly = true;	
            if(this.Columns.Count > 0)
            {
                bool bHaved = (this.Columns[this.SeqNo].HeaderText == "序号");					
                if (ShowSeqNo)
                {
                    if (!bHaved)
                        this.Columns.AddAt(SeqNo, column);
                    this.ItemDataBound += new DataGridItemEventHandler(CustDataGrid_ItemDataBound);
                }
                else
                {
                    if (bHaved)
                        this.Columns.RemoveAt(this.SeqNo);
                }
            }
		}

        protected void CustDataGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("class", "text");
                }
            }
        }

	}
}
