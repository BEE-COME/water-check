using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace water
{
    public partial class FHeaderAsideMainFooter : UIHeaderAsideMainFooterFrame
    {
        public FHeaderAsideMainFooter()
        {
            InitializeComponent();

            //设置关联
            Aside.TabControl = MainTabControl;

            //增加页面到Main
            AddPage(new FPage1(), 1001);
            //AddPage(new FPage2(), 1002);
            //AddPage(new FPage3(), 1003);

            //设置Header节点索引
            Aside.CreateNode("扫码测试", 1001);
            //Aside.CreateNode("进阶测试", 1002);
            //Aside.CreateNode("高级测试", 1003);

            //显示默认界面
            Aside.SelectFirst();
        }
    }

}
