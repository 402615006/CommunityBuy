using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
   
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            HttpManager httpManager = new HttpManager();
            httpManager.GetHttpString("http://image.baidu.com/");
        }
    }
}
