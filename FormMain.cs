using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using static EnumList;

namespace msgc
{
    public partial class FormMain : Form
    {
        MainProgram m_program = new MainProgram();
       
        /*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        /                          FUNCTIONS
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/

        /// <summary>
        // initializes the main form and objects in the heap
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.AllowTransparency = true;
            display_canvas.Size = new Size(1000, 1000);

            m_program.init(new Rectangle(display_canvas.Location, display_canvas.Size));

            button_round_brush.BackColor = Color.Black;

            Bitmap displayImage = m_program.getCanvasImageCopy();

            display_canvas.Image = displayImage;

            // set up color boxes
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Black;
            layer_1_visible.BackColor = Color.Black;
            layer_2_visible.BackColor = Color.Black;
            layer_3_visible.BackColor = Color.Black;

            Color selectedBrushColor = m_program.getSelectedColor();
            Color selectedBrushColorAlt = m_program.getSelectedColorAlt();

            text_input_color_alpha.Text = Convert.ToString(selectedBrushColor.A);
            text_input_color_red.Text = Convert.ToString(selectedBrushColor.R);
            text_input_color_green.Text = Convert.ToString(selectedBrushColor.G);
            text_input_color_blue.Text = Convert.ToString(selectedBrushColor.B);
            
            color_box.BackColor = selectedBrushColor;
            color_box_alt.BackColor = selectedBrushColorAlt;
            color_black.BackColor = Color.Black;
            color_gray.BackColor = Color.Gray;
            color_white.BackColor = Color.White;
            color_1.BackColor = Color.FromArgb(255, 255, 0,   0);
            color_2.BackColor = Color.FromArgb(255, 255, 127, 0);
            color_3.BackColor = Color.FromArgb(255, 255, 255, 0);
            color_4.BackColor = Color.FromArgb(255, 127, 255, 0);

            color_5.BackColor = Color.FromArgb(255, 0, 255, 0);
            color_6.BackColor = Color.FromArgb(255, 0, 255, 127);
            color_7.BackColor = Color.FromArgb(255, 0, 255, 255);
            color_8.BackColor = Color.FromArgb(255, 0, 127, 255);

            color_9.BackColor  = Color.FromArgb(255, 0,   0, 255);
            color_10.BackColor = Color.FromArgb(255, 127, 0, 255);
            color_11.BackColor = Color.FromArgb(255, 255, 0, 255);
            color_12.BackColor = Color.FromArgb(255, 255, 0, 127);

            runUnitTests();
            display_canvas.Refresh();
        }
        
        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// get and act on key event input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            int key = e.KeyChar;

            // shift lowercase letter to uppercase
            // assumes lowercase (char) have higher values than uppercase
            if (key >= 'a' && key <= 'z')
                key -= ('a' - 'A');

            bool isShiftPressed = (Control.ModifierKeys == Keys.Shift);
            bool isControlPressed = (Control.ModifierKeys == Keys.Control);

            switch (key)
            {
                default: break;
            }
            Console.Write("\nA key was pressed");

        }

        //////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////
        //                                                              //
        //                      FORM ELEMENTS                           //
        //                                                              //
        //////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////

        /////////////////////////////////////
        //  Menu Strip
        /////////////////////////////////////

        // file
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear all persisting images
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            m_program.createNewProject(display_canvas.Size);
            display_canvas.Image = m_program.getCanvasImageCopy();

            // update display
            display_canvas.Refresh();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap outImage = m_program.getFinalImageCopy();
                if (outImage != null)
                {
                    outImage.Save(Environment.CurrentDirectory + "\\output.bmp");
                    Console.Write("\nImage saved");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error saving the file.");
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // edit
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // view
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void viewScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void viewToSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void elementsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // project
        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // select
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void inverseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cropSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void transformSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void byColorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // layer

        // tools

        // window

        // help



        /////////////////////////////////////
        // tool buttons
        /////////////////////////////////////
        private void button_pencil_Click(object sender, EventArgs e)
        {
            m_program.setBrushMode(BlendMode.DrawAdd);
        }
        private void button_eraser_Click(object sender, EventArgs e)
        {
            m_program.setBrushMode(BlendMode.Erase);
        }

        /////////////////////////////////////
        // text input
        /////////////////////////////////////
        
        private int stringToByteValue(string s, int failValue)
        {
            int v = failValue;
            int.TryParse(s, out v);

            if (v > 255)
            {
                v = 255;
            }
            return v;
        }

        private void text_input_color_red_TextChanged(object sender, EventArgs e)
        {
            Color selectedColor = m_program.getSelectedColor();
            string s = text_input_color_red.Text;
            int v = stringToByteValue(s, selectedColor.R);

            selectedColor = Color.FromArgb(
                selectedColor.A,
                v,
                selectedColor.G,
                selectedColor.B);

            m_program.setSelectedColor(selectedColor);

            text_input_color_red.Text = Convert.ToString(v);
            color_box.BackColor = selectedColor;
        }
        private void text_input_color_green_TextChanged(object sender, EventArgs e)
        {
            Color selectedColor = m_program.getSelectedColor();
            string s = text_input_color_green.Text;
            int v = stringToByteValue(s, selectedColor.G);

            selectedColor = Color.FromArgb(
                selectedColor.A,
                selectedColor.R,
                v,
                selectedColor.B);

            m_program.setSelectedColor(selectedColor);

            text_input_color_green.Text = Convert.ToString(v);
            color_box.BackColor = selectedColor;
        }
        private void text_input_color_blue_TextChanged(object sender, EventArgs e)
        {
            Color selectedColor = m_program.getSelectedColor();
            string s = text_input_color_blue.Text;
            int v = stringToByteValue(s, selectedColor.B);

            selectedColor = Color.FromArgb(
                selectedColor.A,
                selectedColor.R,
                selectedColor.G,
                v);

            m_program.setSelectedColor(selectedColor);

            text_input_color_blue.Text = Convert.ToString(v);
            color_box.BackColor = selectedColor;
        }
        private void text_input_color_alpha_TextChanged(object sender, EventArgs e)
        {
            Color selectedColor = m_program.getSelectedColor();
            string s = text_input_color_alpha.Text;
            int v = stringToByteValue(s, selectedColor.A);

            selectedColor = Color.FromArgb(
                v,
                selectedColor.R,
                selectedColor.G,
                selectedColor.B);

            m_program.setSelectedColor(selectedColor);

            text_input_color_alpha.Text = Convert.ToString(v);
            color_box.BackColor = selectedColor;
        }


        /////////////////////////////////////
        // color selection
        /////////////////////////////////////

        private void updateColorInputTextBoxes(Color color)
        {
            m_program.setSelectedColor(color);
            Color selectedColor = m_program.getSelectedColor();

            color_box.BackColor = selectedColor;
            text_input_color_alpha.Text = Convert.ToString(selectedColor.A);
            text_input_color_red.Text = Convert.ToString(selectedColor.R);
            text_input_color_green.Text = Convert.ToString(selectedColor.G);
            text_input_color_blue.Text = Convert.ToString(selectedColor.B);

            m_program.setSelectedColor(selectedColor);
        }
        private void color_black_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_black.BackColor);
        }
        private void color_gray_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_gray.BackColor);
        }
        private void color_white_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_white.BackColor);
        }
        private void color_1_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_1.BackColor);
        }
        private void color_2_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_2.BackColor);
        }
        private void color_3_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_3.BackColor);
        }
        private void color_4_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_4.BackColor);
        }
        private void color_5_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_5.BackColor);
        }
        private void color_6_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_6.BackColor);
        }
        private void color_7_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_7.BackColor);
        }
        private void color_8_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_8.BackColor);
        }
        private void color_9_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_9.BackColor);
        }
        private void color_10_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_10.BackColor);
        }
        private void color_11_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_11.BackColor);
        }
        private void color_12_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_12.BackColor);
        }

        /////////////////////////////////////
        // canvas
        /////////////////////////////////////

        /// <summary>
        /// Runs when the mouse button is pressed. Sets m_isDrawing to true. 
        /// Gets current mouse position. 
        /// Sets m_redrawDrawBuffer to the current mouse position. 
        /// Draws to m_drawBuffer. 
        /// Flattens layers and buffer to display_canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseDown(object sender, MouseEventArgs e)
        {
            m_program.onMouseDown(0, display_canvas.PointToClient(Cursor.Position));

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        /// <summary>
        /// Runs whenever the mouse moves. 
        /// If m_isDrawing is true: 
        ///    Gets the current mouse position. 
        ///    Draws to m_drawBuffer. 
        ///    Flattens layers and buffer to display_canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            m_program.onMouseMove(0, display_canvas.PointToClient(Cursor.Position));
            
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            
            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }
        
        /// <summary>
        /// Runs when mouse up event trggers.
        /// Sets m_isDrawing to false.
        /// Flattens all layers and buffer to m_finalImage.
        /// Copies Image to m_canvasImage and display_canvas.
        /// Clears and m_drawBuffer and resets m_redrawDrawBufferZone.
        /// Runs garbage collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseUp(object sender, MouseEventArgs e)
        {
            m_program.onMouseUp(0);
            
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            
            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////                                                                                                    ////
        /////                TEMPORARY INTERFACE FUNCTIONS AND TEST FUNCTIONS PAST THIS POINT                    ////
        /////                                                                                                    ////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void doesNothing() { /*   ONLY EXISTS SO THAT GENERATED CODE APPEARS BELOW HERE    */}

