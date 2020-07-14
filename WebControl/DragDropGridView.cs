using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CommunityBuy.WebControl
{
    /// <summary>
    /// Drag and Drop is the grid view with drag and drop the item for ordering
    /// </summary>
    public class DragAndDropEventArgs : EventArgs
    {
        private int _startIndex;
        /// <summary>
        /// Index of the source item
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }

        }
        private int _endIndex;
        /// <summary>
        /// Index of the destination item
        /// </summary>
        public int EndIndex
        {
            get { return _endIndex; }

        }
        public DragAndDropEventArgs(int startIndex, int endindex)
        {
            _startIndex = startIndex;
            _endIndex = endindex;
        }
    }
    public delegate void DragAndDrop(object sender, DragAndDropEventArgs e);

    public class DragDropGridView : GridView
    {
        /// <summary>
        /// Handle the event for the ordering
        /// </summary>
        public event DragAndDrop DragAndDrop ;

        public DragDropGridView()
        {
            
        }

        protected override void OnPagePreLoad(object sender, EventArgs e)
        {
            base.OnPagePreLoad(sender, e);
            //this.Attributes.Add("onmouseleave", "cleardragging();");
            //call this method for the register __doPostback event on the client side
            Page.ClientScript.GetPostBackEventReference(this, "");
            
        }
        protected override void OnPreRender(EventArgs e)
        {
            //register the script for item dragging
            RegisterScript();
            base.OnPreRender(e);
        }
        protected override void RaisePostBackEvent(string eventArgument)
        {
            base.RaisePostBackEvent(eventArgument);
            //Handle the post back event
            //and set the values of the source and destination items
            if (Page.Request["__EVENTARGUMENT"] != null && Page.Request["__EVENTARGUMENT"] != "" && Page.Request["__EVENTARGUMENT"].StartsWith("GridDragging"))
            {
                char[] sep = { ',' };
                string[] col = eventArgument.Split(sep);
                DragAndDrop(this, new DragAndDropEventArgs(Convert.ToInt32(col[1]), Convert.ToInt32(col[2])));
            }
        }
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
           //set the java script method for the dragging

            e.Row.Attributes.Add("onmousedown", "startDragging" + this.ClientID + "(" + e.Row.RowIndex + ",this); ");
            e.Row.Attributes.Add("onmouseup", "stopDragging" + this.ClientID + "(" + e.Row.RowIndex + "); ");
        }
        private void RegisterClientScriptVariableDeclarection(string variableName)
        {
            string strvardecl = "var " + variableName + ";";
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), variableName, strvardecl, true);
        }
        private void RegisterScript()
        {
            RegisterClientScriptVariableDeclarection("__gridview" + this.ClientID  );
            RegisterClientScriptVariableDeclarection("__item" + this.ClientID );
            RegisterClientScriptVariableDeclarection( "__startPosition" + this.ClientID );
            RegisterClientScriptVariableDeclarection("__endPosition" + this.ClientID );
            RegisterClientScriptVariableDeclarection("__tempDocumentmouseMove" + this.ClientID );
            RegisterClientScriptVariableDeclarection("__tempDocumentselectstart" + this.ClientID );
            RegisterClientScriptVariableDeclarection("__tempDocumentmouseup" + this.ClientID );
            string strvardecl = "var ____isDrag" + this.ClientID  + "=false;";
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "____isDrag" + this.ClientID, strvardecl, true);
            string strvardecl1 = "var __el" + this.ClientID + "= document.createElement('DIV');";
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "__el" + this.ClientID, strvardecl1, true);
                        
            string cleardragging =
            @" function cleardragging" + this.ClientID + @"()
            {   
            if(__isDrag" + this.ClientID + @" == true)
             {
                if(__item" + this.ClientID + @" != null)
                {
                    __item" + this.ClientID + @".innerHTML = '';
                }
                __gridview" + this.ClientID + @" = null;
                __item" + this.ClientID + @" = null;
                __startPosition" + this.ClientID + @" = null;
                __endPosition" + this.ClientID + @" = null;
                document.onmousemove =  __tempDocumentmouseMove" + this.ClientID + @"  ;
                document.onselectstart = __tempDocumentselectstart" + this.ClientID + @"  ;
                document.onmouseup = __tempDocumentmouseup" + this.ClientID + @"   ;
               
            }
            __isDrag" + this.ClientID + @" = false;
        }";
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cleardragging" + this.ClientID, cleardragging, true);
            string strStartDragging =
                @"function startDragging" + this.ClientID + @"(position,item)
            {
                __isDrag" + this.ClientID + @" = true;
                __startPosition" + this.ClientID + @" = position;
               
                __item" + this.ClientID + @" = item;
               el = __el" + this.ClientID + @";
                
                el.style.borderright='blue';
                el.style.bordertop ='blue';
                el.style.backgroundcolor= 'lightgrey';
                el.style.position = 'absolute';
              el.innerHTML ='<Table  bgcolor=gray>' + item.innerHTML + '</Table>';
                el.style.left = item.style.left;
                    el.style.top = item.style.top;
                document.body.appendChild(el);
                __item" + this.ClientID + @" = el;
                __tempDocumentmouseMove" + this.ClientID + @" = document.onmousemove;
                __tempDocumentselectstart" + this.ClientID + @" = document.onselectstart;
                __tempDocumentmouseup" + this.ClientID + @" =  document.onmouseup;
               document.onselectstart = function() { return  false; }
               document.onmousemove = function ()
                {
                    if(__item" + this.ClientID + @" != null){
                        __item" + this.ClientID + @".style.left=  window.event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft ;
                        __item" + this.ClientID + @".style.top =   window.event.clientY + document.body.scrollTop + document.documentElement.scrollTop ;
                      }
                } 
               document.onmouseup = function () { cleardragging" + this.ClientID + @"(); }
            }
            ";

            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "strStartDragging" + this.ClientID, strStartDragging, true);
            string strStoptDragging =
                @"function stopDragging" + this.ClientID + @"(position)
            {
                if(typeof(__isDrag" + this.ClientID + @") != 'undefined' && __isDrag" + this.ClientID + @" == true)
                {
                    startPosition = __startPosition" + this.ClientID + @";
                    endPosition = position
                    cleardragging" + this.ClientID + @"();
                    if(startPosition != -1 && endPosition != -1)
                    {
                        __doPostBack('" + this.ClientID.Replace("_", "$") +@"','GridDragging,'+startPosition+','+endPosition);
                    }
                }
            }
            
            ";
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "strStoptDragging" + this.ClientID, strStoptDragging, true);

        }
    }
}