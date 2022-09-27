
namespace water
{
    partial class FHeaderAsideMainFooter
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // Footer
            // 
            this.Footer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.Footer.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.Footer.Location = new System.Drawing.Point(143, 654);
            this.Footer.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.Footer.Size = new System.Drawing.Size(1007, 25);
            this.Footer.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.Footer.Visible = false;
            // 
            // Aside
            // 
            this.Aside.Location = new System.Drawing.Point(2, 96);
            this.Aside.SelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.Aside.SelectedHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.Aside.Size = new System.Drawing.Size(141, 583);
            this.Aside.Style = Sunny.UI.UIStyle.LayuiGreen;
            // 
            // Header
            // 
            this.Header.Controls.Add(this.uiLabel1);
            this.Header.Location = new System.Drawing.Point(2, 36);
            this.Header.SelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.Header.SelectedHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.Header.Size = new System.Drawing.Size(1148, 60);
            this.Header.Style = Sunny.UI.UIStyle.LayuiGreen;
            // 
            // uiLabel1
            // 
            this.uiLabel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.uiLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLabel1.Font = new System.Drawing.Font("楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(0, 0);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(1148, 60);
            this.uiLabel1.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "智能水杯生产测试工具";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiLabel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // FHeaderAsideMainFooter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1152, 681);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(171)))), ((int)(((byte)(160)))));
            this.ExtendBox = true;
            this.Name = "FHeaderAsideMainFooter";
            this.Padding = new System.Windows.Forms.Padding(2, 36, 2, 2);
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.ShowDragStretch = true;
            this.ShowRadius = false;
            this.Style = Sunny.UI.UIStyle.LayuiGreen;
            this.Text = "智能水杯生产测试工具       美唐科技";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 1212, 512);
            this.Header.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
    }
}