        private void button_square_brush_Click(object sender, EventArgs e)
        {
            button_round_brush.BackColor = Color.Transparent;
            button_square_brush.BackColor = Color.Black;

            Bitmap brushImage = new Bitmap(30, 30);
            for (int i = 0; i < brushImage.Size.Height; i++)
                for (int j = 0; j < brushImage.Size.Width; j++)
                {
                    brushImage.SetPixel(j, i, Color.White);
                }
            m_program.setBrushImage(brushImage);
        }

        private void button_round_brush_Click(object sender, EventArgs e)
        {
            button_round_brush.BackColor = Color.Black;
            button_square_brush.BackColor = Color.Transparent;

            Bitmap brushImage;
            string dir = Environment.CurrentDirectory + @"//standard_round.png";
            if (System.IO.File.Exists(dir))
            {
                brushImage = new Bitmap(dir);
                for (int i = 0; i < brushImage.Size.Height; i++)
                    for (int j = 0; j < brushImage.Size.Width; j++)
                    {
                        Color c = brushImage.GetPixel(j, i);
                        Color inverted = Color.FromArgb(
                            c.A,
                            byte.MaxValue - c.R,
                            byte.MaxValue - c.G,
                            byte.MaxValue - c.B);
                        brushImage.SetPixel(j, i, inverted);
                    }
            }
            else
            {
                Console.Write("\nDefault brush not found: " + dir + "\nCreating new brush");
                Bitmap img = new Bitmap(30, 30, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.Clear(Color.Black);
                }
                brushImage = img;
                for (int i = 0; i < brushImage.Size.Height; i++)
                    for (int j = 0; j < brushImage.Size.Width; j++)
                    {
                        Color c = brushImage.GetPixel(j, i);
                        if ((j + i) % 2 == 1)
                            brushImage.SetPixel(j, i, Color.Transparent);
                    }
            }
            m_program.setBrushImage(brushImage);
        }

