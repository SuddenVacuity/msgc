
using System.Drawing;
using System.Windows.Forms;

namespace msgc
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

       //public void AddMenu()
       //{
       //    MainMenu menuFile = new MainMenu();
       //    this.Menu = menuFile;
       //}

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inverseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cropSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shrinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.layerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.display_canvas = new System.Windows.Forms.PictureBox();
            this.button_eraser = new System.Windows.Forms.Button();
            this.button_pencil = new System.Windows.Forms.Button();
            this.color_selector = new System.Windows.Forms.Panel();
            this.layer_3_visible = new System.Windows.Forms.PictureBox();
            this.layer_2_visible = new System.Windows.Forms.PictureBox();
            this.layer_1_visible = new System.Windows.Forms.PictureBox();
            this.layer_3 = new System.Windows.Forms.PictureBox();
            this.layer_2 = new System.Windows.Forms.PictureBox();
            this.layer_1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.color_11 = new System.Windows.Forms.PictureBox();
            this.color_12 = new System.Windows.Forms.PictureBox();
            this.color_10 = new System.Windows.Forms.PictureBox();
            this.color_9 = new System.Windows.Forms.PictureBox();
            this.color_white = new System.Windows.Forms.PictureBox();
            this.color_gray = new System.Windows.Forms.PictureBox();
            this.color_black = new System.Windows.Forms.PictureBox();
            this.color_7 = new System.Windows.Forms.PictureBox();
            this.color_8 = new System.Windows.Forms.PictureBox();
            this.color_6 = new System.Windows.Forms.PictureBox();
            this.color_5 = new System.Windows.Forms.PictureBox();
            this.color_3 = new System.Windows.Forms.PictureBox();
            this.color_4 = new System.Windows.Forms.PictureBox();
            this.color_2 = new System.Windows.Forms.PictureBox();
            this.color_1 = new System.Windows.Forms.PictureBox();
            this.text_input_color_alpha = new System.Windows.Forms.TextBox();
            this.text_input_color_blue = new System.Windows.Forms.TextBox();
            this.text_input_color_green = new System.Windows.Forms.TextBox();
            this.text_input_color_red = new System.Windows.Forms.TextBox();
            this.color_box = new System.Windows.Forms.PictureBox();
            this.color_box_alt = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.button_round_brush = new System.Windows.Forms.PictureBox();
            this.button_square_brush = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.display_canvas)).BeginInit();
            this.color_selector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layer_3_visible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_2_visible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_1_visible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_white)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_gray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_black)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_box_alt)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.button_round_brush)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_square_brush)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.selectionToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1047, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripSeparator3,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.openToolStripMenuItem.Text = "[Open]";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.saveAsToolStripMenuItem.Text = "[Save As]";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.importToolStripMenuItem.Text = "[Import]";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.exportToolStripMenuItem.Text = "[Export]";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator5,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.undoToolStripMenuItem.Text = "[Undo]";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.redoToolStripMenuItem.Text = "[Redo]";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(108, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.cutToolStripMenuItem.Text = "[Cut]";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.copyToolStripMenuItem.Text = "[Copy]";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.pasteToolStripMenuItem.Text = "[Paste]";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(108, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.viewScaleToolStripMenuItem,
            this.viewToSelectionToolStripMenuItem,
            this.toolStripSeparator6});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.zoomInToolStripMenuItem.Text = "[Zoom In]";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out]";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // viewScaleToolStripMenuItem
            // 
            this.viewScaleToolStripMenuItem.Name = "viewScaleToolStripMenuItem";
            this.viewScaleToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.viewScaleToolStripMenuItem.Text = "[View Scale]";
            this.viewScaleToolStripMenuItem.Click += new System.EventHandler(this.viewScaleToolStripMenuItem_Click);
            // 
            // viewToSelectionToolStripMenuItem
            // 
            this.viewToSelectionToolStripMenuItem.Name = "viewToSelectionToolStripMenuItem";
            this.viewToSelectionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.viewToSelectionToolStripMenuItem.Text = "[Fit View to Selection]";
            this.viewToSelectionToolStripMenuItem.Click += new System.EventHandler(this.viewToSelectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(185, 6);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // selectionToolStripMenuItem
            // 
            this.selectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.noneToolStripMenuItem,
            this.inverseSelectionToolStripMenuItem,
            this.cropSelectionToolStripMenuItem,
            this.shrinkToolStripMenuItem,
            this.transformSelectionToolStripMenuItem,
            this.toolStripSeparator7});
            this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
            this.selectionToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.selectionToolStripMenuItem.Text = "Select";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.allToolStripMenuItem.Text = "[All]";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.noneToolStripMenuItem.Text = "[None]";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // inverseSelectionToolStripMenuItem
            // 
            this.inverseSelectionToolStripMenuItem.Name = "inverseSelectionToolStripMenuItem";
            this.inverseSelectionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.inverseSelectionToolStripMenuItem.Text = "[Inverse Selection]";
            this.inverseSelectionToolStripMenuItem.Click += new System.EventHandler(this.inverseSelectionToolStripMenuItem_Click);
            // 
            // cropSelectionToolStripMenuItem
            // 
            this.cropSelectionToolStripMenuItem.Name = "cropSelectionToolStripMenuItem";
            this.cropSelectionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.cropSelectionToolStripMenuItem.Text = "[Grow]";
            this.cropSelectionToolStripMenuItem.Click += new System.EventHandler(this.cropSelectionToolStripMenuItem_Click);
            // 
            // shrinkToolStripMenuItem
            // 
            this.shrinkToolStripMenuItem.Name = "shrinkToolStripMenuItem";
            this.shrinkToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.shrinkToolStripMenuItem.Text = "[Shrink]";
            this.shrinkToolStripMenuItem.Click += new System.EventHandler(this.shrinkToolStripMenuItem_Click);
            // 
            // transformSelectionToolStripMenuItem
            // 
            this.transformSelectionToolStripMenuItem.Name = "transformSelectionToolStripMenuItem";
            this.transformSelectionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.transformSelectionToolStripMenuItem.Text = "[Transform Selection]";
            this.transformSelectionToolStripMenuItem.Click += new System.EventHandler(this.transformSelectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(185, 6);
            // 
            // layerToolStripMenuItem
            // 
            this.layerToolStripMenuItem.Name = "layerToolStripMenuItem";
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.layerToolStripMenuItem.Text = "Layers";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // display_canvas
            // 
            this.display_canvas.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.display_canvas.BackgroundImage = global::msgc.Properties.Resources.backGround;
            this.display_canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.display_canvas.Location = new System.Drawing.Point(0, 0);
            this.display_canvas.Name = "display_canvas";
            this.display_canvas.Size = new System.Drawing.Size(151, 139);
            this.display_canvas.TabIndex = 3;
            this.display_canvas.TabStop = false;
            this.display_canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.display_canvas_MouseDown);
            this.display_canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.display_canvas_MouseMove);
            this.display_canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.display_canvas_MouseUp);
            // 
            // button_eraser
            // 
            this.button_eraser.Image = global::msgc.Properties.Resources.tool_eraser;
            this.button_eraser.Location = new System.Drawing.Point(3, 49);
            this.button_eraser.Name = "button_eraser";
            this.button_eraser.Size = new System.Drawing.Size(40, 40);
            this.button_eraser.TabIndex = 2;
            this.button_eraser.UseVisualStyleBackColor = true;
            this.button_eraser.Click += new System.EventHandler(this.button_eraser_Click);
            // 
            // button_pencil
            // 
            this.button_pencil.Image = global::msgc.Properties.Resources.tool_pencil;
            this.button_pencil.Location = new System.Drawing.Point(3, 3);
            this.button_pencil.Name = "button_pencil";
            this.button_pencil.Size = new System.Drawing.Size(40, 40);
            this.button_pencil.TabIndex = 1;
            this.button_pencil.Text = "\r\n";
            this.button_pencil.UseVisualStyleBackColor = true;
            this.button_pencil.Click += new System.EventHandler(this.button_pencil_Click);
            // 
            // color_selector
            // 
            this.color_selector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.color_selector.BackColor = System.Drawing.SystemColors.ControlLight;
            this.color_selector.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.color_selector.Controls.Add(this.layer_3_visible);
            this.color_selector.Controls.Add(this.layer_2_visible);
            this.color_selector.Controls.Add(this.layer_1_visible);
            this.color_selector.Controls.Add(this.layer_3);
            this.color_selector.Controls.Add(this.layer_2);
            this.color_selector.Controls.Add(this.layer_1);
            this.color_selector.Controls.Add(this.label6);
            this.color_selector.Controls.Add(this.label4);
            this.color_selector.Controls.Add(this.label3);
            this.color_selector.Controls.Add(this.label2);
            this.color_selector.Controls.Add(this.label1);
            this.color_selector.Controls.Add(this.color_11);
            this.color_selector.Controls.Add(this.color_12);
            this.color_selector.Controls.Add(this.color_10);
            this.color_selector.Controls.Add(this.color_9);
            this.color_selector.Controls.Add(this.color_white);
            this.color_selector.Controls.Add(this.color_gray);
            this.color_selector.Controls.Add(this.color_black);
            this.color_selector.Controls.Add(this.color_7);
            this.color_selector.Controls.Add(this.color_8);
            this.color_selector.Controls.Add(this.color_6);
            this.color_selector.Controls.Add(this.color_5);
            this.color_selector.Controls.Add(this.color_3);
            this.color_selector.Controls.Add(this.color_4);
            this.color_selector.Controls.Add(this.color_2);
            this.color_selector.Controls.Add(this.color_1);
            this.color_selector.Controls.Add(this.text_input_color_alpha);
            this.color_selector.Controls.Add(this.text_input_color_blue);
            this.color_selector.Controls.Add(this.text_input_color_green);
            this.color_selector.Controls.Add(this.text_input_color_red);
            this.color_selector.Controls.Add(this.color_box);
            this.color_selector.Controls.Add(this.color_box_alt);
            this.color_selector.Location = new System.Drawing.Point(922, 27);
            this.color_selector.Name = "color_selector";
            this.color_selector.Size = new System.Drawing.Size(125, 610);
            this.color_selector.TabIndex = 6;
            // 
            // layer_3_visible
            // 
            this.layer_3_visible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_3_visible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_3_visible.Location = new System.Drawing.Point(80, 367);
            this.layer_3_visible.Name = "layer_3_visible";
            this.layer_3_visible.Size = new System.Drawing.Size(22, 22);
            this.layer_3_visible.TabIndex = 31;
            this.layer_3_visible.TabStop = false;
            this.layer_3_visible.Click += new System.EventHandler(this.layer_3_visible_Click);
            // 
            // layer_2_visible
            // 
            this.layer_2_visible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_2_visible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_2_visible.Location = new System.Drawing.Point(80, 339);
            this.layer_2_visible.Name = "layer_2_visible";
            this.layer_2_visible.Size = new System.Drawing.Size(22, 22);
            this.layer_2_visible.TabIndex = 30;
            this.layer_2_visible.TabStop = false;
            this.layer_2_visible.Click += new System.EventHandler(this.layer_2_visible_Click);
            // 
            // layer_1_visible
            // 
            this.layer_1_visible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_1_visible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_1_visible.Location = new System.Drawing.Point(80, 311);
            this.layer_1_visible.Name = "layer_1_visible";
            this.layer_1_visible.Size = new System.Drawing.Size(22, 22);
            this.layer_1_visible.TabIndex = 29;
            this.layer_1_visible.TabStop = false;
            this.layer_1_visible.Click += new System.EventHandler(this.layer_1_visible_Click);
            // 
            // layer_3
            // 
            this.layer_3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_3.Location = new System.Drawing.Point(5, 367);
            this.layer_3.Name = "layer_3";
            this.layer_3.Size = new System.Drawing.Size(66, 22);
            this.layer_3.TabIndex = 28;
            this.layer_3.TabStop = false;
            this.layer_3.Click += new System.EventHandler(this.layer_3_Click);
            // 
            // layer_2
            // 
            this.layer_2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_2.Location = new System.Drawing.Point(5, 339);
            this.layer_2.Name = "layer_2";
            this.layer_2.Size = new System.Drawing.Size(66, 22);
            this.layer_2.TabIndex = 27;
            this.layer_2.TabStop = false;
            this.layer_2.Click += new System.EventHandler(this.layer_2_Click);
            // 
            // layer_1
            // 
            this.layer_1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layer_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layer_1.Location = new System.Drawing.Point(5, 311);
            this.layer_1.Name = "layer_1";
            this.layer_1.Size = new System.Drawing.Size(66, 22);
            this.layer_1.TabIndex = 26;
            this.layer_1.TabStop = false;
            this.layer_1.Click += new System.EventHandler(this.layer_1_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Layers:              v";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "G:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "B:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "A:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "R:";
            // 
            // color_11
            // 
            this.color_11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_11.Location = new System.Drawing.Point(64, 258);
            this.color_11.Name = "color_11";
            this.color_11.Size = new System.Drawing.Size(25, 20);
            this.color_11.TabIndex = 20;
            this.color_11.TabStop = false;
            this.color_11.Click += new System.EventHandler(this.color_11_Click);
            // 
            // color_12
            // 
            this.color_12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_12.Location = new System.Drawing.Point(94, 258);
            this.color_12.Name = "color_12";
            this.color_12.Size = new System.Drawing.Size(25, 20);
            this.color_12.TabIndex = 19;
            this.color_12.TabStop = false;
            this.color_12.Click += new System.EventHandler(this.color_12_Click);
            // 
            // color_10
            // 
            this.color_10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_10.Location = new System.Drawing.Point(35, 258);
            this.color_10.Name = "color_10";
            this.color_10.Size = new System.Drawing.Size(25, 20);
            this.color_10.TabIndex = 18;
            this.color_10.TabStop = false;
            this.color_10.Click += new System.EventHandler(this.color_10_Click);
            // 
            // color_9
            // 
            this.color_9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_9.Location = new System.Drawing.Point(5, 258);
            this.color_9.Name = "color_9";
            this.color_9.Size = new System.Drawing.Size(25, 20);
            this.color_9.TabIndex = 17;
            this.color_9.TabStop = false;
            this.color_9.Click += new System.EventHandler(this.color_9_Click);
            // 
            // color_white
            // 
            this.color_white.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_white.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_white.Location = new System.Drawing.Point(80, 176);
            this.color_white.Name = "color_white";
            this.color_white.Size = new System.Drawing.Size(25, 20);
            this.color_white.TabIndex = 16;
            this.color_white.TabStop = false;
            this.color_white.Click += new System.EventHandler(this.color_white_Click);
            // 
            // color_gray
            // 
            this.color_gray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_gray.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_gray.Location = new System.Drawing.Point(50, 176);
            this.color_gray.Name = "color_gray";
            this.color_gray.Size = new System.Drawing.Size(25, 20);
            this.color_gray.TabIndex = 15;
            this.color_gray.TabStop = false;
            this.color_gray.Click += new System.EventHandler(this.color_gray_Click);
            // 
            // color_black
            // 
            this.color_black.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_black.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_black.Location = new System.Drawing.Point(20, 176);
            this.color_black.Name = "color_black";
            this.color_black.Size = new System.Drawing.Size(25, 20);
            this.color_black.TabIndex = 14;
            this.color_black.TabStop = false;
            this.color_black.Click += new System.EventHandler(this.color_black_Click);
            // 
            // color_7
            // 
            this.color_7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_7.Location = new System.Drawing.Point(64, 234);
            this.color_7.Name = "color_7";
            this.color_7.Size = new System.Drawing.Size(25, 20);
            this.color_7.TabIndex = 13;
            this.color_7.TabStop = false;
            this.color_7.Click += new System.EventHandler(this.color_7_Click);
            // 
            // color_8
            // 
            this.color_8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_8.Location = new System.Drawing.Point(94, 234);
            this.color_8.Name = "color_8";
            this.color_8.Size = new System.Drawing.Size(25, 20);
            this.color_8.TabIndex = 12;
            this.color_8.TabStop = false;
            this.color_8.Click += new System.EventHandler(this.color_8_Click);
            // 
            // color_6
            // 
            this.color_6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_6.Location = new System.Drawing.Point(35, 234);
            this.color_6.Name = "color_6";
            this.color_6.Size = new System.Drawing.Size(25, 20);
            this.color_6.TabIndex = 11;
            this.color_6.TabStop = false;
            this.color_6.Click += new System.EventHandler(this.color_6_Click);
            // 
            // color_5
            // 
            this.color_5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_5.Location = new System.Drawing.Point(5, 234);
            this.color_5.Name = "color_5";
            this.color_5.Size = new System.Drawing.Size(25, 20);
            this.color_5.TabIndex = 10;
            this.color_5.TabStop = false;
            this.color_5.Click += new System.EventHandler(this.color_5_Click);
            // 
            // color_3
            // 
            this.color_3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_3.Location = new System.Drawing.Point(64, 210);
            this.color_3.Name = "color_3";
            this.color_3.Size = new System.Drawing.Size(25, 20);
            this.color_3.TabIndex = 9;
            this.color_3.TabStop = false;
            this.color_3.Click += new System.EventHandler(this.color_3_Click);
            // 
            // color_4
            // 
            this.color_4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_4.Location = new System.Drawing.Point(94, 210);
            this.color_4.Name = "color_4";
            this.color_4.Size = new System.Drawing.Size(25, 20);
            this.color_4.TabIndex = 8;
            this.color_4.TabStop = false;
            this.color_4.Click += new System.EventHandler(this.color_4_Click);
            // 
            // color_2
            // 
            this.color_2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_2.Location = new System.Drawing.Point(35, 210);
            this.color_2.Name = "color_2";
            this.color_2.Size = new System.Drawing.Size(25, 20);
            this.color_2.TabIndex = 7;
            this.color_2.TabStop = false;
            this.color_2.Click += new System.EventHandler(this.color_2_Click);
            // 
            // color_1
            // 
            this.color_1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_1.Location = new System.Drawing.Point(5, 210);
            this.color_1.Name = "color_1";
            this.color_1.Size = new System.Drawing.Size(25, 20);
            this.color_1.TabIndex = 6;
            this.color_1.TabStop = false;
            this.color_1.Click += new System.EventHandler(this.color_1_Click);
            // 
            // text_input_color_alpha
            // 
            this.text_input_color_alpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_input_color_alpha.Location = new System.Drawing.Point(49, 144);
            this.text_input_color_alpha.Name = "text_input_color_alpha";
            this.text_input_color_alpha.Size = new System.Drawing.Size(60, 20);
            this.text_input_color_alpha.TabIndex = 5;
            this.text_input_color_alpha.TextChanged += new System.EventHandler(this.text_input_color_alpha_TextChanged);
            // 
            // text_input_color_blue
            // 
            this.text_input_color_blue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_input_color_blue.Location = new System.Drawing.Point(49, 117);
            this.text_input_color_blue.Name = "text_input_color_blue";
            this.text_input_color_blue.Size = new System.Drawing.Size(60, 20);
            this.text_input_color_blue.TabIndex = 4;
            this.text_input_color_blue.TextChanged += new System.EventHandler(this.text_input_color_blue_TextChanged);
            // 
            // text_input_color_green
            // 
            this.text_input_color_green.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_input_color_green.Location = new System.Drawing.Point(49, 90);
            this.text_input_color_green.Name = "text_input_color_green";
            this.text_input_color_green.Size = new System.Drawing.Size(60, 20);
            this.text_input_color_green.TabIndex = 3;
            this.text_input_color_green.TextChanged += new System.EventHandler(this.text_input_color_green_TextChanged);
            // 
            // text_input_color_red
            // 
            this.text_input_color_red.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_input_color_red.Location = new System.Drawing.Point(49, 63);
            this.text_input_color_red.Name = "text_input_color_red";
            this.text_input_color_red.Size = new System.Drawing.Size(60, 20);
            this.text_input_color_red.TabIndex = 2;
            this.text_input_color_red.TextChanged += new System.EventHandler(this.text_input_color_red_TextChanged);
            // 
            // color_box
            // 
            this.color_box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_box.Location = new System.Drawing.Point(36, 17);
            this.color_box.Name = "color_box";
            this.color_box.Size = new System.Drawing.Size(35, 25);
            this.color_box.TabIndex = 0;
            this.color_box.TabStop = false;
            // 
            // color_box_alt
            // 
            this.color_box_alt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.color_box_alt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color_box_alt.Location = new System.Drawing.Point(51, 27);
            this.color_box_alt.Name = "color_box_alt";
            this.color_box_alt.Size = new System.Drawing.Size(35, 25);
            this.color_box_alt.TabIndex = 1;
            this.color_box_alt.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button_round_brush);
            this.panel1.Controls.Add(this.button_square_brush);
            this.panel1.Controls.Add(this.button_pencil);
            this.panel1.Controls.Add(this.button_eraser);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(107, 610);
            this.panel1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Brush Mode:";
            // 
            // button_round_brush
            // 
            this.button_round_brush.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_round_brush.Location = new System.Drawing.Point(59, 234);
            this.button_round_brush.Name = "button_round_brush";
            this.button_round_brush.Size = new System.Drawing.Size(32, 29);
            this.button_round_brush.TabIndex = 4;
            this.button_round_brush.TabStop = false;
            this.button_round_brush.Click += new System.EventHandler(this.button_round_brush_Click);
            // 
            // button_square_brush
            // 
            this.button_square_brush.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_square_brush.Location = new System.Drawing.Point(11, 234);
            this.button_square_brush.Name = "button_square_brush";
            this.button_square_brush.Size = new System.Drawing.Size(32, 29);
            this.button_square_brush.TabIndex = 3;
            this.button_square_brush.TabStop = false;
            this.button_square_brush.Click += new System.EventHandler(this.button_square_brush_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(0, 636);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1047, 39);
            this.panel2.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.display_canvas);
            this.panel3.Location = new System.Drawing.Point(113, 27);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(803, 610);
            this.panel3.TabIndex = 9;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1047, 672);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.color_selector);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "RASTER EDITOR";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.display_canvas)).EndInit();
            this.color_selector.ResumeLayout(false);
            this.color_selector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layer_3_visible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_2_visible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_1_visible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layer_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_white)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_gray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_black)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_box_alt)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.button_round_brush)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_square_brush)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem selectionToolStripMenuItem;
        private ToolStripMenuItem layerToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem zoomInToolStripMenuItem;
        private ToolStripMenuItem zoomOutToolStripMenuItem;
        private ToolStripMenuItem viewScaleToolStripMenuItem;
        private ToolStripMenuItem viewToSelectionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem allToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem inverseSelectionToolStripMenuItem;
        private ToolStripMenuItem cropSelectionToolStripMenuItem;
        private ToolStripMenuItem shrinkToolStripMenuItem;
        private ToolStripMenuItem transformSelectionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private Button button_pencil;
        private Button button_eraser;
        public PictureBox display_canvas;
        private Panel color_selector;
        private PictureBox color_box;
        private TextBox text_input_color_alpha;
        private TextBox text_input_color_blue;
        private TextBox text_input_color_green;
        private TextBox text_input_color_red;
        private PictureBox color_box_alt;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private PictureBox color_1;
        private PictureBox color_2;
        private PictureBox color_4;
        private PictureBox color_3;
        private PictureBox color_5;
        private PictureBox color_6;
        private PictureBox color_8;
        private PictureBox color_7;
        private PictureBox color_black;
        private PictureBox color_gray;
        private PictureBox color_white;
        private PictureBox color_11;
        private PictureBox color_12;
        private PictureBox color_10;
        private PictureBox color_9;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label label2;
        private PictureBox button_round_brush;
        private PictureBox button_square_brush;
        private Label label5;
        private PictureBox layer_3_visible;
        private PictureBox layer_2_visible;
        private PictureBox layer_1_visible;
        private PictureBox layer_3;
        private PictureBox layer_2;
        private PictureBox layer_1;
        private Label label6;
    }
}