        private void layer_1_Click(object sender, EventArgs e)
        {
            m_program.changeActiveLayer(0);
            layer_1.BackColor = Color.Black;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Transparent;
            
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void layer_2_Click(object sender, EventArgs e)
        {
            m_program.changeActiveLayer(1);
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Black;
            layer_3.BackColor = Color.Transparent;
            
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void layer_3_Click(object sender, EventArgs e)
        {
            m_program.changeActiveLayer(2);
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Black;

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void layer_1_visible_Click(object sender, EventArgs e)
        {
            PictureBox box = layer_1_visible;

            if (m_program.toggleLayerVisiblity(0))
                box.BackColor = Color.Black;
            else
                box.BackColor = Color.Transparent;

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void layer_2_visible_Click(object sender, EventArgs e)
        {
            PictureBox box = layer_2_visible;

            if (m_program.toggleLayerVisiblity(1))
                box.BackColor = Color.Black;
            else
                box.BackColor = Color.Transparent;

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void layer_3_visible_Click(object sender, EventArgs e)
        {
            PictureBox box = layer_3_visible;

            if (m_program.toggleLayerVisiblity(2))
                box.BackColor = Color.Black;
            else
                box.BackColor = Color.Transparent;

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = m_program.getCanvasImageCopy();
            display_canvas.Refresh();
        }

        private void runUnitTests()
        {

            int testsFailed = 0;
            Console.Write("\n==================================");
            Console.Write("\nUNIT TEST STARTED");
            Console.Write("\n==================================");


            RasterBlendUnitTest blendTest = new RasterBlendUnitTest();
            testsFailed += blendTest.run();


            Console.Write("\n\n\n==================================");
            Console.Write("\nUNIT TEST ENDED - TESTS FAILED: " + testsFailed);
            Console.Write("\n==================================");

        }

        private void FormMain_ResizeBegin(object sender, EventArgs e)
        {

        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {

        }
    }
}
